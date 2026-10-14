using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rosca_Ilinca_Lab2.Models;

namespace Rosca_Ilinca_Lab2.Pages.PublisherPages;

public class DetailsModel : PageModel
{
    private readonly Rosca_Ilinca_Lab2Context _context;

    public DetailsModel(Rosca_Ilinca_Lab2Context context)
    {
        _context = context;
    }

    public Publisher Publisher { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Publisher = await _context.Publisher
            .Include(p => p.Books)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (Publisher is null)
        {
            return NotFound();
        }
        return Page();
    }
}
