import { CreateBookRequest } from './../../../core/models/book.model';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router, RouterLink } from '@angular/router';
import { BooksService } from '../../../core/services/books.service';

@Component({
  selector: 'app-add-book',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
  ],
  templateUrl: './add-book.component.html',
  styleUrl: './add-book.component.css',
})
export class AddBookComponent {
  private fb = inject(FormBuilder);
  private book = inject(BooksService);
  private router = inject(Router);

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

  isSubmitting = false;
  errorMessage = '';

  get title() {
    return this.form.get('title')!;
  }
  get author() {
    return this.form.get('author')!;
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.isSubmitting = true;
    this.errorMessage = '';

    const book: CreateBookRequest = {
      title: this.form.controls.title.value!,
      author: this.form.controls.author.value!,
    };

    this.book.createBook(book).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/books']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Error creating book, please try again.';
      },
    });
  }
}
