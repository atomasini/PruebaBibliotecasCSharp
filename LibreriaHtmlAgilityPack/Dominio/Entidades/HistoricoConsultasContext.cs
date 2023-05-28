using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Dominio.Entidades
{
    public partial class HistoricoConsultasContext : DbContext
    {
        public HistoricoConsultasContext()
        {
        }

        public HistoricoConsultasContext(DbContextOptions<HistoricoConsultasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Historico> Historicos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning        To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=CHONII;Database=HistoricoConsultas;User Id=sa;Password=1234;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Historico>(entity =>
            {
                entity.ToTable("Historico");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCoproducto).HasColumnName("idCoproducto");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.Property(e => e.Puntaje).HasColumnName("puntaje");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
