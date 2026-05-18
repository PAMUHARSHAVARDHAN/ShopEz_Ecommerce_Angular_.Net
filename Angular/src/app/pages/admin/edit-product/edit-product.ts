import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../services/product-service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-edit-product',
  imports: [CommonModule,FormsModule,RouterModule],
  templateUrl: './edit-product.html',
  styleUrl: './edit-product.css',
})
export class EditProduct implements OnInit {
   product: any = {
    productImages: [],
    productSizes: []
  };

  // Multiple files
  selectedFiles: File[] = [];

  // Preview images
  previewUrls: string[] = [];

  id!: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService
  ) {}

  ngOnInit(): void {

    this.id = this.route.snapshot.params['id'];

    this.productService
      .getProductById(this.id)
      .subscribe((res: any) => {

        this.product = res;

      });

  }

  // Select multiple images
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

  // Add size
  addSize(): void {

    this.product.productSizes.push({
      sizeValue: '',
      stock: 0
    });

  }

  // Remove size
  removeSize(index: number): void {

    this.product.productSizes.splice(index, 1);

  }

  // Upload images
  uploadImages() {

    if (this.selectedFiles.length === 0) {

      return null;

    }

    const formData = new FormData();

    this.selectedFiles.forEach((file: File) => {

      formData.append('files', file);

    });

    return this.productService.uploadImages(formData);

  }

  // Update product
  updateProduct(): void {

    // upload new images first
    if (this.selectedFiles.length > 0) {

      this.uploadImages()?.subscribe((res: any) => {

        // replace old images
        this.product.productImages =
          res.fileNames.map((img: string) => ({
            imageUrl: img
          }));

        this.saveProduct();

      });

    }
    else {

      this.saveProduct();

    }

  }

  // Save product
  saveProduct(): void {

    this.productService
      .updateProduct(this.id, this.product)
      .subscribe(() => {

        this.productService.clearCache();

        alert('Product updated successfully!');

        this.router.navigate(['/admin/products']);

      });

  }
}
