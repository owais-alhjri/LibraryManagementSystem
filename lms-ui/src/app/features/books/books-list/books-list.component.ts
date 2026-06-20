import { PagedBooksResponse } from './../../../core/models/book.model';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { BooksService } from '../../../core/services/books.service';
import { AuthService } from '../../../core/services/auth.service';
import { Book } from '../../../core/models/book.model';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { BookCardComponent } from '../book-card/book-card.component';
import { RouterLink } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { BorrowService } from '../../../core/services/borrow.service';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import {
  StateViewComponent,
  ViewState,
} from '../../../shared/components/state-view/state-view.component';

@Component({
  selector: 'app-books-list',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    BookCardComponent,
    RouterLink,
    StateViewComponent,
  ],
  templateUrl: './books-list.component.html',
  styleUrl: './books-list.component.css',
})
export class BooksListComponent implements OnInit {
  private dialog = inject(MatDialog);
  private borrowService = inject(BorrowService);
  private booksService = inject(BooksService);
  auth = inject(AuthService);

  displayedColumns = ['title', 'author', 'bookState', 'actions'];

  searchControl = new FormControl('');

  books = signal<Book[]>([]);
  totalCount = signal(0);
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  page = signal(1);
  pageSize = signal(10);

  viewState = computed<ViewState>(() => {
    if (this.isLoading()) return 'loading';
    if (this.errorMessage()) return 'error';
    if (this.books().length === 0) return 'empty';
    return 'success';
  });

  constructor() {
    this.searchControl.valueChanges
      .pipe(debounceTime(400), distinctUntilChanged(), takeUntilDestroyed())
      .subscribe(() => {
        this.page.set(1);
        this.loadBooks();
      });
  }

  ngOnInit() {
    this.loadBooks();
  }

  loadBooks() {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.booksService
      .getBooks(this.page(), this.pageSize(), this.searchControl.value ?? '')
      .subscribe({
        next: (res: PagedBooksResponse) => {
          this.books.set(res.items);
          this.totalCount.set(res.totalCount);
          this.isLoading.set(false);
        },
        error: () => {
          this.errorMessage.set('Failed to load books. Please try again.');
          this.isLoading.set(false);
        },
      });
  }

  onPageChange(event: PageEvent) {
    this.page.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.loadBooks();
  }

  getStateColor(state: string): string {
    switch (state?.toUpperCase()) {
      case 'AVAILABLE':
        return 'primary';
      case 'BORROWED':
        return 'warn';
      case 'RETURNED':
        return 'accent';
      default:
        return '';
    }
  }

  onBorrow(book: Book) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Borrow Book',
        message: `Are you sure you want to borrow "${book?.title}"`,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      const bookId = book.id;
      if (!bookId) return;

      this.borrowService.borrowBook({ bookId }).subscribe({
        next: () => {
          this.books.update((books) =>
            books.map((b) =>
              b.id === bookId ? { ...b, bookState: 'Borrowed' } : b,
            ),
          );
        },
        error: () => {
          console.error('failed to borrow book');
        },
      });
    });
  }

  onBookDeleted(bookId: string) {
    this.books.update((books) => books.filter((book) => book.id !== bookId));

    this.totalCount.update((count) => count - 1);
  }
}
