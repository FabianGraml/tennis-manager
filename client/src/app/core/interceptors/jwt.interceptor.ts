import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { AuthService, RefreshTokenDTO } from '../api/tennis-service';
import { TokenHandlerService } from '../services/token-handler.service';
import { TokenDTO } from '../api/tennis-service/model/tokenDTO';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private tokenHandlerService: TokenHandlerService
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    let authreq = request;
    authreq = this.addTokenHeader(request, this.tokenHandlerService.getToken());
    return next.handle(authreq).pipe(
      catchError((err) => {
        if (err.status === 401) {
          const refreshToken = this.tokenHandlerService.getRefreshToken();
          if (!refreshToken) {
            return throwError(new Error('Refresh token not found'));
          }
          const tokenDTO: RefreshTokenDTO = {
            refreshToken: refreshToken,
          };
          return this.authService.apiAuthRefreshTokenPost(tokenDTO).pipe(
            switchMap((data: TokenDTO) => {
              console.log(data);
              this.tokenHandlerService.saveToken(data.jwtToken!);
              this.tokenHandlerService.saveRefreshToken(data.refreshToken!);
              this.tokenHandlerService.saveUserId(data.jwtToken!);
              authreq = this.addTokenHeader(
                request,
                this.tokenHandlerService.getToken()
              );
              return next.handle(authreq).pipe(
                catchError((error) => {
                  return throwError(error);
                })
              );
            }),
            catchError((error) => {
              return throwError(error);
            })
          );
        }
        return next.handle(authreq) as Observable<HttpEvent<unknown>>;
      })
    ) as Observable<HttpEvent<unknown>>;
  }

  addTokenHeader(request: HttpRequest<any>, token: any) {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
}
