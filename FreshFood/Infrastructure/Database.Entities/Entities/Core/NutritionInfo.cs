using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.Core
{
    [NotMapped]
    public class NutritionInfo : Base
    {
        public string Portion { get; set; }
        public float Calories { get; set; }
        public float CarboHydrates { get; set; }
        public float Proteins { get; set; }
        public float DietaryFat { get; set; }
        public float SaturedFat { get; set; }
        public float TransFat { get; set; }
        public float DietaryFiber { get; set; }
        public float Salt { get; set; }
    }
}