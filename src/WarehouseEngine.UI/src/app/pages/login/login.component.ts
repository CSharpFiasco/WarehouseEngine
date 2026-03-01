import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthStore } from '../../store/auth/auth.store';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    imports: [ReactiveFormsModule, MatFormFieldModule, MatCardModule, MatInputModule, MatButtonModule]
})
export class LoginComponent {
  private readonly authStore = inject(AuthStore);

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

    this.authStore.login({
      username: this.credentials.controls.username.value,
      password: this.credentials.controls.password.value,
    });
  }
}
