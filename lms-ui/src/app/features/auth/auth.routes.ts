import { Routes } from '@angular/router';
import { Component } from '@angular/core';

@Component({ standalone: true, template: '<p>Login coming soon</p>' })
class PlaceholderComponent {}

export const authRoutes: Routes = [
  {
    path: 'login',
    component: PlaceholderComponent
  }
];
