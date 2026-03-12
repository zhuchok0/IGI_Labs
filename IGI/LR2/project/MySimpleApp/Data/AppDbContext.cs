using Microsoft.EntityFrameworkCore;
using MySimpleApp.Models;

namespace MySimpleApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Item> Items { get; set; }
}