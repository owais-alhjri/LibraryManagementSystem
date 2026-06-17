import { ConfirmDialogData } from './../../../core/models/borrow.model';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  standalone:true,
  imports: [MatButtonModule, MatDialogModule ],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.css'
})
export class ConfirmDialogComponent {
 data = inject<ConfirmDialogData>(MAT_DIALOG_DATA);
 dialogRef = inject(MatDialogRef<ConfirmDialogComponent>);

 confirm(){
  this.dialogRef.close(true);
 }

 cancel(){
  this.dialogRef.close(false);
 }

}
