import { Component } from '@angular/core';
import { AuthService, RefreshTokenDTO } from 'src/app/core/api/tennis-service';
import { TokenHandlerService } from 'src/app/core/services/token-handler.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  email: string = '';
  isUserLoggedIn: boolean = false;
  constructor(private tokenHandlerService : TokenHandlerService, private authService: AuthService) {
    this.email = this.tokenHandlerService.getEmailFromToken()!;
    this.isUserLoggedIn = this.tokenHandlerService.isUserLoggedIn();
  }
  logout(): void {
    var refreshTokenDTO : RefreshTokenDTO = {
      refreshToken: this.tokenHandlerService.getRefreshToken()!
    }
    this.authService.apiAuthLogoutPost(refreshTokenDTO).subscribe({
      next: (data) => {
        this.tokenHandlerService.signOut();
        console.log(data);
        window.location.reload();
      },
      error: (error) => {
        throw error;
      },
    });
  }
}