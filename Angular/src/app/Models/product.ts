import { ProductImage } from './product-image';
import { ProductSize } from './product-size';
export interface Product {
  productId: number;   // comes from backend GET
  name: string;
  price: number;
  description: string;
  stock: number;
  isActive: boolean;
  categoryId: number;
  categoryName: string;
  gender: string;
  productImages: ProductImage[];
   productSizes: ProductSize[];
}
