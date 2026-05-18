import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product-service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Observable , map } from 'rxjs';

import { Product } from '../../Models/product';
import { CartService } from '../../services/cart-service';
 

@Component({
  selector: 'app-home',
  imports: [CommonModule,RouterModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit{
 products$!: Observable<Product[]>;
  brands=[
    {name:"Nike", logo:'/brandimages/nike.jpeg'},
     {name:"Mufti", logo:'/brandimages/mufti.png'},
      {name:"Levis", logo:'/brandimages/levis.png'},
       {name:"H&M", logo:'/brandimages/h&m.png'},

  ]

  constructor(private productService: ProductService,
    private cartService:CartService,
    private router:Router
    
  
  ) {}
  ngOnInit(): void {

    this.products$ = this.productService.getProducts()
      .pipe(
        map((products: Product[]) => 
          products.slice(0, 4))
      );
  }
  addToCart(product: any) {

  this.cartService.addToCart(product);

  // optional redirect
  this.router.navigate(['/cart']);
}
}
