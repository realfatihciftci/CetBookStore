namespace CetBookStore.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public bool IsInSale { get; set; }

        public string? ImageUrl { get; set; }
   


    }
    public class HomePageViewModel
    {


        public List<BookViewModel> MostCommenteds { get; set; }=new List<BookViewModel>();
        public List<BookViewModel> NewArrivals { get; set; }= new List<BookViewModel>();
        public List<BookViewModel> RandomBoooks { get; set; } = new List<BookViewModel>();
    }
}
