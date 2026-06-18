import { BooksService } from './../../../core/services/books.service';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCard, MatCardModule } from "@angular/material/card";
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-edit-book',
  standalone: true,
  imports: [
    MatCard,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
  ],
  templateUrl: './edit-book.component.html',
  styleUrl: './edit-book.component.css',
})
export class EditBookComponent {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private bookService = inject(BooksService);
  private fb = inject(FormBuilder);

  private bookId = '';

  isLoading = signal(true);
  isSubmitting = false;
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    title: [
      '',
      [Validators.required, Validators.minLength(3), Validators.maxLength(100)],
    ],
    author: [
      '',
      [Validators.required, Validators.minLength(3), Validators.maxLength(100)],
    ],
  });

  get title(){return this.form.get('title')!;}
  get author(){return this.form.get('author')!;}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.errorMessage.set('Book not found');
      this.isLoading.set(false);
      return;
    }

    this.bookId = id;

    this.bookService.getBookById(id).subscribe({
      next: (book) => {
        this.form.patchValue(book);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to edit book');
        this.isLoading.set(false);
      },
    });
  }

  onSubmit() {
    if(this.form.invalid) return;

    this.isSubmitting = true;
    this.errorMessage.set(null);

    const data = {
        title: this.form.value.title!,
        author: this.form.value.author!
    };

    this.bookService.updateBook(this.bookId, data).subscribe({
      next: ()=>{
        this.isSubmitting = false;
        this.router.navigate(['/books']);
      },
      error: ()=>{
        this.isSubmitting = false;
        this.errorMessage.set('Failed to update book');
      }
    })
  }
}
