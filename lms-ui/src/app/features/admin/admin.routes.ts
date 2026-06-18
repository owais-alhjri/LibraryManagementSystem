import { Routes } from '@angular/router';
export const adminRoutes: Routes = [
    {
        path: 'add-book',
        loadComponent: () =>
            import('./add-book/add-book.component').then(m => m.AddBookComponent)
    },
    {
        path: 'edit-book/:id',
        loadComponent: ()=>
            import('./edit-book/edit-book.component').then(m=>m.EditBookComponent)
    }
];
