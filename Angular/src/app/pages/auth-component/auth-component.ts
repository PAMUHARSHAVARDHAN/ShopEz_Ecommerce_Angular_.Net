import { Component, OnInit,ChangeDetectorRef } from '@angular/core';
import { AuthService } from '../../services/auth-service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgForm } from '@angular/forms';


@Component({
  selector: 'app-auth-component',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './auth-component.html',
  styleUrl: './auth-component.css',
})
export class AuthComponent implements OnInit{
  isSignUp=false;
   registerData={
    fullName:'',
    email:'',
    password:'',
    mobileNo:''
   };

    loginData={
    email:'',
    password:''
    };
    successMessage = '';
    errorMessage = '';
loginErrorMessage = '';
 showLoginPassword = false;
showRegisterPassword = false;

    constructor(private authService:AuthService,
      private router:Router,
      private cdr: ChangeDetectorRef
    ){}
    ngOnInit(){
      if(this.authService.isLoggedIn()){
        this.router.navigate(['/']);
      }
    }
    toggle(val:boolean){
      this.isSignUp=val;
    }
  onRegister(form: NgForm) {
    // Clear previous messages before every submission
    this.successMessage = '';
    this.errorMessage = '';
 
    this.authService.register(this.registerData)
      .subscribe({
        next: () => {
          this.successMessage = 'Registration successful';
 
          // Clear form fields
          this.registerData = {
            fullName: '',
            email: '',
            password: '',
            mobileNo: ''
          };
          form.resetForm();
 
          // Auto-hide success message after 3 seconds
          setTimeout(() => {
            this.successMessage = '';
          }, 3000);
        },
 
        error: (err: any) => {
          this.successMessage = '';
          this.errorMessage =
            err?.error?.message || 'Registration failed';
        }
      });
  }
 
  onLogin() {
    this.loginErrorMessage = '';
  console.log('onLogin called');

  this.authService.login(this.loginData).subscribe({
    next: (res) => {
      console.log('next called', res);
      this.router.navigate(['/']);
    },
    error: (err) => {
       this.loginErrorMessage = err.error.message;
        this.cdr.detectChanges()
    }
  });
  }
  

  
}
