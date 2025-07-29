using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<Warehouse> Warehouses { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        //config Warehouse
        modelBuilder.Entity<Warehouse>(entity =>{
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Name).IsRequired().HasMaxLength(100);
            entity.Property(w => w.Capacity).IsRequired();
        });

        //config Product
        modelBuilder.Entity<Product>(entity =>{
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(40);
            entity.Property(p => p.Quantity).IsRequired();

            entity.HasOne(p => p.Warehouse)
                  .WithMany(w => w.Products)
                  .HasForeignKey(p => p.WarehousesId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        //seeding tables
        modelBuilder.Entity<Warehouse>().HasData(
            new Warehouse{Id = 1, Name = "Moscow Warehouse", Capacity = 1500},
            new Warehouse{Id = 2, Name = "Beirut Warehouse", Capacity = 500},
            new Warehouse{Id = 3, Name = "Berlin Warehouse", Capacity = 750}
        );

        modelBuilder.Entity<Product>().HasData(
            new Product{Id = 1, Name = "GTX 1080 Ti" , Quantity = 950, WarehousesId = 1},
            new Product{Id = 2, Name = "Iphone 10", Quantity = 400, WarehousesId = 2},
            new Product{Id = 3, Name = "CPU I9", Quantity = 600, WarehousesId = 3},
            new Product{Id = 4, Name = "RTX 2090", Quantity = 40, WarehousesId = 1},
            new Product{Id = 5, Name = "Screen LG", Quantity = 70, WarehousesId = 2},
            new Product{Id = 6, Name = "Mouse Bloody", Quantity = 30, WarehousesId =3}
        );
    }
}