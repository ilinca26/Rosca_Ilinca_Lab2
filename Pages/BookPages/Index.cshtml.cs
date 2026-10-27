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

    public string TitleSort { get; set; } = "";

    public string AuthorSort { get; set; } = "";

    public string CurrentFilter { get; set; } = "";

    [BindProperty(SupportsGet = true)]
    public int? SelectedAuthorID { get; set; }

    public SelectList AuthorsList { get; set; } = default!;

    public async Task OnGetAsync(
        int? id,
        int? categoryID,
        string? sortOrder,
        string? searchString)
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

        TitleSort = string.IsNullOrEmpty(sortOrder)
            ? "title_desc"
            : "";

        AuthorSort = sortOrder == "author"
            ? "author_desc"
            : "author";

        CurrentFilter = searchString ?? "";

        var books = _context.Book
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .AsQueryable();

        if (SelectedAuthorID.HasValue)
        {
            books = books.Where(
                b => b.AuthorsID == SelectedAuthorID.Value);
        }

        Book = await books
            .OrderBy(b => b.Title)
            .ToListAsync();

        BookD = new BookData();

        BookD.Books = Book;
        BookD.Categories = new List<Category>();
        BookD.BookCategories = new List<BookCategory>();

        // Pasul 12: căutare după titlu sau numele autorului.
        if (!string.IsNullOrEmpty(searchString))
        {
            BookD.Books = BookD.Books.Where(b =>
                (b.Title != null && b.Title.Contains(searchString))
                ||
                (b.Authors != null &&
                    (
                        (b.Authors.FirstName != null &&
                         b.Authors.FirstName.Contains(searchString))
                        ||
                        (b.Authors.LastName != null &&
                         b.Authors.LastName.Contains(searchString))
                    ))
            );
        }

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

        if (categoryID.HasValue)
        {
            CategoryID = categoryID.Value;
        }

        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(b => b.Title);
                break;

            case "author_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(b => b.Authors == null
                        ? ""
                        : b.Authors.FirstName + " " + b.Authors.LastName);
                break;

            case "author":
                BookD.Books = BookD.Books
                    .OrderBy(b => b.Authors == null
                        ? ""
                        : b.Authors.FirstName + " " + b.Authors.LastName);
                break;

            default:
                BookD.Books = BookD.Books
                    .OrderBy(b => b.Title);
                break;
        }

        Book = BookD.Books.ToList();
    }
}