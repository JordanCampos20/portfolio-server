using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using finance_control_server.Models;
using Microsoft.AspNetCore.Identity;

namespace finance_control_server.Context
{
    public class ApplicationDbContext : IdentityDbContext<Usuario, Cargo, string, ClaimUsuario, UsuarioCargo, LoginUsuario, ClaimCargo, TokenUsuario>
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<ClaimCargo> ClaimCargos { get; set; }
        public DbSet<UsuarioCargo> UsuariosCargos { get; set; }
        public DbSet<TokenUsuario> TokenUsuarios { get; set; }
        public DbSet<LoginUsuario> LoginUsuarios { get; set; }
        public DbSet<ClaimUsuario> ClaimUsuarios { get; set; }

        public DbSet<Contato> Contatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");
            });
            
            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.ToTable("Cargo");
            });

            modelBuilder.Entity<Contato>(entity =>
            {
                entity.ToTable("Contatos");
            });

            modelBuilder.Entity<UsuarioCargo>(entity =>
            {
                entity.HasKey(r => new { r.UserId, r.RoleId });

                entity.ToTable("UsuarioCargo");
            });

            modelBuilder.Entity<ClaimCargo>(entity =>
            {
                entity.ToTable("ClaimCargo");
            });

            modelBuilder.Entity<TokenUsuario>(entity =>
            {
                entity.ToTable("TokenUsuario");
            });

            modelBuilder.Entity<LoginUsuario>(entity =>
            {
                entity.ToTable("LoginUsuario");
            });

            modelBuilder.Entity<ClaimUsuario>(entity =>
            {
                entity.ToTable("ClaimUsuario");
            });
            
            #region Contato

            // Configuração da tabela Contato
            modelBuilder.Entity<Contato>(entity =>
            {
                entity.ToTable("Contatos");

                entity.HasKey(e => e.ContatoId);

                entity.Property(e => e.Nome)
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                entity.Property(e => e.Email)
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar");

                entity.Property(e => e.Assunto)
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar");
                
                entity.Property(e => e.Data)
                        .IsRequired()
                        .HasColumnType("date");
            });

            #endregion
        }
    }
}
