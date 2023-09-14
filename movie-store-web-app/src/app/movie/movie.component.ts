import { Component, OnInit } from '@angular/core';
import { Customer, CustomerClient, GetAllMoviesResponse, LicensingType, Movie, MovieClient } from '../api/api-reference';
import { Router } from '@angular/router';
import { Observable, lastValueFrom, of } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AzureHomeService } from '../service/azure-home.service';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css']
})
export class MovieComponent implements OnInit {

  movies: GetAllMoviesResponse[] = [];
  LicensingType = LicensingType;
  customer: Customer | undefined;
  constructor(private readonly movieClient: MovieClient,
    private readonly router: Router,
    private readonly snackBar: MatSnackBar,
    private readonly azureHomeService: AzureHomeService,
    private readonly customerClient: CustomerClient) {
  }

  ngOnInit() {
    this.getMovies();
    this.customer = this.azureHomeService.loggedInCustomer
  }

  private readonly getMovies = () => {
    this.movieClient.getAll().subscribe(response => this.movies = response);
  };

  navigateToCreateMovie() {
    this.router.navigate(["/movies/create"]);
  }

  navigateToEditMovie(id?: string) {

    this.router.navigate(['movies/edit', id]);
  }

  deleteMovie(movie: GetAllMoviesResponse) {
    if (movie.id == null) {
      return;
    }

    this.movieClient.delete(movie.id).subscribe({
      next: async _ => {
        this.movies = this.movies.filter(x => x.id !== movie.id)
      },
      error: error => {
        this.snackBar.open('Error deleting movie:' + error, 'Close', { duration: 3000 });
        this.getMovies();
      }
    })
  }

  purchaseMovie(movie: GetAllMoviesResponse) {
    const loggedInCustomer = this.azureHomeService.loggedInCustomer;
    if (movie.id == null || loggedInCustomer?.id == null) {
      return;
    }

    this.customerClient.purchaseMovie(loggedInCustomer.id, movie.id).subscribe({
      next: async _ => {
        this.snackBar.open('Movie successfully purchased!', 'Close', { duration: 3000 });
      },
      error: error => {
        this.snackBar.open('Error purchasing movie:' + error, 'Close', { duration: 3000 });
      }
    })
  }

  get isAdmin() {
    return this.customer && this.customer.role === 2;
  }
}
