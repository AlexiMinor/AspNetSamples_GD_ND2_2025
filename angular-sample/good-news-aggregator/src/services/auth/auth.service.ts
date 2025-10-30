import { Injectable } from '@angular/core';
import {ApiService} from '../api/api.service';
import {TokenPair} from '../../models/tokenPair';
import {Observable, tap} from 'rxjs';
import {TokenService} from '../token/token.service';
import {userSignal} from '../../signals/user-signal';
import {jwtDecode} from 'jwt-decode';
import {Token} from '../../models/token';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  constructor(private apiService: ApiService,
              private tokenService: TokenService) {
  }

  login(email: string, password: string): Observable<TokenPair | null> {
    return this.apiService.post<TokenPair>('token/login', {email, password}).pipe(
      tap((token) => this.authorize(token))
    );
  }

  refreshToken(): Observable<TokenPair | null>{
    return this.apiService.post<TokenPair>('token/refresh', {refreshToken: this.tokenService.getRefreshToken()})
      .pipe(
        tap((token) => {
          this.authorize(token);
        })
      )
  }

  isAdmin(): boolean {
    const tokenString = this.tokenService.getToken();
    if (tokenString) {
      const token:Token =  jwtDecode(tokenString);
      return token.role === 'Admin';
    }
    return false;
  }

  isLoggedIn(): boolean {
    const tokenString = this.tokenService.getToken();
    return tokenString !== null;
  }

  private authorize(token: TokenPair | null) : void {
    if (token && token.accessToken) {
      this.tokenService.setToken(token);
    }
  }
}
