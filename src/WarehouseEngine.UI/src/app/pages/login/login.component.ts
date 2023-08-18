import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoginService } from 'src/app/services/login/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  protected credentials = new FormGroup(
    {
      username: new FormControl<string>('', {
        nonNullable: true,
        validators: [Validators.required]
      }),
      password: new FormControl<string>('', {
        nonNullable: true,
        validators: [Validators.required]
      })
    }
  );

  constructor(private readonly loginService: LoginService){}

  protected login(): void {
    if(!this.credentials.valid) return;

    this.loginService.login$(
      this.credentials.controls.username.value,
      this.credentials.controls.password.value
    ).subscribe();
  }
}