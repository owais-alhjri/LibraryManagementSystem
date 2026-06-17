import { Routes } from '@angular/router';
export const borrowRoutes: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./my-borrows/my-borrows.component').then(m => m.MyBorrowsComponent)
    }
];
