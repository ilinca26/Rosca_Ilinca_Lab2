
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rosca_Ilinca_Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rosca_Ilinca_Lab2.Pages.BookPages
{
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Rosca_Ilinca_Lab2Context _context;

        public CreateModel(Rosca_Ilinca_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public IActionResult OnGet()
        {
            ViewData["AuthorsID"] = new SelectList(
                _context.Authors,
                "ID",
                "ID");

            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "ID",
                "PublisherName");

            var book = new Book();
            book.BookCategories = new List<BookCategory>();

            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            string[] selectedCategories)
        {
            var newBook = new Book();

            newBook.BookCategories = new List<BookCategory>();

            if (selectedCategories != null)
            {
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };

                    newBook.BookCategories.Add(catToAdd);
                }
            }

            Book.BookCategories = newBook.BookCategories;

            _context.Book.Add(Book);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
