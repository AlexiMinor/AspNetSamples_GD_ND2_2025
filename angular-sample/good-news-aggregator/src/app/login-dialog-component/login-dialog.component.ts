import {ChangeDetectionStrategy, Component, signal} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {AuthService} from '../../services/auth/auth.service';
import {TokenPair} from '../../models/tokenPair';
import {Router} from '@angular/router';
import {MatDialogModule, MatDialogRef} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';

@Component({
  selector: 'app-login-dialog-component',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './login-dialog.component.html',
  styleUrl: './login-dialog.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginDialogComponent {
  tokenPair = signal<TokenPair| null >(null);
  loginForm: FormGroup;

  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private router: Router,
              private dialogRef: MatDialogRef<LoginDialogComponent>) {
    this.loginForm = this.formBuilder.group({
      email: [''],
      password: ['']
    });
  }

  login(): void {
    console.log("hello");
    const value = this.loginForm.value;

    if (value.email && value.password) {
      this.authService.login(value.email, value.password) .subscribe(tokenPair => {
        this.tokenPair.set(tokenPair);
        this.dialogRef.close(true);
      });
    }
  }
}
