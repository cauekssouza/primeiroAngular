using System.ComponentModel.DataAnnotations;

namespace CatalogoFilmes.API.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;
        
        [Range(1900, 2030)]
        public int Ano { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Genero { get; set; } = string.Empty;
        
        [Required]
        [StringLength(150)]
        public string Diretor { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Sinopse { get; set; } = string.Empty;
        
        public string? PosterUrl { get; set; }

        [Range(1, 10, ErrorMessage = "A avaliação deve estar entre 1 e 10.")]
        public int Avaliacao { get; set; }


        
        public DateTime DataLancamento { get; set; }
        
        [Range(1, 500)]
        public int Duracao { get; set; } 
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    }
}