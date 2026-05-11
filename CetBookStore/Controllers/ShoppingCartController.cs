using CetBookStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CetBookStore.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {


            var userId = context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)!.Id;
            var shoppingCartItems = await context.ShoppingCarts
                .Where(s => s.CetUserId == userId)
                .Include(s => s.Book)
                .ToListAsync();

            return View(shoppingCartItems);
        }

        public async Task<IActionResult> AddToCart(int bookId, int count=1 )
        {
            var userId = context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)!.Id;
             var shoppingCartItem = await context.ShoppingCarts
                .Where(s => s.BookId == bookId && s.CetUserId == userId).FirstOrDefaultAsync();
            if (shoppingCartItem != null)
            {
                shoppingCartItem.Count += count;
                shoppingCartItem.AddedDate = DateTime.Now;
            }
            else { 

                shoppingCartItem = new Models.ShoppingCart
                {
                    BookId = bookId,
                    CetUserId = userId,
                    Count = count,
                    AddedDate = DateTime.Now
                };
                context.ShoppingCarts.Add(shoppingCartItem);
            }
            await context.SaveChangesAsync();
            // Logic to add the book to the shopping cart
            // You can use the context to retrieve the book and add it to the cart
            return RedirectToAction("Index");
        }


    }
}
