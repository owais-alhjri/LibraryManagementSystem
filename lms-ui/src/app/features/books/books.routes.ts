import { Routes } from '@angular/router';
export const booksRoutes: Routes = [
{
    path: '',
    loadComponent: () =>
        import('./books-list/books-list.component').then(m=>m.BooksListComponent)
},
{
    path:':id',
    loadComponent: () =>
        import('./book-detail/book-detail.component').then(m=>m.BookDetailComponent)
}
];
