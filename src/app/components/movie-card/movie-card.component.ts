import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Movie } from '../../models/movie';

@Component({
  selector: 'app-movie-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './movie-card.component.html',
  styleUrls: ['./movie-card.component.css']
})
export class MovieCardComponent {
  @Input() movie!: Movie;
  @Output() editMovie = new EventEmitter<Movie>();
  @Output() deleteMovie = new EventEmitter<number>();

  getStars(rating: number): string[] {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      stars.push(i <= rating ? '⭐' : '☆');
    }
    return stars;
  }

  onEdit() {
    this.editMovie.emit(this.movie);
  }

  onDelete() {
    if (confirm(`Tem certeza que deseja excluir "${this.movie.titulo}"?`)) {
      this.deleteMovie.emit(this.movie.id!);
    }
  }
}