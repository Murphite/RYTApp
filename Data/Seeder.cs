using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RYT.Models.Entities;

namespace RYT.Data
{
	public static class Seeder
	{
		public static void SeedeMe(IApplicationBuilder app)
		{
			var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RYTDbContext>();
			var userMgr = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService< UserManager<AppUser>>();
			var roleMgr = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.MigrateAsync().Wait();
			}

			try
			{
                if (!roleMgr.Roles.Any())
                {
                    foreach (var role in SeedData.Roles)
                    {
                        roleMgr.CreateAsync(new IdentityRole(role)).Wait();
                    }
                }

                if (!userMgr.Users.Any())
                {
                    int counter = 0;
                    foreach (var user in SeedData.Users)
                    {
                        user.EmailConfirmed = true;
                        userMgr.CreateAsync(user, "P@ssw0rd1").Wait();
                        context.Add(new Wallet
                        {
                            Balance = 0,
                            UserId = user.Id
                        });
                        context.SaveChanges();
                        if (counter < 1)
                        {
                            userMgr.AddToRoleAsync(user, "admin").Wait();
                        }
                        else
                        {
                            var r = new Random().Next(1, 3);
                            userMgr.AddToRoleAsync(user, SeedData.Roles[r]).Wait();
                        }
                        counter++;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
	}
}

