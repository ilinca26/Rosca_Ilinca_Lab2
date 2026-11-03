using Microsoft.EntityFrameworkCore;
using Rosca_Ilinca_Lab2.Models;

public class Rosca_Ilinca_Lab2Context : DbContext
{
    public Rosca_Ilinca_Lab2Context(DbContextOptions<Rosca_Ilinca_Lab2Context> options)
        : base(options)
    {
    }

    public DbSet<Book> Book { get; set; } = default!;
    public DbSet<Publisher> Publisher { get; set; } = default!;
    public DbSet<Rosca_Ilinca_Lab2.Models.Authors> Authors
    { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<BookCategory> BookCategory { get; set; } = default!;
    public DbSet<Borrowing> Borrowing { get; set; } = default!;
    public DbSet<Member> Member { get; set; } = default!;
}
