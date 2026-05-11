using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CetBookStore.Data;
using CetBookStore.Models;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CetBookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Books.Include(b => b.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Author,Publisher,PageCount,Price,IsInSale,PreviousPrice,PublicationDate,CategoryId,ImageFile")] Book book)
        {
            if (ModelState.IsValid)
            {

                if (book.ImageFile != null)
                {

                    var fileExtension = Path.GetExtension(book.ImageFile.FileName);
                    var newFileName = Guid.NewGuid().ToString("N") + fileExtension;

                    // Resim boyutlandırma ve kaydetme
                    using (var image = Image.Load(book.ImageFile.OpenReadStream()))
                    {
                        if (image.Width > 1024)
                        {
                            image.Mutate(x => x.Resize(1024, 0)); // Genişliği 1024'e ayarla, yüksekliği orantılı olarak ayarla
                        }
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", newFileName);
                        await image.SaveAsync(imagePath);
                    }
                    
                    book.ImageUrl = newFileName;  
                    _context.Add(book);
                     await _context.SaveChangesAsync();
                     return RedirectToAction(nameof(Index));
                } else                 {
                    ModelState.AddModelError("ImageFile", "Please upload an image.");

                }
              
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }
        [Authorize]
        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Author,Publisher,PageCount,Price,IsInSale,PreviousPrice,PublicationDate,CreatedDate,CategoryId,ImageFile,ImageUrl")] Book book)
        {
            if (id != book.Id)
                return NotFound();

            // Mevcut kitabı veritabanından al
            var existingBook = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            if (existingBook == null)
                return NotFound();

            // Eğer yeni bir resim yüklendiyse
            if (book.ImageFile != null)
            {
                // Eski resmi sil
                if (!string.IsNullOrEmpty(existingBook.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", existingBook.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Yeni resmi kaydet ve boyutlandır
                var fileExtension = Path.GetExtension(book.ImageFile.FileName);
                var newFileName = Guid.NewGuid().ToString("N") + fileExtension;

                using (var image = Image.Load(book.ImageFile.OpenReadStream()))
                {
                    if (image.Width > 1024)
                    {
                        image.Mutate(x => x.Resize(1024, 0)); // Genişliği 1024'e ayarla, yüksekliği orantılı olarak ayarla
                    }
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", newFileName);
                    await image.SaveAsync(imagePath);
                }
                book.ImageUrl = newFileName; // Yeni resim URL'sini ata
            }
            else
            {
                // Yeni resim yüklenmediyse, mevcut resim URL'sini koru
                book.ImageUrl = existingBook.ImageUrl;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // CreatedDate'i koru
                    book.CreatedDate = existingBook.CreatedDate;
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                // Kitap silinirken resmi de sil
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", book.ImageUrl);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        [Authorize]
        public async Task<IActionResult> Buy(int id, int count=1)
        {
            if(count<1)
            {
                return BadRequest();
            }
            var book = await _context.Books.FindAsync(id);
            if(book==null)
            {                
                return NotFound();
            }
            
           // Sale sale = new Sale();
           // sale.CetUserId =_context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
            //sale.BookId = id;

           // sale.TotalPrice = book.Price*count;
           // sale.SalesDate = DateTime.Now;
           // sale.Count = count;
           // _context.Sales.Add(sale);
           // await _context.SaveChangesAsync();
            return RedirectToAction( "Index", "Home");

        }
        public async Task<IActionResult> CommentCreate([Bind("Id,UserName,Content,BookId")] Comment comment)
        {

            comment.CreatedDate = DateTime.Now;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                comment.UserName = HttpContext.User.Identity.Name;

            }
           

           
                _context.Add(comment);
                await _context.SaveChangesAsync();
                
            

            return RedirectToAction(nameof(Details),new { id = comment.BookId });
        }
    }
}
