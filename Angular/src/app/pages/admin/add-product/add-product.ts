import { Component } from '@angular/core';
import { ProductService } from '../../../services/product-service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-add-product',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './add-product.html',
  styleUrl: './add-product.css',
})
export class AddProduct {
 
  product: any = {
    name: '',
    price: 0,
    description: '',
    stock: 0,
    categoryId: 0,
    gender: '',

    productImages: [],

    productSizes: []
  };

  // Multiple files
  selectedFiles: File[] = [];

  // Preview images
  previewUrls: string[] = [];

  // Sizes
  sizes: any[] = [
    {
      sizeValue: '',
      stock: 0
    }
  ];

  constructor(
    private productService: ProductService,
    private router: Router
  ) {}

  // Select Images
  onFilesSelected(event: any): void {

    this.selectedFiles = Array.from(event.target.files);

    this.previewUrls = [];

    this.selectedFiles.forEach((file: File) => {

      const reader = new FileReader();

      reader.onload = (e: any) => {

        this.previewUrls.push(e.target.result);

      };

      reader.readAsDataURL(file);

    });

  }

  // Add Size
  addSize(): void {

    this.sizes.push({
      sizeValue: '',
      stock: 0
    });

  }

  // Remove Size
  removeSize(index: number): void {

    this.sizes.splice(index, 1);

  }

  // Create Product
  createProduct(): void {

    // Upload images first
    if (this.selectedFiles.length > 0) {

      const formData = new FormData();

      // append files
      this.selectedFiles.forEach((file: File) => {

        formData.append('files', file);

      });

      // REQUIRED for backend
      formData.append(
        'category',
        this.product.gender
      );

      formData.append(
        'productFolder',
        this.product.name.replace(/\s+/g, '-')
      );

      // upload images
      this.productService
        .uploadImages(formData)
        .subscribe((res: any) => {

          // backend response
          this.product.productImages =
            res.productImages;

          // sizes
          this.product.productSizes =
            this.sizes;

          // save product
          this.saveProduct();

        });

    }
    else {

      this.product.productImages = [];

      this.product.productSizes =
        this.sizes;

      this.saveProduct();

    }

  }

  // Save Product
  saveProduct(): void {

    this.productService
      .createProduct(this.product)
      .subscribe(() => {

        this.productService.clearCache();

        alert('Product Created Successfully');

        this.router.navigate(['/admin/products']);

      });

  }
}
