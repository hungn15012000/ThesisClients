using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MiddleWare.Data
{
    public partial class MiddleWare_dbContext : IdentityDbContext<ApiUser>
    {
        public MiddleWare_dbContext()
        {
        }

        public MiddleWare_dbContext(DbContextOptions<MiddleWare_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;


        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=tcp:middlewarethesisdb.database.windows.net,1433;Initial Catalog=MiddleWare_db;Persist Security Info=False;User ID=thesisadmin;Password=Thesis2022;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CompanyName).HasMaxLength(50);

                entity.Property(e => e.Firstname).HasMaxLength(50);

                entity.Property(e => e.Lastname).HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.ProductSerialNumber, "UQ__Products__95D2CF590D601497")
                    .IsUnique();

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.ProductInvoice).HasMaxLength(50);

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.ProductSerialNumber).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_ToTable");
               
            });

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Id = "f0c49c86-448e-48e1-a2f4-52290a13b172"
                },
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    Id = "f3a39e15-3bd5-466b-b406-4f11ceb983ca"
                }
                );
            var hasher = new PasswordHasher<ApiUser>();

            modelBuilder.Entity<ApiUser>().HasData(
                new ApiUser
                {
                    Id = "d4a5ab24-9aae-4816-9629-5c45856f8945",
                    Email = "admin@thesis2022.com",
                    NormalizedEmail = "ADMIN@THESIS2022.COM",
                    UserName = "admin@thesis2022.com",
                    NormalizedUserName = "ADMIN@THESIS2022.COM",
                    Firstname = "System",
                    Lastname = "Admin",
                    PasswordHash = hasher.HashPassword(null, "password")

                },
                new ApiUser
                {
                    Id = "32614026-3bb6-49f1-9420-86f185b79e6a",
                    Email = "user@thesis2022.com",
                    NormalizedEmail = "USER@THESIS2022.COM",
                    UserName = "user@thesis2022.com",
                    NormalizedUserName = "USER@THESIS2022.COM",
                    Firstname = "System",
                    Lastname = "User",
                    PasswordHash = hasher.HashPassword(null, "password")
                }
                );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                
                new IdentityUserRole<string>
                {
                    RoleId = "f0c49c86-448e-48e1-a2f4-52290a13b172",
                    UserId = "32614026-3bb6-49f1-9420-86f185b79e6a"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "f3a39e15-3bd5-466b-b406-4f11ceb983ca",
                    UserId = "d4a5ab24-9aae-4816-9629-5c45856f8945"
                }
            );

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
