import { RegisterRequest } from './../models/auth.model';
import { HttpClient } from "@angular/common/http";
import { computed, inject, Injectable, signal } from "@angular/core";
import { Router } from "@angular/router";
import { AuthResponse, LoginRequest, TokenPayload } from "../models/auth.model";
import { Role } from "../models/user.model";
import { environment } from "../../../environments/environment";
import { tap } from "rxjs";

@Injectable({providedIn: 'root'})
export class AuthService{

  private http = inject(HttpClient);
  private router = inject(Router);

  private readonly TOKEN_KEY = 'lms_token';

  private tokenSignal = signal<string | null>(localStorage.getItem(this.TOKEN_KEY));

  private payload = computed<TokenPayload | null>(()=>{
    const token = this.tokenSignal();
    if(!token) return null;
    return this.decodeToken(token);
  });

  isLoggedIn = computed(()=> !!this.tokenSignal());
  currentRole = computed<Role | null>(()=> this.payload()?.role as Role ?? null);
  currentUserId = computed<string | null>(()=> this.payload()?.sub ?? null);
  currentEmail = computed<string | null>(()=> this.payload()?.email ?? null);

  // Role helpers
  isAdmin = computed(()=> this.currentRole() === 'ADMIN');
  isLibrarian = computed(() => this.currentRole() === 'LIBRARIAN');
  isMember = computed(() => this.currentRole() === 'MEMBER');
  canManageBooks = computed(() => this.isAdmin() || this.isLibrarian());

  login(credentials: LoginRequest){
    return this.http.post<AuthResponse>(`${environment.apiUrl}/User/login`, credentials).pipe(
      tap(res =>{
         this.setToken(res.token);
        this.router.navigate(['/books']);
      })
    );
  }

  register(data: RegisterRequest){
    return this.http.post<AuthResponse>(`${environment.apiUrl}/User`,data).pipe(
      tap(res =>{
        this.setToken(res.token);
        this.router.navigate(['/books']);
      })
    );
  }

  logout(){
    this.clearToken();
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null{
    return this.tokenSignal();
  }

  private setToken(token: string){
    localStorage.setItem(this.TOKEN_KEY, token);
    this.tokenSignal.set(token);
  }

  private clearToken(){
    localStorage.removeItem(this.TOKEN_KEY);
    this.tokenSignal.set(null);
  }

  private decodeToken(token: string): TokenPayload | null{
    try{
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    }catch{
      return null;
    }
  }

}
