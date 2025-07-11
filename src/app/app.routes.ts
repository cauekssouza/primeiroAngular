import { Routes } from '@angular/router';
import { MovieListComponent } from './components/movie-list/movie-list.component';
import { MovieFormComponent } from './components/movie-form/movie-form.component';

export const routes: Routes = [
{ path: '', redirectTo: '/filmes', pathMatch: 'full' },
  { path: 'filmes', component: MovieListComponent },
  { path: 'adicionar', component: MovieFormComponent },
  { path: 'editar/:id', component: MovieFormComponent },
  { path: '**', redirectTo: '/filmes' }
];

