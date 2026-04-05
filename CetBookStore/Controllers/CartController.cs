using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CetBookStore.Data; 
using CetBookStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace CetBookStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var cartItems = await _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.ApplicationUserId == userId)
                .ToListAsync();

            return View(cartItems);
        }

        public async Task<IActionResult> AddToCart(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.ApplicationUserId == userId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var newItem = new CartItem
                {
                    BookId = bookId,
                    ApplicationUserId = userId,
                    Quantity = 1,
                    // Kitap fiyatını Book modelinden alıp sepet itemına atıyoruz
                    Price = book.Price 
                };
                _context.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Purchase()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.CartItems.Include(c => c.Book)
                .Where(c => c.ApplicationUserId == userId).ToListAsync();

            if (cartItems.Count == 0) return RedirectToAction("Index");
            
            var order = new Order
            {
                OrderDate = DateTime.Now,
                ApplicationUserId = userId,
                TotalAmount = cartItems.Sum(x => x.Quantity * x.Price)
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("MyOrders");
        }
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Book)
                .Where(o => o.ApplicationUserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
    }
}

