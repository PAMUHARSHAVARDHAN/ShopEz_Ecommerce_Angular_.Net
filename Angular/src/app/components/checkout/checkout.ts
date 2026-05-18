import { Component } from '@angular/core';
import { CartService } from '../../services/cart-service';
import { OrderService } from '../../services/order-service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './checkout.html',
  styleUrls: ['./checkout.css'],
})
export class Checkout {
    cartItems: any[] = [];
  totalAmount = 0;

  user: any = {
    fullName: '',
    email: '',
    address: ''
  };

  constructor(
    private cartService: CartService,
    private orderService: OrderService,
    private router: Router
  ) {}

  ngOnInit() {
    this.cartItems = this.cartService.cartItems();
    this.totalAmount = this.cartService.getTotal();

    const userData = localStorage.getItem('user');
    if (userData) {
      const u = JSON.parse(userData);
      this.user.fullName = u.fullName;
      this.user.email = u.email;
    }
  }

  placeOrder() {
      const userData = JSON.parse(localStorage.getItem('user') || '{}');

    const order = {
      userId: userData.userId, //  later get from JWT
      fullName: this.user.fullName,
      email: this.user.email,
      address: this.user.address,
      totalAmount: this.totalAmount,
      items: this.cartItems.map(item => ({
        productId: item.productId,
  productName: item.name,
  productImage: item.imageUrl,
  selectedSize: item.selectedSize,
  quantity: item.quantity,
  price: item.price
      }))
    };
    console.log('ORDER:', order);

    this.orderService.placeOrder(order).subscribe({
      next: () => {
        alert('Order placed successfully 🎉');
        this.cartService.clearCart();
        this.router.navigate(['/']);
      },
      error: (err) => {
        console.error(err)
        alert( err.error?.message || 'Order failed ');
      }
    });
  }
}

