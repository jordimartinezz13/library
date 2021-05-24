using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace pt1_mvc.Models
{
    public partial class bankContext : DbContext
    {
        public bankContext()
        {
        }

        public bankContext(DbContextOptions<bankContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Compte> Comptes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");

                entity.Property(e => e.Cognoms)
                    .IsRequired()
                    .HasColumnName("cognoms");

                entity.Property(e => e.Dni)
                    .IsRequired()
                    .HasColumnName("dni");

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasColumnName("nom");
            });

            modelBuilder.Entity<Compte>(entity =>
            {
                entity.ToTable("compte");

                entity.HasIndex(e => e.ClientId, "IX_FK_clientcompte");

                entity.Property(e => e.ClientId).HasColumnName("client_Id");

                entity.Property(e => e.Codi)
                    .IsRequired()
                    .HasColumnName("codi");

                entity.Property(e => e.Saldo).HasColumnName("saldo");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Comptes)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clientcompte");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
