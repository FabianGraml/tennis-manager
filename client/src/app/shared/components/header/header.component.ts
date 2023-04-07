import { Component } from '@angular/core';
import { TokenHandlerService } from 'src/app/core/services/token-handler.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  email: string = '';
  isUserLoggedIn: boolean = false;
  constructor(private tokenHandlerService : TokenHandlerService) {
    this.email = this.tokenHandlerService.getEmailFromToken()!;
    this.isUserLoggedIn = this.tokenHandlerService.isUserLoggedIn();
  }
  logout(): void {
    this.tokenHandlerService.signOut();
    window.location.reload();  
  }
}