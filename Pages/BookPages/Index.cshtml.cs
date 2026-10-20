
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rosca_Ilinca_Lab2.Models;

namespace Rosca_Ilinca_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Rosca_Ilinca_Lab2Context _context;

    public IndexModel(Rosca_Ilinca_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = new List<Book>();

    public BookData BookD { get; set; } = new BookData();

    public int BookID { get; set; }

    public int CategoryID { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SelectedAuthorID { get; set; }

    public SelectList AuthorsList { get; set; } = default!;

    public async Task OnGetAsync(int? id, int? categoryID)
    {
        var authors = await _context.Set<Authors>()
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .Select(a => new
            {
                a.ID,
                FullName = a.FirstName + " " + a.LastName
            })
            .ToListAsync();

        AuthorsList = new SelectList(
            authors,
            "ID",
            "FullName",
            SelectedAuthorID);

        var books = _context.Book
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .AsQueryable();

        // Pastram filtrarea dupa autor
        if (SelectedAuthorID.HasValue)
        {
            books = books.Where(
                b => b.AuthorsID == SelectedAuthorID.Value);
        }

        // Incarcam cartile ordonate dupa titlu
        Book = await books
            .OrderBy(b => b.Title)
            .ToListAsync();

        // Datele necesare pentru punctul 20
        BookD = new BookData();

        BookD.Books = Book;

        BookD.Categories = new List<Category>();

        BookD.BookCategories = new List<BookCategory>();

        // Selectarea unei carti
        if (id.HasValue)
        {
            BookID = id.Value;

            var selectedBook = BookD.Books
                .SingleOrDefault(b => b.ID == id.Value);

            if (selectedBook != null)
            {
                BookD.Categories = selectedBook.BookCategories?
                    .Select(bc => bc.Category)
                    .ToList() ?? new List<Category>();
            }
        }

        // Selectarea unei categorii
        if (categoryID.HasValue)
        {
            CategoryID = categoryID.Value;
        }
    }
}
