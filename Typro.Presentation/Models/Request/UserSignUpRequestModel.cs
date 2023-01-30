namespace Typro.Presentation.Models.Request
{
    public class UserSignUpRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
