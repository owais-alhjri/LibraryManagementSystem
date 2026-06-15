import { PagedBooksResponse } from './../../../core/models/book.model';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { BooksService } from '../../../core/services/books.service';
import { AuthService } from '../../../core/services/auth.service';
import { Book } from '../../../core/models/book.model';
import { debounce, debounceTime, distinctUntilChanged, single } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';


@Component({
  selector: 'app-books-list',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatTableModule, MatPaginatorModule,
    MatFormFieldModule, MatInputModule,
    MatButtonModule, MatIconModule,
    MatProgressSpinnerModule, MatChipsModule
  ],
  templateUrl: './books-list.component.html',
  styleUrl: './books-list.component.css'
})
export class BooksListComponent implements OnInit {

  private booksService = inject(BooksService);
  auth = inject(AuthService);

  // Table columns
  displayedColumns = ['title', 'author', 'bookState', 'actions'];

  // Search input as a FormControl — so we can use RxJS operators on it
  searchControl = new FormControl('');

  // Signals for state
  books = signal<Book[]>([]);
  totalCount = signal(0);
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  // Pagination state
  page = signal(1);
  pageSize = signal(10);

  constructor() {
    // takeUntilDestroyed must be called in constructor
    this.searchControl.valueChanges.pipe(
      debounceTime(400),              // wait 400ms after user stops typing
      distinctUntilChanged(),         // only emit if value actually changed
      takeUntilDestroyed()            // auto-unsubscribe when component destroys
    ).subscribe(() => {
      this.page.set(1);              // reset to page 1 on new search
      this.loadBooks();
    });
  }

  ngOnInit() {
    this.loadBooks();
  }

  loadBooks() {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.booksService.getBooks(
      this.page(),
      this.pageSize(),
      this.searchControl.value ?? ''
    ).subscribe({
      next: (res: PagedBooksResponse) => {
        this.books.set(res.items);
        this.totalCount.set(res.totalCount);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to load books. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  onPageChange(event: PageEvent) {
    this.page.set(event.pageIndex + 1);  // mat-paginator is 0-indexed
    this.pageSize.set(event.pageSize);
    this.loadBooks();
  }

  getStateColor(state: string): string {
    switch (state?.toUpperCase()) {
      case 'AVAILABLE': return 'primary';
      case 'BORROWED': return 'warn';
      default: return '';
    }
  }

}
