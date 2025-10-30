import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {TokenService} from '../services/token/token.service';
import {inject} from '@angular/core';
import {first, tap, pipe, catchError, switchMap, throwError} from 'rxjs';
import {AuthService} from '../services/auth/auth.service';
import {Router} from '@angular/router';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenService = inject(TokenService);
  const token = tokenService.getToken();
  const router = inject(Router);
  const authService = inject(AuthService);

  console.log(token);
  if (token){
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    })
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return authService.refreshToken().pipe(
          switchMap(() => {
            const newToken = tokenService.getToken();
            const clonedReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`
              }
            });
            return next(clonedReq);
          }),
          catchError((refreshError) => {
            tokenService.removeToken();
            router.navigateByUrl('/login');
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => error);
    })
  );
};
