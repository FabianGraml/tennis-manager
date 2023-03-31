import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, RegisterDTO } from 'src/app/core/api/tennis-service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  firstname: string = '';
  lastname: string = '';
  email: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  register() {
    const registerDTO: RegisterDTO = {
      firstname: this.firstname,
      lastname: this.lastname,
      email: this.email,
      password: this.password,
    };
    this.authService.apiAuthRegisterPost(registerDTO).subscribe({
      next: (data) => {
        this.router.navigateByUrl('/login');
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
}