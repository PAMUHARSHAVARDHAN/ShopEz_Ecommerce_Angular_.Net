import { Component, OnInit ,ChangeDetectorRef,NgZone} from '@angular/core';
import { ProductService } from '../../../services/product-service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit{
  products: any[] = [];
   constructor(private productService: ProductService, 
    private ngZone: NgZone,
     private cdr: ChangeDetectorRef) {}

ngOnInit() {
  this.loadProducts();
}
loadProducts() {
    this.productService.getAdminProducts().subscribe({
      next: (res) => {
         this.ngZone.run(() => {          // ← WRAP in ngZone.run
          this.products = [...res];      // ← spread to create new reference
          this.cdr.detectChanges();      // ← force Angular to re-render
        });
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  deleteProduct(id: number) {

  if (!confirm('Are you sure to delete?')) {
    return;
  }

  this.productService.deleteProduct(id).subscribe({

    next: () => {

      this.productService.clearCache();

      // remove from UI immediately
      this.products = this.products.filter(
        (p: any) => p.productId !== id
      );

      alert('Deleted successfully');

    },

    error: (err) => {

      console.log(err);

      alert(
        'Cannot delete product because it exists in orders'
      );

    }

  });

}



}
