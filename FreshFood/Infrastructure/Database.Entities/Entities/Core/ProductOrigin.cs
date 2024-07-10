using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.Core
{
    [NotMapped]
    public class ProductOrigin : Base
    {
        public FarmerInfo Farmer { get; set; }

        public DateTime CollectedOn { get; set; }

    }
}