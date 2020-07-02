namespace SSO.Services.RequestModel.User
{
    public class UserPasswordChangeUpdateRequest
    {

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
        public string CheckPassword { get; set; }
    }
}
