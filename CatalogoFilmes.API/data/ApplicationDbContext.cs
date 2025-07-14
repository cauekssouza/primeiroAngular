using Microsoft.EntityFrameworkCore;
using CatalogoFilmes.API.Models;

namespace CatalogoFilmes.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Genero).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Diretor).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Sinopse).HasMaxLength(1000);
                entity.HasIndex(e => e.Titulo);
                entity.HasIndex(e => e.Ano);
            });

            
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 1,
                    Titulo = "O Poderoso Chefão",
                    Ano = 1972,
                    Genero = "Drama",
                    Diretor = "Francis Ford Coppola",
                    Sinopse = "A saga da família Corleone e sua ascensão no mundo do crime organizado em Nova York.",
                    Avaliacao = 5,
                    DataLancamento = new DateTime(1972, 3, 24),
                    Duracao = 175,
                    PosterUrl = "https://image.tmdb.org/t/p/w500/3bhkrj58Vtu7enYsRolD1fZdja1.jpg",
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                },
                new Movie
                {
                    Id = 2,
                    Titulo = "Pulp Fiction",
                    Ano = 1994,
                    Genero = "Crime",
                    Diretor = "Quentin Tarantino",
                    Sinopse = "Histórias entrelaçadas de crime e redenção em Los Angeles, contadas de forma não linear.",
                    Avaliacao = 5,
                    DataLancamento = new DateTime(1994, 10, 14),
                    Duracao = 154,
                    PosterUrl = "https://image.tmdb.org/t/p/w500/d5iIlFn5s0ImszYzBPb8JPIfbXD.jpg",
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                },
                new Movie
                {
                    Id = 3,
                    Titulo = "Cidade de Deus",
                    Ano = 2002,
                    Genero = "Drama",
                    Diretor = "Fernando Meirelles",
                    Sinopse = "A história da evolução do crime organizado na Cidade de Deus, uma favela do Rio de Janeiro.",
                    Avaliacao = 5,
                    DataLancamento = new DateTime(2002, 8, 30),
                    Duracao = 130,
                    PosterUrl = "https://image.tmdb.org/t/p/w500/k7eYdcdYEZhKkANBWyoqBjbO2cz.jpg",
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                },
                new Movie
                {
                    Id = 4,
                    Titulo = "Interestelar",
                    Ano = 2014,
                    Genero = "Ficção Científica",
                    Diretor = "Christopher Nolan",
                    Sinopse = "Em um futuro próximo, a Terra está morrendo. Um grupo de exploradores deve viajar através de um buraco de minhoca para salvar a humanidade.",
                    Avaliacao = 4,
                    DataLancamento = new DateTime(2014, 11, 7),
                    Duracao = 169,
                    PosterUrl = "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg",
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                }
            );
        }
    }
}