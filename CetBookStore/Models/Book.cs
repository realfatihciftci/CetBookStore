using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CetBookStore.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }       
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.Html)]
        [StringLength(2000, MinimumLength = 10)]
        public string Description { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Author { get; set; }
        [StringLength(100,MinimumLength =3)]
        public string Publisher { get; set; }
        [Range(1, 10000)]
        public int PageCount { get; set; }
        public Decimal Price { get; set;  }
        public bool IsInSale   { get; set; }
        public Decimal PreviousPrice { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CategoryId { get; set; }

        public string? ImageUrl { get; set; }= null;

        [NotMapped]
        public IFormFile? ImageFile { get; set; } = null;

        public virtual Category? Category { get; set; }

        public virtual List<Comment>? Comments { get; set; }

        


    }
}
