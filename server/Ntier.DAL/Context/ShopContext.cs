using System;


using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ntier.DAL.Entities;

namespace Ntier.DAL.Context
{
    public partial class ShopContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ShopContext()
        {
        }

        public ShopContext(DbContextOptions<ShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;
        public virtual DbSet<ProductSize> ProductSizes { get; set; } = null!;
        public virtual DbSet<ProductSizeDetail> ProductSizeDetails { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IdentityUser
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims")
            .HasKey(x => x.Id);

            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles")
            .HasKey(x => new { x.RoleId, x.UserId });

            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .HasColumnName("Name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Address");

                entity.Property(e => e.CreateAt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CreateAt");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ORDERS__USER_ID__412EB0B6");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(x => new { x.ProductId, x.OrderId });

                entity.ToTable("Order_detail");

                entity.Property(e => e.OrderId).HasColumnName("OrderId");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductId");

                entity.Property(e => e.Quantity).HasColumnName("Quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(d => d.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__ORDER_DET__ORDER__32AB8735");

                entity.HasOne(d => d.Product)
                    .WithMany(d => d.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ORDER_DET__PRODU__3587F3E0");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("CreateAt");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("Description");

                entity.Property(e => e.MinPrice).HasColumnName("MinPrice");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("Name");

                entity.Property(e => e.Sale).HasColumnName("Sale");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__PRODUCT__CATEGOR__2CF2ADDF");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Url)
                    .HasName("PK__PRODUCT___C5B100083B40F9CD");

                entity.ToTable("ProductImage");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Url");

                entity.Property(e => e.Index).HasColumnName("Index");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__PRODUCT_I__PRODU__395884C4");
            });

            modelBuilder.Entity<ProductSize>(entity =>
            {
                entity.ToTable("ProductSize");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Size).HasColumnName("Size");
            });

            modelBuilder.Entity<ProductSizeDetail>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.SizeId })
                    .HasName("PK__PRODUCT___9420F6C487A5BC55");

                entity.ToTable("ProductSizeDetail");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductId");

                entity.Property(e => e.SizeId).HasColumnName("SizeId");

                entity.Property(e => e.Price).HasColumnName("Price");

                entity.Property(e => e.Quantity).HasColumnName("Quantity");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSizeDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PRODUCT_S__PRODU__3D2915A8");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.ProductSizeDetails)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PRODUCT_S__SIZE___3E1D39E1");
            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
