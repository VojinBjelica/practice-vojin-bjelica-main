import { Component, OnInit } from '@angular/core';
import { AzureHomeService } from '../service/azure-home.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent implements OnInit {
  isCustomerLoggedIn: boolean = false;
  constructor(private azureHomeService: AzureHomeService) { }


  ngOnInit(): void {
    this.azureHomeService.isCustomerLoggedIn$.subscribe(
      x => this.isCustomerLoggedIn = x
    )
  }

  login() {
    this.azureHomeService.login();
  }

  logout() {
    this.azureHomeService.logout();
  }
}
