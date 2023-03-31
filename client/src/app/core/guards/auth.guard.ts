import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, tap } from 'rxjs';
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
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const jwtToken = this.tokenHandler.getToken();
    const refreshToken = this.tokenHandler.getRefreshToken();
    const isExpired = this.isJwtTokenExpired(jwtToken!);
    if (!jwtToken || isExpired) {
      const tokenDTO: RefreshTokenDTO = {
        refreshToken: refreshToken,
      };
     this.authService.apiAuthRefreshTokenPost(tokenDTO).subscribe({
        next: (data) => {
          const tokenDTO = data as TokenDTO;
          this.tokenHandler.saveToken(tokenDTO.jwtToken!);
          this.tokenHandler.saveRefreshToken(tokenDTO.refreshToken!);
          this.tokenHandler.saveUserId(tokenDTO.jwtToken!);
          return of(true);
        },
        error: (error) => {
          console.error(error);
          this.router.navigate(['/login'], {queryParams: {returnUrl: state.url}});
          return of(false);
        }
      });
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
