namespace CetBookStore.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string CetUserId { get; set; }
        public virtual CetUser? CetUser { get; set; }

        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

        public int Count { get; set; }

        public DateTime AddedDate { get; set; }
  
    }
}
