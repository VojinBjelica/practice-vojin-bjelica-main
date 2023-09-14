import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerComponent } from './customer/customer.component';
import { MovieComponent } from './movie/movie.component';
import { MovieEditComponent } from './movie/movie-edit/movie-edit.component';
import { HomeComponent } from './home/home.component';
import { RoleGuard } from './service/role-guard';
import { CustomerRole } from './api/api-reference';

const routes: Routes = [
  { path: 'customers', component: CustomerComponent, canActivate: [RoleGuard], data: { role: [CustomerRole.Admin] } },
  { path: 'movies', component: MovieComponent, canActivate: [RoleGuard], data: { role: [CustomerRole.Regular, CustomerRole.Admin] } },
  { path: 'movies/create', component: MovieEditComponent, canActivate: [RoleGuard], data: { role: [CustomerRole.Admin] } },
  { path: 'movies/edit/:id', component: MovieEditComponent, canActivate: [RoleGuard], data: { role: [CustomerRole.Admin] } },
  { path: '', component: HomeComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  declarations: []
})
export class AppRoutingModule { }
