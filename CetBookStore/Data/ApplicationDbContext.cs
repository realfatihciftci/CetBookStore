using CetBookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CetBookStore.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<CetUser>(options)
    {
        
            public DbSet<Book> Books { get; set; }
            public DbSet<Category> Categories { get; set; }
           public DbSet<Comment> Comments { get; set; }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    }
}
