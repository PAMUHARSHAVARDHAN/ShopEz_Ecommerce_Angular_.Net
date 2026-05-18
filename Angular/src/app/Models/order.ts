export interface OrderItem {
  orderItemId: number;
  productId: number;
  quantity: number;
  price: number;
}

export interface Order {
  orderId: number;
  userId: number;
  orderDate: string;
  totalAmount: number;
  orderItems: OrderItem[];
}
