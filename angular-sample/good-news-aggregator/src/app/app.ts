import {Component, OnInit, signal} from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import {TokenService} from '../services/token/token.service';
import {userSignal} from '../signals/user-signal';
import {AuthService} from '../services/auth/auth.service';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatListModule, MatNavList} from '@angular/material/list';
import {MediaMatcher} from '@angular/cdk/layout';
import {LoginDialogComponent} from './login-dialog-component/login-dialog.component';
import {MatDialog} from '@angular/material/dialog';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,
    RouterLink,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatNavList,
    MatListModule,
    MatSidenavModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})

export class App implements OnInit{
  userSignal = userSignal.asReadonly();
  isAdmin = signal(false);
  protected readonly isMobile = signal(true);

  private readonly _mobileQuery: MediaQueryList;
  private readonly _mobileQueryListener: () => void;

  constructor(private tokenService: TokenService,
              private authService: AuthService,
              private media: MediaMatcher,
              private dialog: MatDialog) {
    this._mobileQuery = media.matchMedia('(max-width: 600px)');
    this.isMobile.set(this._mobileQuery.matches);
    this._mobileQueryListener = () =>
      this.isMobile.set(this._mobileQuery.matches);
    this._mobileQuery.addEventListener('change', this._mobileQueryListener);
  }

  ngOnInit(): void {
    // this.tokenService.currentAccessToken.subscribe(token => {
    //   this.isUserLoggedIn.set(token !== null);
    // });
    this.isAdmin.set(this.authService.isAdmin());
  }

  openLoginDialog(): void{
    const loginModal = this.dialog.open(LoginDialogComponent);

    loginModal.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  logout(): void {
    this.tokenService.removeToken();
    // this.isUserLoggedIn.set(false);
  }
}
