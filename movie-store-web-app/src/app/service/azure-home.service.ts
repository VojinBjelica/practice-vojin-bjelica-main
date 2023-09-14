import { Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { Customer, CustomerClient } from '../api/api-reference';

@Injectable({
  providedIn: 'root'
})
export class AzureHomeService {
  displayName = '';
  private isCustomerLoggedIn = new Subject<boolean>();
  private customerName = new BehaviorSubject<string | undefined>(undefined);
  customerName$ = this.customerName.asObservable();
  isCustomerLoggedIn$ = this.isCustomerLoggedIn.asObservable();

  private customer: Customer | undefined;


  constructor(private readonly authService: MsalService,
    private readonly customerClient: CustomerClient) { }

  readonly login = () => {
    this.authService.loginPopup().subscribe(_ => {
      const accounts = this.authService.instance.getAllAccounts();
      if (accounts && accounts[0]) {
        this.customerName.next(accounts[0].name);
      }
      this.isCustomerLoggedIn.next(true);
      this.customerClient.create().subscribe(customer => this.customer = customer);
    });
  };

  readonly parseRoleToString = (role: number): string => (role === 1) ? 'REGULAR' : 'ADMIN';

  readonly logout = () => {
    this.authService.logoutPopup().subscribe(_ => {
      this.customerName.next(undefined);
      this.isCustomerLoggedIn.next(false);
      localStorage.setItem('STATE', 'false');
      localStorage.setItem('ROLE', '');
    });
  };

  get loggedInCustomer() {
    return this.customer;
  };

  isCustomerLoggedInStorage(): boolean {
    return localStorage.getItem('STATE') === 'true';
  }

  getLoggedInCustomerRole(): string {
    return localStorage.getItem('ROLE') || '';
  }

}