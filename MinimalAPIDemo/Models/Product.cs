using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalAPIDemo.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName="nvarchar(100)")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Price field is required")]
        [Range(100,10000,ErrorMessage ="Price range should be between 200 to 1000")]
        [Column(TypeName= "decimal(7,2)")]        
        public decimal Price { get; set; }

        [Required(ErrorMessage ="Description is required")]
        [StringLength(100,ErrorMessage ="Description can't be more than 50 characters")]
        [Column(TypeName ="varchar(100)")]
        public string Description { get; set; }

    }
}
