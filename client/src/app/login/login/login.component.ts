import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService, LoginDTO } from 'src/app/core/api/tennis-service';
import { TokenDTO } from 'src/app/core/api/tennis-service/model/tokenDTO';
import { TokenHandlerService } from 'src/app/core/services/token-handler.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  returnUrl: any = '/';

  constructor(
    private authService: AuthService,
    private tokenHandlerService: TokenHandlerService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.returnUrl = this.route.snapshot?.queryParams['returnUrl'] || '/';
  }

  login() {
    const loginDTO: LoginDTO = {
      email: this.email,
      password: this.password,
    };
    this.authService.apiAuthLoginPost(loginDTO).subscribe({
      next: (data) => {
        const tokenDTO = data.success as TokenDTO;
        this.tokenHandlerService.saveToken(tokenDTO.jwtToken!);
        this.tokenHandlerService.saveRefreshToken(tokenDTO.refreshToken!);
        this.tokenHandlerService.saveUserId(tokenDTO.jwtToken!);
        this.router.navigate([this.returnUrl]);
      },
      error: (error) => {
        throw error;
      },
    });
  }
}
