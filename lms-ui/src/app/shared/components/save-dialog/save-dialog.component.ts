import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-save-dialog',
  standalone:true,
  imports: [MatButtonModule, MatDialogModule,
    MatFormFieldModule, MatSelectModule, ReactiveFormsModule
  ],
  templateUrl: './save-dialog.component.html',
  styleUrl: './save-dialog.component.css'
})
export class SaveDialogComponent {
  
  private fb = new FormBuilder();
  dialogRef = inject(MatDialogRef<ConfirmDialogComponent>)

  form = this.fb.group({
    role: ['']
  });

  save(){
    this.dialogRef.close(this.form.value.role);
  }

}
