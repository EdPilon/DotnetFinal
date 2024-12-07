using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, MaxLength(15)]
        public string CategoryName { get; set; }

        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
