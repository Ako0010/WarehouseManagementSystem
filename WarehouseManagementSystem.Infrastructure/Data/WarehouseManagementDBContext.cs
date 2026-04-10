using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Domain.Model;

namespace WarehouseManagementSystem.Infrastructure.Data;

public class WarehouseManagementDBContext : IdentityDbContext<User>
{
    public WarehouseManagementDBContext(DbContextOptions options) 
        : base(options) 
    {}

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<Transfer> Transfers => Set<Transfer>();
    public DbSet<Order> Orders => Set<Order>();
    


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(product =>
        {
            product.HasKey(p => p.Id);

            product.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            product.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");

            product.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            product.HasOne(p => p.Location)
                   .WithMany(l => l.Products)
                   .HasForeignKey(p => p.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(order =>
        {
            order.HasKey(p => p.Id);

            order.Property(p => p.CustomerName)
                   .IsRequired()
                   .HasMaxLength(100);

            order.Property(p => p.Quantity)
                   .IsRequired();

            order.HasOne(o => o.Product)
                    .WithMany()
                    .HasForeignKey(o => o.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            order.Property(o => o.UserId)
                    .IsRequired();


        });

        modelBuilder.Entity<StockMovement> (stockMovement =>
         {
           stockMovement.HasOne(s => s.Product)
             .WithMany()
             .HasForeignKey(s => s.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
         });

        modelBuilder.Entity<Transfer>(transfer =>
        {
            transfer.HasOne(t => t.Product)
                    .WithMany()
                    .HasForeignKey(t => t.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            transfer.HasOne(t => t.FromLocation)
                    .WithMany()
                    .HasForeignKey(t => t.FromLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

            transfer.HasOne(t => t.ToLocation)
                    .WithMany()
                    .HasForeignKey(t => t.ToLocationId)
                    .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<Category>(category =>
        {
            category.HasKey(c => c.Id);

            category.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);
        });

        modelBuilder.Entity<RefreshToken>(
           refresh =>
           {
               refresh.HasKey(rt => rt.Id);
               refresh.HasIndex(rt => rt.JwtId).IsUnique();
               refresh.Property(rt => rt.JwtId).IsRequired().HasMaxLength(64);
               refresh.Property(rt => rt.UserId).IsRequired().HasMaxLength(450);

               refresh.HasOne(rt => rt.User).WithMany()
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

           });
    }
}
