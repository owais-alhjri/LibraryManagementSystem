import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';


function passwordMatchValidator(control: AbstractControl): ValidationErrors | null{
  const password = control.get('password')?.value;
  const confirm = control.get('confirmPassword')?.value;
  return password === confirm ? null : {passwordMismatch: true};
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
        ReactiveFormsModule, RouterLink,
    MatFormFieldModule, MatInputModule,
    MatButtonModule, MatCardModule,
    MatProgressSpinnerModule, MatIconModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  private fb = inject(FormBuilder);
  private auth = inject(AuthService);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required]
  }, {validators: passwordMatchValidator});


  isSubmitting = false;
  errorMessage = '';
  hidePassword = true;
  hideConfirm = true;

  get name() { return this.form.get('name')!; }
  get email() { return this.form.get('email')!; }
  get password() { return this.form.get('password')!; }
  get confirmPassword() { return this.form.get('confirmPassword')!; }
  
onSubmit() {
    if (this.form.invalid) return;

    this.isSubmitting = true;
    this.errorMessage = '';

    const { name, email, password } = this.form.value;

    this.auth.register({ name: name!, email: email!, password: password! }).subscribe({
      next: () => {
        this.isSubmitting = false;
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.status === 409
          ? 'An account with this email already exists'
          : 'Something went wrong. Please try again.';
      }
    });
  }
}
