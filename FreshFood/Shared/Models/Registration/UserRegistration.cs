using System.Numerics;

namespace Models.Registration
{
    public class UserRegistration
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public byte[] Avatar{ get; set; }

        public byte[] Password { get; set; }

        public Guid CountryId { get; set; }

        public string State { get; set; }

        public string Address { get; set; }

        public string Zipcode { get; set; }

        public string Cellphone { get; set; }

        public string VATNumber { get; set; }

        public string City { get; set; }
    }
}
