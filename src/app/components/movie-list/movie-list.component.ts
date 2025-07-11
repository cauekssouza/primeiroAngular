import { Component, type OnInit } from "@angular/core"
import { CommonModule } from "@angular/common"
import { RouterModule } from "@angular/router"
import { MovieService, Movie, MovieResponse } from "../../services/movie.service"

@Component({
  selector: "app-movie-list",
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="container">
      <div class="header">
        <h1>Catálogo de Filmes</h1>
        <a routerLink="/adicionar" class="btn btn-primary">
          Adicionar Filme
        </a>
      </div>

      <!-- Loading -->
      <div *ngIf="loading" class="loading">
        <div class="spinner"></div>
        <p>Carregando filmes...</p>
      </div>

      <!-- Error -->
      <div *ngIf="error" class="error">
        <p>{{ error }}</p>
        <button (click)="loadMovies()" class="btn btn-danger btn-small">
          Tentar Novamente
        </button>
      </div>

      <!-- Movies Grid -->
      <div *ngIf="!loading && !error && movies.length > 0" class="movies-grid">
        <div *ngFor="let movie of movies" class="movie-card">
          <div class="movie-card-content">
            <h3 class="movie-title">{{ movie.titulo }}</h3>
            <p class="movie-info"><strong>Diretor:</strong> {{ movie.diretor }}</p>
            <p class="movie-info"><strong>Ano:</strong> {{ movie.ano }}</p>
            <p class="movie-info"><strong>Gênero:</strong> {{ movie.genero }}</p>
            <p class="movie-info"><strong>Avaliação:</strong> {{ movie.avaliacao }}/10</p>
            <p class="movie-synopsis">{{ movie.sinopse }}</p>

            <div class="movie-actions">
              <a [routerLink]="['/editar', movie.id]" class="btn btn-warning btn-small">
                Editar
              </a>
              <button (click)="deleteMovie(movie.id!)" class="btn btn-danger btn-small">
                Excluir
              </button>
            </div>
          </div>
        </div>
      </div>
      </div>
  `,
})
export class MovieListComponent implements OnInit {
  movies: Movie[] = []
  loading = false
  error: string | null = null

  constructor(private readonly movieService: MovieService) {}

  ngOnInit(): void {
    this.loadMovies()
  }

  loadMovies(): void {
    this.loading = true
    this.error = null

    this.movieService.getMovies().subscribe({
      next: (response: MovieResponse) => {
        this.movies = response.movies || []
        this.loading = false
      },
      error: (error: any) => {
        this.error = error.message
        this.loading = false
      },
    })
  }

  deleteMovie(movieId: number): void {
    if (confirm("Tem certeza que deseja excluir este filme?")) {
      this.movieService.deleteMovie(movieId).subscribe({
        next: () => {
          this.loadMovies()
        },
        error: (error: any) => {
          this.error = "Erro ao deletar filme"
        },
      })
    }
  }
}
