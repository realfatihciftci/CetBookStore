using System.ComponentModel.DataAnnotations;

namespace CetBookStore.Models
{
    public class Comment
    {
        public int Id { get; set; }


     
        [MaxLength(100)]
        public string UserName { get; set; } =null!;

        [Required]
        [MaxLength(2000)]
       
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
       
        public int BookId { get; set; }
        // Navigation properties

        public virtual Book? Book { get; set; } = null!;
    }
}
