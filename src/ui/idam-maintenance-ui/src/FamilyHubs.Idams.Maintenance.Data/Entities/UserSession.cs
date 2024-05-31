namespace FamilyHubs.Idams.Maintenance.Data.Entities
{
    public class UserSession : EntityBase<long>
    {
        public required string Email { get; set; }

        /// <summary>
        /// This is session id provided by OneLogin
        /// </summary>
        public required string Sid { get; set; }

        public DateTime? LastActive { get; set; }
    }
}
