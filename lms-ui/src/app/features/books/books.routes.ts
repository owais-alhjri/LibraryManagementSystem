import { Routes } from '@angular/router';
export const booksRoutes: Routes = [
    {
    path: '',
    redirectTo: '/auth/login',
    pathMatch: 'full'
  }
];
