using Database.Entities.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.Entities
{
    [Index(nameof(Email))]
    [Index(nameof(Id))]
    public class User : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
        public byte[]? Avatar { get; set; }

        [Required]
        [MinLength(6)]
        public byte[] Password { get; set; }    

        public string? PhoneNumber { get; set; }

        [Required]
        [ForeignKey("CountryId")]
        public Guid CountryId { get; set; }
        public Country Country { get; set; }


        public UserBillingInfo BillingInfo { get; set; }
        public UserShippingInfo? ShippingInfo { get; set; }
    }
}
