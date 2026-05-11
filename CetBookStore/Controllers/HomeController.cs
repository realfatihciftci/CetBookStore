using CetBookStore.Data;
using CetBookStore.Models;
using CetBookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLitePCL;

namespace CetBookStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
           
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {


            HomePageViewModel homePageViewModel = new HomePageViewModel();


           homePageViewModel.MostCommenteds =await context.Books.
                OrderByDescending(b => b.Comments.Count)
                .ThenByDescending(b => b.Title)
                .Take(3)
                .Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                OldPrice = b.PreviousPrice,
                IsInSale = b.IsInSale,
                ImageUrl = b.ImageUrl
                }).ToListAsync();

            homePageViewModel.NewArrivals = await context.Books
                .OrderByDescending(b => b.CreatedDate)
                .ThenBy(b => b.Title)
                .Take(3)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price,
                    OldPrice = b.PreviousPrice,
                    IsInSale = b.IsInSale,
                    ImageUrl = b.ImageUrl
                }).ToListAsync();


           
                var allBooks = await context.Books.Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price,
                    OldPrice = b.PreviousPrice,
                    IsInSale = b.IsInSale,
                    ImageUrl = b.ImageUrl
                }).ToListAsync();
            homePageViewModel.RandomBoooks = allBooks.OrderBy(b => Guid.NewGuid()).Take(6).ToList();

            return View(homePageViewModel);
        }
        
       
        public IActionResult Privacy()
        {
          
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
