using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogoFilmes.API.Data;
using CatalogoFilmes.API.Models;
using CatalogoFilmes.API.DTOs;

namespace CatalogoFilmes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ApplicationDbContext context, ILogger<MoviesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        [HttpGet]
        public async Task<ActionResult<MovieResponseDto>> GetMovies(
            int page = 1, 
            int pageSize = 10, 
            string? search = null)
        {
            try
            {
                var query = _context.Movies.AsQueryable();

                // Filtro de busca
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(m => 
                        m.Titulo.Contains(search) || 
                        m.Diretor.Contains(search) ||
                        m.Genero.Contains(search) ||
                        m.Ano.ToString().Contains(search));
                }

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var movies = await query
                    .OrderByDescending(m => m.DataCriacao)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var response = new MovieResponseDto
                {
                    Movies = movies,
                    TotalCount = totalCount,
                    CurrentPage = page,
                    TotalPages = totalPages
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar filmes");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);

                if (movie == null)
                {
                    return NotFound($"Filme com ID {id} não encontrado");
                }

                return Ok(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar filme com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        
       [HttpPost]
public async Task<ActionResult<Movie>> CreateMovie([FromBody] MovieDto movieDto)
{
    try
    {
        // Mapeando os dados do DTO para a entidade Movie
        var movie = new Movie
        {
            Titulo = movieDto.Titulo,
            Ano = movieDto.Ano,
            Genero = movieDto.Genero,
            Diretor = movieDto.Diretor,
            Sinopse = movieDto.Sinopse,
            PosterUrl = movieDto.PosterUrl,
            Avaliacao = movieDto.Avaliacao,
            DataLancamento = movieDto.DataLancamento,
            Duracao = movieDto.Duracao,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        
        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro ao criar filme: {Message}", ex.Message);
        return StatusCode(500, "Erro ao criar filme");
    }
}


        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieDto movieDto)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);
                
                if (movie == null)
                {
                    return NotFound($"Filme com ID {id} não encontrado");
                }

                movie.Titulo = movieDto.Titulo;
                movie.Ano = movieDto.Ano;
                movie.Genero = movieDto.Genero;
                movie.Diretor = movieDto.Diretor;
                movie.Sinopse = movieDto.Sinopse;
                movie.PosterUrl = movieDto.PosterUrl;
                movie.Avaliacao = movieDto.Avaliacao;
                movie.DataLancamento = movieDto.DataLancamento;
                movie.Duracao = movieDto.Duracao;
                movie.DataAtualizacao = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar filme com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie == null)
                {
                    return NotFound($"Filme com ID {id} não encontrado");
                }

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar filme com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Movie>>> SearchMovies(string search)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    return BadRequest("Termo de busca é obrigatório");
                }

                var movies = await _context.Movies
                    .Where(m => 
                        m.Titulo.Contains(search) || 
                        m.Diretor.Contains(search) ||
                        m.Genero.Contains(search) ||
                        m.Ano.ToString().Contains(search))
                    .OrderByDescending(m => m.DataCriacao)
                    .ToListAsync();

                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pesquisar filmes com termo: {Search}", search);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}