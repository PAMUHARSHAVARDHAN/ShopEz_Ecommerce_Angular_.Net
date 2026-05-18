import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../Models/product';
import { Observable ,shareReplay} from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl= `${environment.apiGateway}/Product`;
    private products$!: Observable<Product[]>;
  constructor( private httpClient:HttpClient){}
 getProducts(): Observable<Product[]> {
     if (!this.products$) {
      this.products$ = this.httpClient.get<Product[]>(this.apiUrl).pipe(
        shareReplay(1)
      );
    }
    return this.products$;
  }

  // Add this — call after create/update/delete
  clearCache() {
    this.products$ = null!;
  }

  // Get by ID
  getProductById(id: number): Observable<Product> {
    return this.httpClient.get<Product>(`${this.apiUrl}/${id}`);
  }

  // Create product
  createProduct(product: any): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}`, product);
  }

  // Update product
  updateProduct(id: number, data: any): Observable<any> {
    return this.httpClient.put(`${this.apiUrl}/${id}`, data);
  }

  // Upload image
  uploadImage(formData: FormData): Observable<any> {
    return this.httpClient.post<any>(
      `${this.apiUrl}/upload`,
      formData
    );
  }

  // Delete product
  deleteProduct(id: number): Observable<any> {
    return this.httpClient.delete(`${this.apiUrl}/${id}`);
  }
  getAdminProducts() {
  return this.httpClient.get<any[]>(
    `${this.apiUrl}/admin`
  );
 }
 uploadImages(formData: FormData): Observable<any> {

  return this.httpClient.post<any>(
    `${this.apiUrl}/upload-multiple`,
    formData
  );

}
}
