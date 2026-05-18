import { CommonModule, } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../../Models/product';
import { ProductService } from '../../services/product-service';
import { ActivatedRoute, Router,RouterModule } from '@angular/router';
import { CartService } from '../../services/cart-service';

@Component({
  selector: 'app-product-details',
  imports: [CommonModule,RouterModule],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css',
})
export class ProductDetails implements OnInit{
  product$! : Observable<Product>;
  selectedSize: string = '';

selectSize(size: string) {

  this.selectedSize = size;

}
constructor(private route:ActivatedRoute,
  private productService:ProductService,
  private cartService: CartService,
  private router:Router){}

  
  ngOnInit(): void {
     const id = Number(this.route.snapshot.paramMap.get('id'));
    this.product$ = this.productService.getProductById(id);
  }
  addToCart(product: any) {

   if (!this.selectedSize) {

    alert('Please select size');

    return;
  }

   const cartItem = {

    productId: product.productId,

    name: product.name,

    price: product.price,

    quantity: 1,

    selectedSize: this.selectedSize,

    productImages: product.productImages

  };

  this.cartService.addToCart(cartItem);

  this.router.navigate(['/cart']);
}

}
