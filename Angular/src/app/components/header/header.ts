import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-header',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './header.html',
  styleUrls: ['./header.css'],
})
export class Header implements  OnInit{
  isOpen=false;
  user:any;
   searchText: string = '';
  constructor(private authService:AuthService,
    private router:Router){}
     searchProducts() {

    if(this.searchText.trim()) {

      this.router.navigate(['/products'], {
        queryParams: { search: this.searchText }
      });

    }
     this.searchText = '';

  }

  ngOnInit(): void {
   this.authService.currentUser$
      .subscribe(user => {

        this.user = user;
      });
  }
    isAdmin() {
  return this.user?.role === 'Admin';
}
  
  toggleMenu(){
    this.isOpen=!this.isOpen;
  }

  closeMenu(){
    this.isOpen=false;
  }
  
  isLoggedIn() {
    return this.authService.isLoggedIn();
  }
    logout() {
    this.authService.logout();
    this.router.navigate(['/auth']);
  }
}
