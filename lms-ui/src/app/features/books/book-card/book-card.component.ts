import { Book } from './../../../core/models/book.model';
import { Component, EventEmitter, Input, Output } from '@angular/core';
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
import { MatDialog } from '@angular/material/dialog';
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

  @Input() book!: Book;

  @Output() borrow = new EventEmitter<Book>();

  onBorrow(){
    this.borrow.emit(this.book);
  }
  
}
