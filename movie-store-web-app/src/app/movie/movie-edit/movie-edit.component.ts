import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CreateMovieCommand, LicensingType, Movie, MovieClient, UpdateMovieCommand } from '../../api/api-reference'
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-movie-edit',
  templateUrl: './movie-edit.component.html',
  styleUrls: ['./movie-edit.component.css']
})
export class MovieEditComponent implements OnInit {
  formGroup = new FormGroup({
    name: new FormControl<string>('', { nonNullable: true, validators: Validators.required }),
    licensingType: new FormControl<LicensingType>(LicensingType.TwoDay, { nonNullable: true, validators: Validators.required })
  });
  movieId: string | undefined;

  constructor(private readonly movieClient: MovieClient,
    private readonly snackBar: MatSnackBar,
    private readonly route: ActivatedRoute) { }

  ngOnInit(): void {
    this.movieId = this.route.snapshot.paramMap.get('id') ?? undefined;
    if(this.movieId){
      this.movieClient.getById(this.movieId).subscribe(movie => 
        this.patchForm(movie))
    }
  }

  readonly createMovie = () => {
    const createMovieCommand: CreateMovieCommand = new CreateMovieCommand({
      name: this.formGroup.controls.name.value,
      licensingType: this.formGroup.controls.licensingType.value
    });

    this.movieClient.create(createMovieCommand).subscribe({
      next: res => {
        this.snackBar.open('Successfully created', 'Close', { duration: 3000 });
      },
      error: error => {
        this.snackBar.open('Error retrieving movies:' + error, 'Close', { duration: 3000 });
      }
    });
  }

  readonly editMovie = () => {
    const updateMovieCommand: UpdateMovieCommand = new UpdateMovieCommand({
      movieId: this.movieId,
      name: this.formGroup.controls.name.value,
      licensingType: this.formGroup.controls.licensingType.value
    });

    this.movieClient.update(updateMovieCommand).subscribe({
      next: res => {
        this.snackBar.open('Successfully updated', 'Close', { duration: 3000 });
      },
      error: error => {
        this.snackBar.open('Error retrieving movies:' + error, 'Close', { duration: 3000 });
      }
    });
  }

  public saveMovie = () => {
    this.movieId ? this.editMovie() : this.createMovie()
  }
  
  private readonly patchForm = (movie: Movie) => {
    this.formGroup.patchValue({
      licensingType: movie.licensingType,
      name: movie.name
    });
  }
}
