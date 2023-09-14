import { Component, OnInit } from '@angular/core';
import { Customer, CustomerClient } from '../api/api-reference';
import { Status, CustomerRole } from '../api/api-reference';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, catchError, of } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { CustomerCreateDialogComponent } from './customer-create-dialog/customer-create-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {

  displayedColumns: string[] = ['Email', 'Status', 'Status expiration', 'Role', 'Menu'];
  dataSource: Customer[] = []
  CustomerRole = CustomerRole;
  Status = Status;
  customers$: Observable<Customer[]> = new Observable;

  constructor(private readonly customerClient: CustomerClient,
    private readonly snackBar: MatSnackBar,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.customers$ = this.customerClient.getAll();
    this.customers$.subscribe({
      next: customers => {
        this.dataSource = customers;
      },
      error: error => {
        this.snackBar.open('Error retrieving movies:' + error, 'Close', { duration: 3000 });
      }
    }
    );
  }

  openDialog(customerId: string): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      data: { customerId: customerId },
    })

    dialogRef.afterClosed().subscribe(async (_) => {
      try {
        await this.updateTable();
      } catch (error) {
        this.snackBar.open('Error retrieving movies:' + error, 'Close', { duration: 3000 });
      }
    });
  }

  updateTable(): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.customerClient.getAll().subscribe({
        next: customers => {
          this.dataSource = customers;
          this.customers$ = of(customers);
          resolve();
        },
        error: error => {
          reject(error);
        }
      });
    });
  }

  public deleteCustomer(customerId: string) {
    if (customerId == null) {
      return;
    }

    this.customerClient.delete(customerId).subscribe(async (_) => {
      try {
        await this.updateTable();
      } catch (error) {
        this.snackBar.open('Error retrieving movies:' + error, 'Close', { duration: 3000 });
      }
    })
  }

  public promoteCustomer(customerId: string) {
    if (customerId == null) {
      return;
    }

    this.customerClient.promote(customerId)
    .pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          this.snackBar.open('User not eligible for promotion.', 'Close', { duration: 3000 });
        }
        throw error;
      })
    )
    .subscribe(async (_) => {
      try {
        await this.updateTable();
      } catch (error) {
        this.snackBar.open('Error retrieving movies:' + error, 'Close', { duration: 3000 });
      }
    });

  }

}
