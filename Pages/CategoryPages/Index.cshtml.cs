using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rosca_Ilinca_Lab2.Models;

namespace Rosca_Ilinca_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Rosca_Ilinca_Lab2Context _context;

    public IndexModel(Rosca_Ilinca_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; }
        = new List<Category>();

    public IList<Book> SelectedBooks { get; set; }
        = new List<Book>();

    public int? SelectedCategoryID { get; set; }

    public string SelectedCategoryName { get; set; } = "";

    public async Task OnGetAsync(int? id)
    {
        
        Category = await _context.Set<Category>()
            .OrderBy(c => c.CategoryName)
            .AsNoTracking()
            .ToListAsync();

        if (!id.HasValue)
        {
            return;
        }

        
        var selectedCategory = Category
            .SingleOrDefault(c => c.ID == id.Value);

        if (selectedCategory == null)
        {
            return;
        }

        SelectedCategoryID = selectedCategory.ID;
        SelectedCategoryName = selectedCategory.CategoryName;

        
        SelectedBooks = await _context.Book
            .Include(b => b.Authors)
            .Where(b => b.BookCategories.Any(
                bc => bc.Category.ID == id.Value))
            .OrderBy(b => b.Title)
            .AsNoTracking()
            .ToListAsync();
    }
}