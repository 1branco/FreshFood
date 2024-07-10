using System.ComponentModel.DataAnnotations.Schema;
using Database.Enums;

namespace Database.Entities.Core
{
    [NotMapped]
    public class Product : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ProductTypeEnum ProductType { get; set; }

        /// <summary>
        /// Price by unit, in case the product is going to be sold by unit
        /// </summary>
        public float UnitPrice { get; set; }

        /// <summary>
        /// Total price, in case the product is goind to be sold within a "package"
        /// </summary>
        public float TotalPrice { get; set; }

        /// <summary>
        /// Origin of the product comes from
        /// </summary>
        public ProductOrigin ProductOrigin { get; set; }

        /// <summary>
        /// Nutrition information regarding the product, how many calories has, etc...
        /// </summary>
        public NutritionInfo NutritionInfo { get; set; }

        /// <summary>
        /// Reference to, for example, how the product can be cooked, how must be stored ...
        /// </summary>
        public string AdditionalNotes { get; set; }
    }
}
