import { ProductImage } from "./product-image";

export interface CartItem {
     productId: number;
  name: string;
  price: number;
  quantity: number;
   selectedSize: string;
   productImages: ProductImage[];
}
