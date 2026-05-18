import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {BehaviorSubject, tap } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiurl= `${environment.apiGateway}/User`;
   private currentUserSubject =
    new BehaviorSubject<any>(this.getUser());

  currentUser$ =
    this.currentUserSubject.asObservable();
  constructor(private httpClient:HttpClient) {}
  register(data:any){
    return this.httpClient.post(`${this.apiurl}/register`,data);
  }
  login(data:any){
    return this.httpClient.post(`${this.apiurl}/login`,data)
    .pipe(
      tap((res:any)=>{
          console.log('LOGIN RESPONSE:', res); 
        localStorage.setItem('token',res.token);
     
          localStorage.setItem('role', res.role);
          const user = {
            userId: res.userId,
            fullName: res.name,
            email: res.email,
            role: res.role
          };
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSubject.next(user);
      })
    );
  }
   logout() {
    localStorage.clear();
    this.currentUserSubject.next(null);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
  getUser() {
  const user = localStorage.getItem('user');
  return user ? JSON.parse(user) : null;
}
}
