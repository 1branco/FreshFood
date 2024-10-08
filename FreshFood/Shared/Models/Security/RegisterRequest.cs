﻿namespace Shared.Models.Security
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string VatNumber { get; set; }
    }
}
