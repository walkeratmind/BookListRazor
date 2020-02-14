using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListRazor.Modal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookListRazor
{
    public class UpsertModel : PageModel
    {
        private ApplicationDbContext _db;

        public UpsertModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Book Book { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            Book = new Book();
            if(id == null)
            {
                // For create
                return Page();
            }



            // update
            Book = await _db.Book.FirstOrDefaultAsync(u => u.Id == id);

            //or
            //var BookFromDb = await _db.Book.FindAsync(Book.Id);

            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if(Book.Id == 0)
                {

                    await _db.Book.AddAsync(Book);

                } else
                {
                    var BookFromDb = await _db.Book.FindAsync(Book.Id);
                    BookFromDb.Name = Book.Name;
                    BookFromDb.Author = Book.Author;
                    BookFromDb.ISBN = Book.ISBN;
                    BookFromDb.Price = Book.Price;
                    //_db.Book.Update(Book);
                }


                await _db.SaveChangesAsync();

                return RedirectToPage("Index");

            }
            else
            {
                return Page();
            }
        }
    }
}