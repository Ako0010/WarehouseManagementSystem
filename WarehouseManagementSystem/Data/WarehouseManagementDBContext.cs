using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WarehouseManagementSystem.Model;

namespace WarehouseManagementSystem.Data;

public class WarehouseManagementDBContext : IdentityDbContext<User>
{
    public WarehouseManagementDBContext(DbContextOptions options) 
        : base(options) 
    {}

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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
        });

        modelBuilder.Entity<Sale>(sale =>
        {
            sale.HasKey(s => s.Id);

            sale.Property(s => s.TotalPrice)
                .HasColumnType("decimal(18,2)");

            sale.HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
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
