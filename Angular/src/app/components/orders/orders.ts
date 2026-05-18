import { Component ,OnInit,NgZone,ChangeDetectorRef} from '@angular/core';
import { OrderService } from '../../services/order-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-orders',
  imports: [CommonModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
})
export class Orders implements OnInit {
    orders: any[] = [];
  constructor(private orderService: OrderService,
    private ngZone: NgZone,
    private cdr: ChangeDetectorRef) {}
  
  ngOnInit() {
    this.orderService.getMyOrders().subscribe({
      next: (data) => {
         this.ngZone.run(() => {       // WRAP in ngZone.run
          this.orders = [...data];    //  spread to new reference
          this.cdr.detectChanges();   //  force re-render
        });
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
