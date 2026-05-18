import {  Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product-service';
import { CommonModule } from '@angular/common';

import { Product } from '../../Models/product';
import { Router, RouterModule } from '@angular/router';
import { CartService } from '../../services/cart-service';
import{ActivatedRoute} from '@angular/router';
@Component({
  selector: 'app-products',
  standalone:true,
  imports: [CommonModule,RouterModule],
  templateUrl: './products.html',
  styleUrls: ['./products.css'],
})
export class Products implements OnInit {
  
  products: Product[] = [];
  filteredProducts: Product[] = [];

  categories: string[] = [];

  selectedCategory: string = '';
  selectedGender: string = '';;
  

  constructor(private productService:ProductService, 
    private cartService:CartService,
  private router:Router,
  private route:ActivatedRoute){}

ngOnInit(): void {
  this.productService.getProducts()
      .subscribe({
        next: (data) => {

          this.products = data;
          this.filteredProducts = data;

          // Get unique categories
          this.categories = [
            ...new Set(data.map(x => x.categoryName))
          ] as string[];
            this.route.queryParams.subscribe(params => {

            const search = params['search'];

            if (search) {

              this.filteredProducts = this.products.filter(product =>
                product.name
                  .toLowerCase()
                  .includes(search.toLowerCase())
              );

            }
            else {

              this.filteredProducts = this.products;

            }

          });

        },

        error: (err) => {
          console.log(err);
        }

        
      });

  }

  // FILTER PRODUCTS
  filterProducts() {
 this.filteredProducts = this.products.filter(product => {

    const categoryMatch =
      this.selectedCategory === '' ||
      product.categoryName === this.selectedCategory;

    const genderMatch =
      this.selectedGender === '' ||
      product.gender === this.selectedGender;

    return categoryMatch && genderMatch;

  });

  }

  // CATEGORY FILTER
  selectCategory(category: string) {

    this.selectedCategory = category;

    this.filterProducts();
  }

  // GENDER FILTER
  selectGender(gender: string) {

    this.selectedGender = gender;

    this.filterProducts();
  }

  // CLEAR FILTERS
  clearFilters() {

    this.selectedCategory = '';
    this.selectedGender = '';

    this.filteredProducts = this.products;
  }

  viewProduct(id: number) {

    this.router.navigate(['/product', id]);
  }

  addToCart(product: any) {

    const cartItem = {

    productId: product.productId,

    name: product.name,

    price: product.price,

    quantity: 1,

    productImages: product.productImages

  };

  this.cartService.addToCart(cartItem);

  }
}
