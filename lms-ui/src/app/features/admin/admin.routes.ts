import { Routes } from '@angular/router';
export const adminRoutes: Routes = [
    {
        path: 'all',
        loadComponent:()=>
            import('./update-role/update-role.component').then(m=>m.UpdateRoleComponent)
    }

];
