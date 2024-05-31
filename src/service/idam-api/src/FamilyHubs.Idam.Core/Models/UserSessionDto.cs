namespace FamilyHubs.Idam.Core.Models
{
    public class UserSessionDto
    {
        public required string Email { get; set; }

        /// <summary>
        /// This is the id provided by OneLogin
        /// </summary>
        public required string Sid { get; set; }

        public DateTime LastActive { get; set; }
    }
}
