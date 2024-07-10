using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Base
    {
        [Key]
        public Guid Id { get; set; }    
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime UpdatedOn { get; set;}
    }
}
