using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rosca_Ilinca_Lab2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rosca_Ilinca_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Rosca_Ilinca_Lab2Context _context;

    public EditModel(Rosca_Ilinca_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing.FirstOrDefaultAsync(m => m.ID == id);
        if (borrowing is null)
        {
            return NotFound();
        }
        Borrowing = borrowing;
        await LoadOptionsAsync();
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadOptionsAsync();
            return Page();
        }

        _context.Attach(Borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BorrowingExists(Borrowing.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }
    private async Task LoadOptionsAsync()
    {
        var members = await _context.Member.ToListAsync();

        var books = await _context.Book
            .Include(book => book.Authors)
            .ToListAsync();

        var bookOptions = books.Select(book => new
        {
            book.ID,
            Text = book.Authors == null
                ? book.Title
                : $"{book.Title} - {book.Authors.FullName}"
        });

        ViewData["MemberID"] = new SelectList(
            members, "ID", "FullName", Borrowing.MemberID);

        ViewData["BookID"] = new SelectList(
            bookOptions, "ID", "Text", Borrowing.BookID);
    }

    private bool BorrowingExists(int id)
    {
        return _context.Borrowing.Any(e => e.ID == id);
    }
}
