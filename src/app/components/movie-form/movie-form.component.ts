import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { MovieService, Movie } from "../../services/movie.service";

@Component({
  selector: "app-movie-form",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="container">
      <div class="form-container">
        <h1>{{ isEditing ? 'Editar' : 'Adicionar' }} Filme</h1>

        <form [formGroup]="movieForm" (ngSubmit)="onSubmit()">
          <div class="form-group">
            <label class="form-label">Título</label>
            <input type="text" formControlName="titulo" class="form-input" />
          </div>

          <div class="form-group">
            <label class="form-label">Diretor</label>
            <input type="text" formControlName="diretor" class="form-input" />
          </div>

          <div class="form-row">
            <div class="form-group">
              <label class="form-label">Ano</label>
              <input type="number" formControlName="ano" class="form-input" min="1900" max="2030" />
            </div>
            <div class="form-group">
              <label class="form-label">Duração (min)</label>
              <input type="number" formControlName="duracao" class="form-input" min="1" />
            </div>
          </div>

          
          <div class="form-group">
            <label class="form-label">Gênero</label>
            <select formControlName="genero" class="form-input">
              <option value="" disabled selected>Selecione um gênero</option>
              <option *ngFor="let genero of generos" [value]="genero">{{ genero }}</option>
            </select>
          </div>
          <div class="form-group">
            <label class="form-label">Avaliação (1-10)</label>
            <input type="number" min="1" max="10" formControlName="avaliacao" class="form-input">
          </div>



          <div class="form-group">
            <label class="form-label">Data de Lançamento</label>
            <input type="date" formControlName="dataLancamento" class="form-input" />
          </div>

          <div *ngIf="movieForm.get('posterUrl')?.value" class="poster-preview">
            <label class="form-label">Pré-visualização do Poster:</label>
            <img 
              [src]="movieForm.get('posterUrl')?.value" 
              alt="Poster do filme" 
              class="poster-image"
              (error)="posterErro = true"
              *ngIf="!posterErro"
            />
            <p *ngIf="posterErro" class="field-error">URL da imagem inválida</p>
          </div>

          <div class="form-group">
            <label class="form-label">URL do Poster (opcional)</label>
            <input type="url" formControlName="posterUrl" class="form-input" />
          </div>

          <div class="form-group">
            <label class="form-label">Sinopse</label>
            <textarea formControlName="sinopse" class="form-textarea"></textarea>
          </div>

          <div class="form-actions">
            <button type="submit" [disabled]="movieForm.invalid || loading" class="btn btn-success">
              <span *ngIf="loading" class="spinner"></span>
              {{ loading ? 'Salvando...' : (isEditing ? 'Atualizar' : 'Adicionar') }}
            </button>
            <button type="button" (click)="goBack()" class="btn btn-secondary">Cancelar</button>
          </div>
        </form>

        <div *ngIf="error" class="error">{{ error }}</div>
      </div>
    </div>
  `,
})
export class MovieFormComponent implements OnInit {
  movieForm: FormGroup;
  isEditing = false;
  movieId: number | null = null;
  loading = false;
  error: string | null = null;
  posterErro = false;

  generos: string[] = [
    'Ação',
    'Aventura',
    'Comédia',
    'Drama',
    'Ficção Científica',
    'Terror',
    'Romance',
    'Suspense',
    'Animação',
    'Documentário',
  ];

  constructor(
    private readonly fb: FormBuilder,
    private readonly movieService: MovieService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {
    this.movieForm = this.fb.group({
      titulo: ['', Validators.required],
      diretor: ['', Validators.required],
      ano: ['', [Validators.required, Validators.min(1900), Validators.max(2030)]],
      genero: ['', Validators.required],   // <-- aqui
      sinopse: ['', Validators.required],
      posterUrl: [''],
      avaliacao: ['', [Validators.required, Validators.min(1), Validators.max(10)]],
      dataLancamento: ['', Validators.required],
      duracao: ['', [Validators.required, Validators.min(1)]],
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditing = true;
      this.movieId = +id;
      this.loadMovie();
    }

    this.movieForm.get('posterUrl')?.valueChanges.subscribe(() => {
      this.posterErro = false;
    });
  }

  loadMovie(): void {
    if (this.movieId) {
      this.movieService.getMovie(this.movieId).subscribe({
        next: (movie: Movie) => {
          const dataFormatada = new Date(movie.dataLancamento).toISOString().split('T')[0];
          this.movieForm.patchValue({
            titulo: movie.titulo,
            ano: movie.ano,
            genero: movie.genero,
            diretor: movie.diretor,
            sinopse: movie.sinopse,
            posterUrl: movie.posterUrl,
            avaliacao: movie.avaliacao,
            dataLancamento: dataFormatada,
            duracao: movie.duracao,
          });
        },
        error: () => {
          this.error = 'Erro ao carregar filme';
        },
      });
    }
  }

  onSubmit(): void {
    if (this.movieForm.valid) {
      this.loading = true;
      this.error = null;

      const movieData = {
        ...this.movieForm.value,
        dataLancamento: new Date(this.movieForm.value.dataLancamento),
      };

      if (this.isEditing && this.movieId) {
        this.movieService.updateMovie(this.movieId, movieData).subscribe({
          next: () => {
            this.router.navigate(['/filmes']);
          },
          error: () => {
            this.error = 'Erro ao atualizar filme';
            this.loading = false;
          },
        });
      } else {
        this.movieService.createMovie(movieData).subscribe({
          next: () => {
            this.router.navigate(['/filmes']);
          },
          error: () => {
            this.error = 'Erro ao criar filme';
            this.loading = false;
          },
        });
      }
    }
  }

  goBack(): void {
    this.router.navigate(['/filmes']);
  }
}
