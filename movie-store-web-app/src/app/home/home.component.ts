import { Component, OnInit } from '@angular/core';
import { AzureHomeService } from '../service/azure-home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  isUserLoggedIn: boolean = false;
  name: string | undefined;

  constructor(private azureHomeService: AzureHomeService) { }

  ngOnInit() {
    this.azureHomeService.isCustomerLoggedIn$.subscribe(
      x => this.isUserLoggedIn = x
    )

    this.azureHomeService.customerName$.subscribe(x => this.name = x);
  }
}
