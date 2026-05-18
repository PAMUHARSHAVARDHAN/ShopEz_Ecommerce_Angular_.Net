import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  imports: [CommonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit { 
  user:any;
  ngOnInit(): void {
    const userData = localStorage.getItem('user');

  if (userData) {
    this.user = JSON.parse(userData);
  } else {
    this.user = null;
  }
    
  }
 
}
