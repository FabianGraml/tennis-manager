import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, tap, map, catchError } from 'rxjs';
import { of } from 'rxjs';
import { AuthService, RefreshTokenDTO } from '../api/tennis-service';
import { TokenDTO } from '../api/tennis-service/model/tokenDTO';
import { TokenHandlerService } from '../services/token-handler.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private tokenHandler: TokenHandlerService,
    private authService: AuthService,
  ) {}

  
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const jwtToken = this.tokenHandler.getToken();
    const refreshToken = this.tokenHandler.getRefreshToken();
    const isExpired = this.isJwtTokenExpired(jwtToken!);
  
    if (!jwtToken && !refreshToken) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return of(false);
    }
  
    if (!jwtToken || isExpired) {
      if (!refreshToken) {
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return of(false);
      }
      const tokenDTO: RefreshTokenDTO = {
        refreshToken: refreshToken,
      };
      return this.authService.apiAuthRefreshTokenPost(tokenDTO).pipe(
        map((data: TokenDTO) => {
          this.tokenHandler.saveToken(data.jwtToken!);
          this.tokenHandler.saveRefreshToken(data.refreshToken!);
          this.tokenHandler.saveUserId(data.jwtToken!);
          return true;
        }),
        catchError((error) => {
          this.tokenHandler.signOut();
          this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
          return of(false);
        })
      );
    }
    return of(true);
  }
  isJwtTokenExpired(token: string): boolean {
    if (!token) {
      return true;
    }
    var isExpired = true;

    try{
      const jwtData = token.split('.')[1];
      const decodedJwtJsonData = window.atob(jwtData);
      const decodedJwtData = JSON.parse(decodedJwtJsonData);
      const exp = decodedJwtData.exp;
      isExpired = Math.floor(new Date().getTime() / 1000) >= exp;

    }catch(e){
      return isExpired;
    }
    return isExpired;
  }
}