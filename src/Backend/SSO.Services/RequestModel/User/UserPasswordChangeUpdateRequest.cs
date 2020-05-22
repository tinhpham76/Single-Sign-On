namespace SSO.Services.RequestModel.User
{
    public class UserPasswordChangeUpdateRequest
    {
        public string UserId { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
