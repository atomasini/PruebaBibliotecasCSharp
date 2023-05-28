using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dominio.Entidades
{
    public partial class PW3TiendaContext : DbContext
    {
        public PW3TiendaContext()
        {
        }

        public PW3TiendaContext(DbContextOptions<PW3TiendaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HistorialProducto> HistorialProductos { get; set; } = null!;
        public virtual DbSet<Historico> Historicos { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-8SMFKT9\\SQLEXPRESS;Database=PW3Tienda;User Id=sa;Password=1234;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HistorialProducto>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("historial_producto");

                entity.Property(e => e.IdCoproducto).HasColumnName("id_coproducto");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.HasOne(d => d.IdCoproductoNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdCoproducto)
                    .HasConstraintName("FK__HISTORIAL__id_co__38996AB5");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__HISTORIAL__id_pr__37A5467C");
            });

            modelBuilder.Entity<Historico>(entity =>
            {
                entity.ToTable("Historico");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCoproducto).HasColumnName("idCoproducto");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.Property(e => e.Puntaje).HasColumnName("puntaje");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("producto");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Imagen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("imagen");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("precio");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
