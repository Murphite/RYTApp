namespace RYT.Models.ViewModels
{
    public class TeacherSignUpStep2GetViewModel
    {
        public IList<string> listOfSchoolTypes { get; set; } = new List<string>();

        public IList<string> listOfSubjectsTaught { get; set; } = new List<string>();
    }
}
