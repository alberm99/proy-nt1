using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Reservas_de_Canchas.Models
{
    public partial class EmpresaContext : DbContext
    {
        public EmpresaContext()
        {
        }

        public EmpresaContext(DbContextOptions<EmpresaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cancha> Cancha { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Turno> Turno { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-1I9HHSE\\SQLEXPRESS;Database=Empresa;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cancha>(entity =>
            {
                entity.HasKey(e => e.NroCancha);

                entity.Property(e => e.NombreCancha)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Contraseña)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Turno>(entity =>
            {
                entity.HasKey(e => e.NroTurno);

                entity.Property(e => e.EmailCliente)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.HasOne(d => d.EmailClienteNavigation)
                    .WithMany(p => p.Turno)
                    .HasForeignKey(d => d.EmailCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Turno_Cliente");

                entity.HasOne(d => d.NroCanchaNavigation)
                    .WithMany(p => p.Turno)
                    .HasForeignKey(d => d.NroCancha)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Turno_Cancha");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        private bool TurnoDisponible(Turno t)
        {
            bool TurnoDips = false;
            try
            {
                using (var db = new EmpresaContext())
                {
                    foreach (Turno item in db.Turno)
                    {
                        if (item.FechaHora == t.FechaHora)
                        {
                            if (item.NroCancha == t.NroCancha)
                            {
                                TurnoDips = true;
                            }
                        }
                    }

                    return TurnoDips;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
