using Microsoft.EntityFrameworkCore;
using Labb2WebbUtveckling.Models; // Vi behöver referera till modellen för att kunna använda den i databasen

namespace Labb2WebbUtveckling.Data
{
    public class BlommorDbContext : DbContext
    {
        public BlommorDbContext(DbContextOptions<BlommorDbContext> options) : base(options)
        {
        }

        public DbSet<Blomma> Blommor { get; set; }

    }
}
