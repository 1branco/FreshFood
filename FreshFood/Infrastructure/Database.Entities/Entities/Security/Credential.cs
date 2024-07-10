using Database.Enums;

namespace Database.Entities.Security
{
    public class Credential : Base
    {
        public CredentialTypeEnum Type { get; set; }
        public string? Value { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
    }
}
