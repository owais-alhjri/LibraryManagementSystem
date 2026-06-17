import { ConfirmDialogComponent } from './../../../shared/components/confirm-dialog/confirm-dialog.component';
import { Component, inject, signal } from '@angular/core';
import { BorrowService } from '../../../core/services/borrow.service';
import { BorrowRecord } from '../../../core/models/borrow.model';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-my-borrows',
  standalone: true,
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    DatePipe,
  ],
  templateUrl: './my-borrows.component.html',
  styleUrl: './my-borrows.component.css',
})
export class MyBorrowsComponent {
  private borrowService = inject(BorrowService);
  private dialog = inject(MatDialog);

  borrows = signal<BorrowRecord[]>([]);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);

  ngOnInit() {
    this.borrowService.getMyBorrowRecords().subscribe({
      next: (borrows) => {
        this.borrows.set(borrows);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
        this.errorMessage.set('Failed to load borrow records.');
      },
    });
  }

  onReturn(borrowed: BorrowRecord) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Return Book',
        message: `Are you sure you want to return "${borrowed.bookTitle}"?`,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      this.borrowService.returnBook({ bookId: borrowed.bookId }).subscribe({
        next: () => {
          this.borrows.update((records) =>
            records.map((r) =>
              r.id === borrowed.id ? { ...r, status: 'Returned' } : r,
            ),
          );
        },
        error: () => {
          console.error('Failed to return book');
        },
      });
    });
  }
}
