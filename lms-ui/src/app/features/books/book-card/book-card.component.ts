import { ConfirmDialogComponent } from './../../../shared/components/confirm-dialog/confirm-dialog.component';
import { Book } from './../../../core/models/book.model';
import { Component, EventEmitter, inject, Input, Output, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from "@angular/router";
import { BooksService } from '../../../core/services/books.service';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../../core/services/auth.service';
@Component({
  selector: 'app-book-card',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatTableModule, MatPaginatorModule,
    MatFormFieldModule, MatInputModule,
    MatButtonModule, MatIconModule,
    MatProgressSpinnerModule, MatChipsModule,
    RouterLink
],
  templateUrl: './book-card.component.html',
  styleUrl: './book-card.component.css'
})
export class BookCardComponent {
  private bookService = inject(BooksService);
  private dialog = inject(MatDialog);
  auth = inject(AuthService);

  errorMessage = signal<string | null>(null);

  @Input() book!: Book;

  @Output() borrow = new EventEmitter<Book>();
  @Output() deleted = new EventEmitter<string>();

  onBorrow(){
    this.borrow.emit(this.book);
  }
  
  onDelete(){
    const dialogRef = this.dialog.open(ConfirmDialogComponent,{
      data:{
        title: 'Delete Book',
        message: `Are you sure you want to delete "${this.book.title}"`
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if(!result) return;

      const bookId = this.book.id;
      if(!bookId) return;

      this.bookService.deleteBook(bookId).subscribe({
        next: ()=>{
          this.deleted.emit(bookId);
        },
        error: ()=>{
          this.errorMessage.set("Failed to delete book");
        }
      })
    })
  }
}
