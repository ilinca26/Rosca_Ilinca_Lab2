using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rosca_Ilinca_Lab2.Models;

namespace Rosca_Ilinca_Lab2.Pages.PublisherPages;

public class DeleteModel : PageModel
{
    private readonly Rosca_Ilinca_Lab2Context _context;

    public DeleteModel(Rosca_Ilinca_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Publisher Publisher { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var publisher = await _context.Publisher
            .Include(p => p.Books)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (publisher is null)
        {
            return NotFound();
        }

        Publisher = publisher;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var publisher = await _context.Publisher.FindAsync(id);

        if (publisher is not null)
        {
            _context.Publisher.Remove(publisher);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
