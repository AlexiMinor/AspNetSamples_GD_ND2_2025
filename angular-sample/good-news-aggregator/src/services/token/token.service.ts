import {Injectable, signal} from '@angular/core';
import {TokenPair} from '../../models/tokenPair';
import {CookieService} from 'ngx-cookie-service';
import {BehaviorSubject, Observable} from 'rxjs';
import {jwtDecode} from 'jwt-decode';
import {Token} from '../../models/token';
import {userSignal} from '../../signals/user-signal';

@Injectable({
  providedIn: 'root'
})

export class TokenService {
  public currentAccessToken: Observable<string | null>;
  private currentAccessTokenSubject: BehaviorSubject<string | null>;
  // public userSignal = signal<Token | null>(null);
  constructor(private cookieService: CookieService) {
    this.currentAccessTokenSubject = new BehaviorSubject<string | null>(this.getTokenFromStorage());
    this.currentAccessToken = this.currentAccessTokenSubject.asObservable();

    this.currentAccessToken.subscribe(token => {
      this.commitTokenToStorage(token);
      this.decodeToken(token);
    })
  }

  public getToken(): string | null {
    return this.currentAccessTokenSubject.value;
  }

  public setToken(token: TokenPair): void {
    this.currentAccessTokenSubject.next(token.accessToken);
    this.cookieService.set('refreshToken', token.refreshToken);
  }

  public removeToken(): void {
    this.currentAccessTokenSubject.next(null);
    this.cookieService.delete('refreshToken');
    userSignal.update(() => null);
  }

  decodeToken(token: string | null) {
    if (this.getToken()) {
      let decodedToken: Token = jwtDecode(<string>token);
      userSignal.update(() => decodedToken)
    }
  }

  public getRefreshToken(): string | null {
    return this.cookieService.get('refreshToken');
  }

  private getTokenFromStorage(): string | null{
    const token = localStorage.getItem('accessToken');
    return token ? JSON.parse(token) : null;
  }

  private commitTokenToStorage(token: string | null): void {
    localStorage.setItem('accessToken', JSON.stringify(token))
  }
}
