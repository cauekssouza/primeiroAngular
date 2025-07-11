export interface Movie {
  id?: number;
  titulo: string;
  ano: number;
  genero: string;
  diretor: string;
  sinopse: string;
  posterUrl?: string;
  avaliacao: number; 
  dataLancamento: Date;
  duracao: number;
}

export interface MovieResponse {
  duracao: any;
  avaliacao: any;
  posterUrl: any;
  sinopse: any;
  diretor: any;
  genero: any;
  ano: any;
  titulo: any;
  dataLancamento: string | number | Date;
  movies: Movie[];
  totalCount: number;
  currentPage: number;
  totalPages: number;
}