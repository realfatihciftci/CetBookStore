using CetBookStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CetBookStore.Components
{
    public class DynamicMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext context;

        public DynamicMenuViewComponent(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult>InvokeAsync()
        {
            var categories =await context.Categories.Where(c => c.IsVisibleInMenu).OrderBy(c => c.Name).ToListAsync();
            return View(categories);
        }
    }
}
