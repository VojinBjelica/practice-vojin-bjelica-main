import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieComponent } from './movie.component';
import { MatCardModule } from '@angular/material/card'
import { MatButtonModule } from '@angular/material/button'
import { EnumPipeModule } from '../enum-pipe.module';
import { MatIconModule } from '@angular/material/icon';
import { MovieEditComponent } from './movie-edit/movie-edit.component'
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field'
import { ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule} from '@angular/material/select'
import { MatInputModule } from '@angular/material/input';



@NgModule({
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    EnumPipeModule,
    MatIconModule,
    MatSnackBarModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatInputModule
  ],
  declarations: [MovieComponent, MovieEditComponent]
})
export class MovieModule { }
