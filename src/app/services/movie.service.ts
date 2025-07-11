import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'

export interface Movie {
  id?: number
  titulo: string
  diretor: string
  ano: number
  genero: string
  sinopse: string
  posterUrl?: string
  avaliacao: number
  dataLancamento: string
  duracao: number
}

export interface MovieResponse {
  movies: Movie[]
}

@Injectable({ providedIn: 'root' })
export class MovieService {
  private readonly API_URL = 'http://localhost:5042/api/movies'

  constructor(private readonly http: HttpClient) {}

  getMovies(): Observable<MovieResponse> {
    return this.http.get<MovieResponse>(this.API_URL)
  }

  getMovie(id: number): Observable<Movie> {
    return this.http.get<Movie>(`${this.API_URL}/${id}`)
  }

  createMovie(movie: Movie): Observable<Movie> {
    return this.http.post<Movie>(this.API_URL, movie)
  }

  updateMovie(id: number, movie: Movie): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}`, movie)
  }

  deleteMovie(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`)
  }
}
