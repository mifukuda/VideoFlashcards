namespace Server.Dtos
{
    // Data transfer object for registering a user
    public partial class UserForRegistrationDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string PasswordConfirm { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}