using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RYT.Data;
using RYT.Models.Entities;
using RYT.Models.Enums;
using RYT.Models.ViewModels;
using RYT.Services.Emailing;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;
using RYT.Models.Enums;
using RYT.Services.Repositories;
using RYT.Services.CloudinaryService;
using CloudinaryDotNet.Actions;



namespace RYT.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IRepository _repository;
        private readonly IPhotoService _photoService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IEmailService emailService, IRepository repository, IPhotoService photoService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _repository = repository;
            _photoService = photoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(StudentSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = _mapper.Map<AppUser>(model);
                var user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                };

                user.Wallet = new Wallet
                {
                    UserId = user.Id,
                    Balance = 0,
                    Status = WalletStatus.Active
                };

                var emailToCheck = await _userManager.FindByEmailAsync(model.Email);
                if (emailToCheck == null)
                {
                    var createUser = await _userManager.CreateAsync(user, model.Password);
                    if (createUser.Succeeded)
                    {
                        var addRole = await _userManager.AddToRoleAsync(user, "student");
                        if (addRole.Succeeded)
                        {
                            // send email confirmation link
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var link = Url.Action("ConfirmEmail", "Account", new { user.Email, token }, Request.Scheme);
                            var body = @$"Hi{user.FirstName},
Please click the link <a href='{link}'>here</a> to confirm your account's email";
                            await _emailService.SendEmailAsync(user.Email, "Confirm Email", body);

                            return RedirectToAction("RegisterCongrats", "Account", new { name = user.FirstName });
                        }

                        foreach (var err in addRole.Errors)
                        {
                            ModelState.AddModelError(err.Code, err.Description);

                        }
                    }
                    foreach (var err in createUser.Errors)
                    {
                        ModelState.AddModelError(err.Code, err.Description);
                    }
                }
                ModelState.AddModelError("", "email already exists");


            }
            return View(model);
        }

       

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string Email, string token)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, token);
                if (confirmEmailResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var err in confirmEmailResult.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }

                return View(ModelState);
            }

            ModelState.AddModelError("", "Email confirmation failed");

            return View(ModelState);
        }

        [HttpGet]
        public IActionResult TeacherSignUp()
        {
            var model = new TeacherSignUpStep1ViewModel();
            model.SchoolsTaught = SeedData.Schools;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TeacherSignUp(TeacherSignUpStep1ViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                ModelState.AddModelError("Email", "Email already exists");

                return View("TeacherSignUp", model);
            }

            HttpContext.Session.SetString("TeacherSignUpStep1ViewModel", JsonSerializer.Serialize(model));

            return RedirectToAction("TeacherSignUpStep2");
        }

        [HttpGet]
        public IActionResult TeacherSignUpStep2()
        {
            var model = new TeacherSignUpStep2ViewModel();
            model.ListOfSchoolTypes = SeedData.SchoolTypes;
            model.listOfSubjectsTaught = SeedData.Subjects;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TeacherSignUpStep2(TeacherSignUpStep2ViewModel model)
        {
            /*var step1ViewModel =
                JsonSerializer.Deserialize<TeacherSignUpStep1ViewModel>(
                    HttpContext.Session.GetString("TeacherSignUpStep1ViewModel"));
            HttpContext.Session.Clear();*/

            var step1ViewModelJson = HttpContext.Session.GetString("TeacherSignUpStep1ViewModel");
            if (step1ViewModelJson is null)
            {
                return RedirectToAction("TeacherSignUp");
            }
            var step1ViewModel = JsonSerializer.Deserialize<TeacherSignUpStep1ViewModel>(step1ViewModelJson);
            HttpContext.Session.Clear();

            if (step1ViewModel is null)
            {
                return RedirectToAction("TeacherSignUp");
            }

            if (ModelState.IsValid)
            {
                var NINUploadImageResult = await _photoService.UploadImage(model.NINUploadImage, step1ViewModel.Name);

                if (NINUploadImageResult.ContainsKey("Code") && NINUploadImageResult["Code"] == "200")
                {
                    var user = new AppUser
                    {
                        FirstName = step1ViewModel.FirstName,
                        LastName = step1ViewModel.LastName,
                        Email = step1ViewModel.Email,
                        UserName = step1ViewModel.Email,
                        NameofSchool = step1ViewModel.SelectedSchool,
                    };

                    var createUserResult = await _userManager.CreateAsync(user, step1ViewModel.Password);
                    if (createUserResult.Succeeded)
                    {
                        // Add Role to teacher
                        var addRoleResult = await _userManager.AddToRoleAsync(user, "teacher");
                        if (addRoleResult.Succeeded)
                        {

                            var teacher = new Teacher
                            {
                                UserId = user.Id,
                                YearsOfTeaching = model.YearsOfTeaching,
                                SchoolType = model.SelectedSchoolType,
                                NINUploadUrl = NINUploadImageResult["Url"], // added url returned from cloudinary
                                NINUploadPublicId = NINUploadImageResult["PublicId"] // added public id returned from cloudinary
                            };
                            await _repository.AddAsync(teacher);

                            var teacherSubject = new SubjectsTaught
                            {
                                TeacherId = teacher.Id,
                                Subject = model.SelectedSubject
                            };
                            await _repository.AddAsync(teacherSubject);

                            var teacherSchool = new SchoolsTaught
                            {
                                TeacherId = teacher.Id,
                                School = step1ViewModel.SelectedSchool
                            };
                            await _repository.AddAsync(teacherSchool);

                            var wallet = new Wallet
                            {
                                UserId = user.Id,
                                Balance = 0,
                                Status = WalletStatus.Active
                            };
                            await _repository.AddAsync(wallet);

                            // send email confirmation link
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var link = Url.Action("ConfirmEmail", "Account", new { user.Email, token }, Request.Scheme);
                            string body = @$"Hi{user.FirstName},
Please click the link <a href='{link}'>here</a> to confirm your account's email";
                            await _emailService.SendEmailAsync(user.Email, "Confirm Email", body);

                            return RedirectToAction("RegisterCongrats", "Account", new { name = user.FirstName });
                        }
                        foreach (var err in addRoleResult.Errors)
                        {
                            ModelState.AddModelError(err.Code, err.Description);
                        }
                    }

                    foreach (var err in createUserResult.Errors)
                    {
                        ModelState.AddModelError(err.Code, err.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Failed to upload NIN.");
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult RegisterCongrats(string name)
        {
            ViewBag.Name = name;
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string Email, String Token)
        {
            var resetPasswordModel = new ResetPasswordViewModel { Email = Email, Token = Token };
            return View(resetPasswordModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

              var user =   await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                   var resetPasswordResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                    if (resetPasswordResult.Succeeded)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in resetPasswordResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        return View(model);
                    }
                }
                ModelState.AddModelError("", "Email Not Recognized");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (await _userManager.IsEmailConfirmedAsync(user))
                    {
                        var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                        if (loginResult.Succeeded)
                        {
                            var userRoles = await _userManager.GetRolesAsync(user);
                            if (string.IsNullOrEmpty(returnUrl))
                            {
                                if(userRoles.Any(x => x.ToLower().Equals("teacher")) ||
                                    userRoles.Any(x => x.ToLower().Equals("student")))
                                {
                                    return RedirectToAction("overview", "dashboard");

                                }
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                if (userRoles.Any(x => x.ToLower().Equals("teacher")) ||
                                    userRoles.Any(x => x.ToLower().Equals("student")))
                                {
                                    return LocalRedirect(returnUrl);

                                }
                                return RedirectToAction("Index", "Home");
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Email or Password is incorrect");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email is not yet Confirmed");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var link = Url.Action("ResetPassword", "Account", new { user.Email, token }, Request.Scheme );
                    var body = @$"Hi{user.FirstName}{user.LastName},
						please, click the link <a href='{link}'>here</a> to reset your password";

                    await _emailService.SendEmailAsync(user.Email, "Forgot Password", body);

                    ViewBag.Message = "Password Reset details has been sent to your email";
                    return View();
                }

                ModelState.AddModelError("", "Invalid Email");
            }

            return View(model);
        }
    }
}