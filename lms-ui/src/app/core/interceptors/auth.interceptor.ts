import { AuthService } from './../services/auth.service';
import {
  HttpErrorResponse,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import {
  BehaviorSubject,
  catchError,
  filter,
  switchMap,
  take,
  throwError,
} from 'rxjs';
import { Token } from '@angular/compiler';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const authReq = addToken(req, authService.getToken());

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      const isAuthEndpoint =
        req.url.includes('/User/login') || req.url.includes('/User/refresh');

      if (error.status === 401 && !isAuthEndpoint) {
        return handle401(authReq, next, authService);
      }

      return throwError(() => error);
    }),
  );
};

function addToken(req: HttpRequest<unknown>, token: string | null) {
  return req.clone({
    withCredentials: true,
    headers: token
      ? req.headers.set('Authorization', `Bearer ${token}`)
      : req.headers,
  });
}

function handle401(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService,
) {
  if (!isRefreshing) {
    isRefreshing = true;
    refreshTokenSubject.next(null);

    return authService.refreshAccessToken().pipe(
      switchMap((res) => {
        isRefreshing = false;
        refreshTokenSubject.next(res.token);
        return next(addToken(req, res.token));
      }),
      catchError((err) => {
        isRefreshing = true;
        authService.logout();
        return throwError(() => err);
      }),
    );
  }

  return refreshTokenSubject.pipe(
    filter((token) => token !== null),
    take(1),
    switchMap((token) => next(addToken(req, token))),
  );
}
