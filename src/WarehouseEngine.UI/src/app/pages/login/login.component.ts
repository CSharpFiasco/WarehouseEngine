import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { AuthFacade } from 'src/app/store/auth/auth.facade';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatCardModule, MatInputModule, MatButtonModule],
  standalone: true,
})
export class LoginComponent {
  private readonly loginFacade: AuthFacade = inject(AuthFacade);
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

  protected login(): void {
    if (!this.credentials.valid) return;

    this.loginFacade.login(this.credentials.controls.username.value, this.credentials.controls.password.value);
  }
}
