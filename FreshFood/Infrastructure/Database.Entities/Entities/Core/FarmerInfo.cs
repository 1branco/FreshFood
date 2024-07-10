using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.Core
{
    [NotMapped]
    public class FarmerInfo : Base
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
    }
}