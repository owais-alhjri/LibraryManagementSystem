import { ConfirmDialogComponent } from './../../../shared/components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BooksService } from '../../../core/services/books.service';
import { Book } from '../../../core/models/book.model';
import { BorrowService } from '../../../core/services/borrow.service';

@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [],
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.css'
})
export class BookDetailComponent {

  private route = inject(ActivatedRoute);
  private bookService = inject(BooksService);

  private dialog = inject(MatDialog);
  private borrowService = inject(BorrowService);

  book = signal<Book | null>(null);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);


  ngOnInit(){
    const id = this.route.snapshot.paramMap.get('id');

    if(!id){
      this.errorMessage.set('Book not found');
      this.isLoading.set(false);
      return;
    }

    this.bookService.getBookById(id).subscribe({
      next:(book) =>{
        this.book.set(book);
        this.isLoading.set(false);
      },
      error: () =>{
        this.errorMessage.set('Failed to load book');
        this.isLoading.set(false);
      }
    });
  }

  onBorrow(){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data:{
        title: 'Borrow Book',
        message:`Are you sure you want to borrow "${this.book()?.title}"`
      }
    });

    dialogRef.afterClosed().subscribe(result =>{
      if(!result) return;

      const bookId = this.book()?.id;
      if(!bookId) return;

      this.borrowService.borrowBook({bookId}).subscribe({
        next: () =>{
          this.book.update(b=>b ? {...b, bookState: 'Borrowed'}: b);
        },
        error:()=> {
          console.error('failed to borrow book');
        }
      })
    })
  }

}
