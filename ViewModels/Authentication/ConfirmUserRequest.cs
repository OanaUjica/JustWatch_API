
namespace Lab1_.NET.ViewModels.Authentication
{
    public class ConfirmUserRequest
    {
        public string Email { get; set; }

        public string ConfirmationToken { get; set; }
    }
}
