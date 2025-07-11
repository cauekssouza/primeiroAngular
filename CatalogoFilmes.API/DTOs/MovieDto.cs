using CatalogoFilmes.API.Models;

namespace CatalogoFilmes.API.DTOs
{
    public class MovieDto
    {
        public string Titulo { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Genero { get; set; } = string.Empty;
        public string Diretor { get; set; } = string.Empty;
        public string Sinopse { get; set; } = string.Empty;
        public string? PosterUrl { get; set; }
        public int Avaliacao { get; set; }
        public DateTime DataLancamento { get; set; }
        public int Duracao { get; set; }
    }

    public class MovieResponseDto
    {
        public List<Movie> Movies { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}