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

    [BindProperty(SupportsGet = true)]
    public int? SelectedAuthorID { get; set; }

    public SelectList AuthorsList { get; set; } = default!;

    public async Task OnGetAsync()
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
            .AsQueryable();

        if (SelectedAuthorID.HasValue)
        {
            books = books.Where(
                b => b.AuthorsID == SelectedAuthorID.Value);
        }

        Book = await books.ToListAsync();
    }
}
