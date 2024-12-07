using Microsoft.EntityFrameworkCore;


namespace NorthwindApp.Models
{
    public class NorthwindContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string ConnectionString = File.ReadAllText(@"..\..\..\Models\ConnectionString.txt");
            optionsBuilder.UseSqlServer(@ConnectionString);
        }
    }
}
