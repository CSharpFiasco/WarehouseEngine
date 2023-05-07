import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  protected username = new FormControl<string>('', [Validators.required]);
  protected password = new FormControl<string>('', [Validators.required]);
  
  constructor(){
    
  }
}