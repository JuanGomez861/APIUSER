using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;


namespace Infrastructura
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Cuenta> Cuentas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            modelBuilder.Entity<Usuario>(entity =>
            {
                // Definir la clave primaria
                entity.HasKey(e => e.IdUsuario);

                // Definir la generación automática del Id
                entity.Property(e => e.IdUsuario)
                      .ValueGeneratedOnAdd();

                // Definir el nombre como obligatorio y con longitud máxima de 100
                entity.Property(e => e.Nombre)
                      .HasMaxLength(100)
                      .IsRequired();

                // Definir el apellido como obligatorio y con longitud máxima de 100
                entity.Property(e => e.Apellido)
                      .HasMaxLength(100)
                      .IsRequired();

                // Definir la cédula como obligatoria y única
                entity.Property(e => e.Cedula)
                      .IsRequired();  // No permite null

                // Crear un índice único para la cédula
                entity.HasIndex(e => e.Cedula)
                      .IsUnique();

                // Definir la dirección como obligatoria y con longitud máxima de 100
                entity.Property(e => e.Direccion)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.ToTable("Usuario");
            });

            modelBuilder.Entity<Cuenta>(entity =>
            {
                // Definir la clave primaria
                entity.HasKey(e => e.IdCuenta);

                // Definir la generación automática del Id
                entity.Property(e => e.IdCuenta)
                      .ValueGeneratedOnAdd();

                // Definir el correo como obligatorio, con longitud máxima de 150
                entity.Property(e => e.Correo)
                      .HasMaxLength(150)
                      .IsRequired();  // No permite null

                // Definir la contraseña como obligatoria, con longitud máxima de 20
                entity.Property(e => e.Contraseña)
                      .HasMaxLength(100)
                      .IsRequired();  // No permite null

                // Definir el rol como obligatorio
                entity.Property(e => e.Rol)
                      .IsRequired();  // No permite null

                entity.ToTable("Cuenta");

            });



        }

    }
}
