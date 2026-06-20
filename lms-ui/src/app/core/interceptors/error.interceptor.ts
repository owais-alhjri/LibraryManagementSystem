import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';
import { extractErrorMessage } from '../utils/error-message.util';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const message = extractErrorMessage(error);

      snackBar.open(message, 'Dismiss', {
        duration: 5000,
        panelClass: ['error-snackbar'],
      });

      return throwError(() => error);
    }),
  );
};
