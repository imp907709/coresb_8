using System.Linq;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureCheckers.Vit
{
    public class SampleDataVit {
        
        
        public class VitContext : DbContext
        {
            public  DbSet<Product> products;
            public DbSet<PriceDetail> prices;

            public VitContext(DbContextOptions<DbContext> options) : base(options)
            {
            
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);
            }
            
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }
        }
    
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public virtual ICollection<PriceDetail> PriceDetails { get; set; }
        }
        
        public class PriceDetail    
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public decimal Price { get; set; }
            public string Segment { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }
            public virtual Product Product { get; set; }
        }
    }
}
