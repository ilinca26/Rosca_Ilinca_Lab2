using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rosca_Ilinca_Lab2.Models;
using Rosca_Ilinca_Lab2.Models.ViewModels;

namespace Rosca_Ilinca_Lab2.Pages.PublisherPages;

public class IndexModel : PageModel
{
    private readonly Rosca_Ilinca_Lab2Context _context;

    public IndexModel(Rosca_Ilinca_Lab2Context context)
    {
        _context = context;
    }

    public IList<Publisher> Publisher { get; set; } = default!;

    public PublisherIndexData PublisherData { get; set; }
    public int PublisherID { get; set; }
    public int BookID { get; set; }
    public async Task OnGetAsync(int? id, int? bookID)
    {
        PublisherData = new PublisherIndexData();
        PublisherData.Publishers = await _context.Publisher
        .Include(i => i.Books)
        .ThenInclude(c => c.Authors)
        .OrderBy(i => i.PublisherName)
        .ToListAsync();
        if (id != null)

        {
            PublisherID = id.Value;
            Publisher publisher = PublisherData.Publishers
            .Where(i => i.ID == id.Value).Single();
            PublisherData.Books = publisher.Books;

        }

    }
}
