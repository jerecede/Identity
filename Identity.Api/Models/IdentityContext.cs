using Identity.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Api.Services.Models
{
    public class IdentityContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }

        public IdentityContext()
        {
        }

        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(AppConfig.GetConnectionString());

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id).HasName("users_pkey");
                entity.Property(u => u.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50).HasColumnName("first_name");
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50).HasColumnName("last_name");
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100).HasColumnName("email");
                entity.Property(u => u.Password).IsRequired().HasMaxLength(100).HasColumnName("password");

                //entity.HasMany(u => u.Requests)
                //      .WithOne(r => r.User)
                //      .HasForeignKey(r => r.UserId);
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("requests");
                entity.HasKey(r => r.Id).HasName("requests_pkey");
                entity.Property(r => r.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(r => r.Text).IsRequired().HasMaxLength(500).HasColumnName("text");
                entity.Property(r => r.CreationTime).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("creation_time");
                entity.Property(r => r.UserId).IsRequired().HasColumnName("user_id");

                entity.HasOne(r => r.User).WithMany(u => u.Requests)
                      .HasForeignKey(r => r.UserId)
                      .HasConstraintName("user_fkey")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(r => r.Id).HasName("roles_pkey");
                entity.Property(r => r.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50).HasColumnName("name");
            });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(ur => ur.ToTable("user_roles"));

        }
    }
}
