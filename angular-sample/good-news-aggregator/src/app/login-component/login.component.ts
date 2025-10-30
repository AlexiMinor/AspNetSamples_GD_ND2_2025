import {Component, signal} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {AuthService} from '../../services/auth/auth.service';
import {Article} from '../../models/article';
import {TokenPair} from '../../models/tokenPair';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login-component',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  tokenPair = signal<TokenPair| null >(null);
  loginForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.formBuilder.group({
      email: [''],
      password: ['']
    });
  }

  login(): void {
    const value = this.loginForm.value;

    if (value.email && value.password) {
      this.authService.login(value.email, value.password) .subscribe(tokenPair => {
        this.tokenPair.set(tokenPair);
        this.router.navigate(['/articles']);
      });
    }
  }
}
