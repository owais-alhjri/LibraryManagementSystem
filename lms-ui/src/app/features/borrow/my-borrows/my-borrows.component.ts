import { ConfirmDialogComponent } from './../../../shared/components/confirm-dialog/confirm-dialog.component';
import { Component, computed, inject, signal, OnInit } from '@angular/core';
import { BorrowService } from '../../../core/services/borrow.service';
import { BorrowRecord } from '../../../core/models/borrow.model';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { StateViewComponent, ViewState } from '../../../shared/components/state-view/state-view.component';

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
    MatChipsModule,
    DatePipe,
    StateViewComponent,
  ],
  templateUrl: './my-borrows.component.html',
  styleUrl: './my-borrows.component.css',
})
export class MyBorrowsComponent implements OnInit {
  private borrowService = inject(BorrowService);
  private dialog = inject(MatDialog);

  borrows = signal<BorrowRecord[]>([]);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);

  viewState = computed<ViewState>(() => {
    if (this.isLoading()) return 'loading';
    if (this.errorMessage()) return 'error';
    if (this.borrows().length === 0) return 'empty';
    return 'success';
  });

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
              r.id === borrowed.id
                ? { ...r, status: 'Returned', returnedDate: new Date().toISOString() }
                : r,
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