import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Movie } from './movie.model';


@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent {
  public movies: Movie[];

  constructor(http: HttpClient, @Inject('API_URL') apiUrl: string) {
    http.get<Movie[]>(apiUrl + 'Movies').subscribe(result => {
      this.movies = result;
    }, error => console.error(error));
  }
}

