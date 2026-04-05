using System.ComponentModel.DataAnnotations;

namespace CetBookStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ApplicationUserId { get; set; }
        public decimal TotalAmount { get; set; }
        
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}