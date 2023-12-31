import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiHost + '/auth';
  helper: JwtHelperService;

  constructor(private http: HttpClient, private router: Router) {
    this.helper = new JwtHelperService();
  }

  signUp(user: User): Observable<User>{
    const url = this.apiUrl + '/sign-up';
    return this.http.post<User>(url, user);
  }

  signIn(user: User): Observable<User>{
    const url = this.apiUrl + '/sign-in';
    return this.http.post<User>(url, user);
  }

  sendRecoveryLink(user: User): Observable<User>{
    const url = this.apiUrl + '/send-recovery-link';
    return this.http.post<User>(url, user);
  }

  logout(){
    localStorage.removeItem('user');
    this.router.navigateByUrl('/auth/sign-in');
  }

  updatePassword(user: User, token: string){
    let httpHeaders = new HttpHeaders({ 'Authorization': 'Bearer ' + token });
    return this.http.put<User>(this.apiUrl+'/reset-password', user, { headers: httpHeaders });
  }
}