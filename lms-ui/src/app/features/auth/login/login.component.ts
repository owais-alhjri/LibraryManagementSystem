import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink,
    MatFormFieldModule, MatInputModule,
    MatButtonModule, MatCardModule,
    MatProgressSpinnerModule, MatIconModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);

  form = this.fb.group({
    email: ['',[Validators.required, Validators.email]],
    password: ['',[Validators.required, Validators.minLength(6)]]
  });

isSubmitting = false;
errorMessage = '';
hidePassword = true;

get email() {return this.form.get('email')!;}
get password(){return this.form.get('password')!;}

onSubmit(){
  if(this.form.invalid)return;
  this.isSubmitting = true;
  this.errorMessage = '';

  this.auth.login(this.form.value as any).subscribe({
    next: () => {
      this.isSubmitting = false;
    },
    error: (err)=>{
      this.isSubmitting = false;
      this.errorMessage = err?.status === 401
      ? 'Invalid email or password'
      : 'Something went wrong. Please try again.';
    }
  });
}

}
