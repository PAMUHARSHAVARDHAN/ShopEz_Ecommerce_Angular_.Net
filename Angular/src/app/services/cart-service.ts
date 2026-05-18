import { Injectable, signal,effect} from '@angular/core';
import { CartItem } from '../Models/cart-item';

@Injectable({
  providedIn: 'root',
})
export class CartService {
    cartItems = signal<CartItem[]>([]);
     constructor() {
    // Load from localStorage
    const savedCart = localStorage.getItem('cart');
    if (savedCart) {
      this.cartItems.set(JSON.parse(savedCart));
    }

    //  Auto-save whenever cart changes
    effect(() => {
      localStorage.setItem('cart', JSON.stringify(this.cartItems()));
    });
  }

  // ADD TO CART
  addToCart(product: any) {
    const items = this.cartItems();

    const existing = items.find(p => p.productId === product.productId &&
      p.selectedSize === product.selectedSize
    );

    if (existing) {
      existing.quantity += 1;
    } else {
      items.push({
        productId: product.productId,
        name: product.name,
        price: product.price,
        quantity: 1,
          selectedSize: product.selectedSize,
          productImages: product.productImages
        
      });
    }

    this.cartItems.set([...items]);
  }
removeAll(id: number) {
  const updated = this.cartItems().filter(item => item.productId !== id);
  this.cartItems.set(updated);
}
  // REMOVE
  removeItem(id: number) {
   const items = this.cartItems();

  const existing = items.find(p => p.productId === id);

  if (!existing) return;

  if (existing.quantity > 1) {
    existing.quantity -= 1;   // decrease quantity
  } else {
    // remove completely
    const updated = items.filter(p => p.productId !== id);
    this.cartItems.set(updated);
    return;
  }

  this.cartItems.set([...items]); // refresh UI
  }

  // TOTAL
  getTotal() {
    return this.cartItems().reduce(
      (total, item) => total + item.price * item.quantity, 0
    );
  }
  clearCart() {
  this.cartItems.set([]);
}
}
