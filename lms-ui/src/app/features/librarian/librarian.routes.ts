import { Routes } from "@angular/router";

export const librarianRoutes: Routes = [
    {
        path: 'add-book',
        loadComponent: () =>
            import('../librarian/add-book/add-book.component').then(m => m.AddBookComponent)
    },
    {
        path: 'edit-book/:id',
        loadComponent: ()=>
            import('../librarian/edit-book/edit-book.component').then(m=>m.EditBookComponent)
    }
]