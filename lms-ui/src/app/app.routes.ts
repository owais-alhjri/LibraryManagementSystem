import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';
import { manageBooksGuard } from './core/guards/managr-books.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'books',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: ()=>
      import('./features/auth/auth.routes').then(m => m.authRoutes)
  },
  {
    path: 'books',
    canActivate: [authGuard],
    loadChildren: () =>
            import('./features/books/books.routes').then(m => m.booksRoutes)
  },
  {
    path: 'borrow',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/borrow/borrow.routes').then(m => m.borrowRoutes)
  },
  {
    path: 'admin',
    canActivate: [authGuard, AdminGuard],
    loadChildren: () =>
      import('./features/admin/admin.routes').then(m => m.adminRoutes)
  },
  {
    path: 'librarian',
    canActivate: [authGuard, manageBooksGuard],
    loadChildren:()=>
      import('./features/librarian/librarian.routes').then(m => m.librarianRoutes)
  },
  {
    path: '**',
    redirectTo: 'books'
  }
];
