using Microsoft.AspNetCore.Identity;

namespace CetBookStore.Models
{
    public class CetUser:IdentityUser
    {
        public virtual List<Sale>? Sales { get; set; }

        public virtual List<ShoppingCart>? ShoppingCarts { get; set; }

    }
}
