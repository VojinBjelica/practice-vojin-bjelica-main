import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Customer, CustomerClient, UpdateCustomerCommand } from 'src/app/api/api-reference';


export interface DialogData {
  customerId: string
}

@Component({
  selector: 'app-customer-create-dialog',
  templateUrl: './customer-create-dialog.component.html',
  styleUrls: ['./customer-create-dialog.component.css']
})
export class CustomerCreateDialogComponent implements OnInit {
  customer: Customer = new Customer;

  formGroup = new FormGroup({
    email: new FormControl('', { nonNullable: true, validators: Validators.required }),
    role: new FormControl('', { nonNullable: true, validators: Validators.required })
  });

  constructor(public dialogRef: MatDialogRef<CustomerCreateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private readonly customerClient: CustomerClient,
    private readonly snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.customerClient.getById(this.data.customerId).subscribe(
      res => {
        this.customer = res;
        this.formGroup.patchValue({
          email: this.customer.email?.value,
          role: this.customer.role.toString()
        });
      });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onYesClick = () => {
    const updateCustomerCommand: UpdateCustomerCommand = new UpdateCustomerCommand({
      customerId: this.customer.id,
      email: this.formGroup.controls.email.value,
      role: +this.formGroup.controls.role.value
    });

    this.customerClient.update(updateCustomerCommand).subscribe({
      next: _ => {
        this.dialogRef.close();
      },
      error: error => {
        this.snackBar.open('Error retrieving movies:' + error, 'Close', { duration: 3000 });
      }
    });
  }



}
