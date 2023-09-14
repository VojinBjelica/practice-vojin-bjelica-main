import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, CanDeactivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { AzureHomeService } from './azure-home.service';
import { CustomerRole } from '../api/api-reference';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate  {
  isUserLoggedIn : boolean = false;
  constructor(private azureHomeService: AzureHomeService, private router: Router) {}

   canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.checkUserLogin(next);
  }

  checkUserLogin(route: ActivatedRouteSnapshot): boolean {
    const customer = this.azureHomeService.loggedInCustomer;
    if(!customer){
      return false;
    }

    if(route.data['role'] && route.data['role'].indexOf(customer.role) === -1){
      return false;
    }

    return true;
  }
}
