import { CommonModule } from '@angular/common';
import { Component, inject, type OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { tap } from 'rxjs';
import { LoginService } from 'src/app/services/login/login.service';
import type { JwtTokenResponse } from 'src/app/types/jwt';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatCardModule, MatInputModule, MatButtonModule],
  standalone: true,
})
export class LoginComponent implements OnInit {
  private readonly loginService: LoginService = inject(LoginService);
  private readonly authService: AuthService = inject(AuthService);
  private readonly router: Router = inject(Router);

  private readonly demoUsername = 'demo';
  // deepcode ignore NoHardcodedPasswords: This is demo credentials only
  private readonly demoPassword = 'P@ssword1';
  protected credentials = new FormGroup({
    username: new FormControl<string>(this.demoUsername, {
      nonNullable: true,
      validators: [Validators.required],
    }),
    password: new FormControl<string>(this.demoPassword, {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  ngOnInit(): void {
    if (this.authService.getJwtToken() != null) {
      console.log('gotten token');
    }
  }

  protected login(): void {
    console.log(this.credentials.valid);
    if (!this.credentials.valid) return;

    this.loginService
      .login$(this.credentials.controls.username.value, this.credentials.controls.password.value)
      .pipe(
        tap((res: JwtTokenResponse) => {
          if (res.type === 'Success') {
            const jwt = res.jwt;
            this.authService.setJwtToken(jwt);
            this.router.navigate(['/']);
          } // todo: handle else
        })
      )
      .subscribe();
  }
}
