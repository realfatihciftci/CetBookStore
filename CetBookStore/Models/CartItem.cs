using System.ComponentModel.DataAnnotations;
namespace CetBookStore.Models
{
    public class CartItem
    {
    public int Id { get; set; }
    [Required]
    public int BookId { get; set; }
    public Book? Book { get; set; }
    [Required]
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? ApplicationUserId { get; set; }
    }  
}

