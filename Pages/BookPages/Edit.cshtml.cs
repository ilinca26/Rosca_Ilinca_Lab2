using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rosca_Ilinca_Lab2.Models;

namespace Rosca_Ilinca_Lab2.Pages.BookPages;

public class EditModel : PageModel
{
    private readonly Rosca_Ilinca_Lab2Context _context;

    public EditModel(Rosca_Ilinca_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = new();

    private void LoadLists()
    {
        ViewData["PublisherID"] = new SelectList(
            _context.Set<Publisher>(),
            "ID",
            "PublisherName",
            Book.PublisherID);

        var authors = _context.Set<Authors>()
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .Select(a => new
            {
                a.ID,
                FullName = a.FirstName + " " + a.LastName
            })
            .ToList();

        ViewData["AuthorsID"] = new SelectList(
            authors,
            "ID",
            "FullName",
            Book.AuthorsID);
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .FirstOrDefaultAsync(b => b.ID == id);

        if (book == null)
        {
            return NotFound();
        }

        Book = book;

        LoadLists();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            LoadLists();
            return Page();
        }

        var bookToUpdate = await _context.Book
            .FirstOrDefaultAsync(b => b.ID == Book.ID);

        if (bookToUpdate == null)
        {
            return NotFound();
        }

        bookToUpdate.Title = Book.Title;
        bookToUpdate.Price = Book.Price;
        bookToUpdate.PublishingDate = Book.PublishingDate;
        bookToUpdate.PublisherID = Book.PublisherID;
        bookToUpdate.AuthorsID = Book.AuthorsID;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}