using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;
public class OrganicContext : DbContext
{
    public OrganicContext(DbContextOptions options):base(options){}
    public DbSet<Department> Departments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Cart> Carts { get; set; }
}