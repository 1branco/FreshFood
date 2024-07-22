using Database.Entities.Entities.Base;

namespace Database.Entities.Entities
{
    public class UserShippingInfo : BaseEntity
    {
        public string State { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
    }
}
