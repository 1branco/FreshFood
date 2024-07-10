using Database.Entities.Security;

namespace Database.Entities.Customer
{
    public class Customer : Base
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public CustomerBillingInfo BillingInfo { get; set; }
        public string? Image { get; set; }

        /// <summary>
        /// Customer can have multiple credentials set up, but only 1 credential must be active
        /// </summary>
        public IList<Credential> Credentials { get; set; }
    }
}
