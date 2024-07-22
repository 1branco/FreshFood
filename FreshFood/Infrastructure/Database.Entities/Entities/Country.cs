using Database.Entities.Entities.Base;

namespace Database.Entities.Entities
{
    public class Country : BaseEntity
    {
        public string IsoAlphaCode2 { get; set; }

        public string IsoAlphaCode3 { get; set; }

        public string Name { get; set; }
    }
}
