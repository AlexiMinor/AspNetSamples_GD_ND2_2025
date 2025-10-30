import {Component, OnInit, signal} from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import {TokenService} from '../services/token/token.service';
import {userSignal} from '../signals/user-signal';
import {AuthService} from '../services/auth/auth.service';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})

export class App implements OnInit{
  userSignal = userSignal.asReadonly();
  isAdmin = signal(false);
  title: string = 'good-news-aggregator';

  constructor(private tokenService: TokenService, private authService: AuthService) {}

  ngOnInit(): void {
    // this.tokenService.currentAccessToken.subscribe(token => {
    //   this.isUserLoggedIn.set(token !== null);
    // });
    this.isAdmin.set(this.authService.isAdmin());
  }

  logout(): void {
    this.tokenService.removeToken();
    // this.isUserLoggedIn.set(false);
  }
}
