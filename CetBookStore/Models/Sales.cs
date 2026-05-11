using CetBookStore.Controllers;

namespace CetBookStore.Models
{
    public class Sale
    {
        public int Id { get; set; }
       
        public Decimal TotalPrice { get; set; }

        public DateTime SalesDate { get; set; } = DateTime.Now;
        public string CetUserId { get; set; }
        public virtual CetUser? CetUser { get; set; }

        public string? CardOnName { get; set; }
        public string? CardNumber { get; set; }
        
        public string? Address { get; set; }

        public virtual List<SaleItem>? SaleItems { get; set; }

    }

    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public virtual Sale? Sale { get; set; }
        public int BookId { get; set; }
        public virtual Book? Book { get; set; }
        public int Count { get; set; }
        public Decimal Price { get; set; }
    }
}
