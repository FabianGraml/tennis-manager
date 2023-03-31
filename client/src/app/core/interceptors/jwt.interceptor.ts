import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { AuthService, RefreshTokenDTO } from '../api/tennis-service';
import { TokenHandlerService } from '../services/token-handler.service';
import { TokenDTO } from '../api/tennis-service/model/tokenDTO';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private tokenHandlerService: TokenHandlerService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    let authreq = request;
    authreq = this.addTokenHeader(request, this.tokenHandlerService.getToken());
    return next.handle(authreq).pipe(
      catchError((err) => {
        if (err.status === 401) {
          const tokenDTO : RefreshTokenDTO = {
            refreshToken: this.tokenHandlerService.getRefreshToken(),
          };
          this.authService.apiAuthRefreshTokenPost(tokenDTO).subscribe({
            next: (data) => {
              const tokenDTO = data as TokenDTO;
              this.tokenHandlerService.saveToken(tokenDTO.jwtToken!);
              this.tokenHandlerService.saveRefreshToken(tokenDTO.refreshToken!);
              this.tokenHandlerService.saveUserId(tokenDTO.jwtToken!);
              authreq = this.addTokenHeader(request, this.tokenHandlerService.getToken());
              return next.handle(authreq);
            },
            error: (error) => {
              console.log(error);
            }
          });
        }
        return next.handle(authreq);
      }));
  }
  addTokenHeader(request: HttpRequest<any>, token:any){
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
}