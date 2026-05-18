import { Component } from '@angular/core';
import { CartService } from '../../services/cart-service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-cart',
  imports: [RouterModule],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
})
export class Cart {
  constructor(public cartService:CartService,
    private router:Router
  ){}
  goCheckout() {
  console.log('clicked');            
  this.router.navigate(['/checkout']);
}
}
