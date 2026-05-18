import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Order } from '../Models/order';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class OrderService {
   private apiUrl = `${environment.apiGateway}/Order`;

  constructor(private http: HttpClient) {}
    //  Place Order
  placeOrder(order: any): Observable<any> {
    return this.http.post(this.apiUrl, order);
  }
// Get all orders (for next step)
  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl);
  }
   //  Get order by id
  getOrderById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }
  getMyOrders() {
  return this.http.get<any[]>('https://localhost:7151/api/Order/my-orders');
}
}
