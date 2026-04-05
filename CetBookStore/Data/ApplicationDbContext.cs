using CetBookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CetBookStore.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
            public DbSet<Book> Books { get; set; }
            public DbSet<Category> Categories { get; set; }
           public DbSet<Comment> Comments { get; set; }
           public DbSet<CartItem> CartItems { get; set; }
           public DbSet<Order> Orders { get; set; }
           public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
