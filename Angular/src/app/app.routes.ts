import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Products } from './components/products/products';
import { Component } from '@angular/core';
import { ProductDetails } from './components/product-details/product-details';
import { Cart } from './components/cart/cart';
import { AuthComponent } from './pages/auth-component/auth-component';
import { authGuard } from './guards/auth-guard';
import { Profile } from './pages/profile/profile';
import { Checkout } from './components/checkout/checkout';
import { Orders } from './components/orders/orders';
import { ProductList } from './pages/admin/product-list/product-list';
import { AddProduct } from './pages/admin/add-product/add-product';
import { EditProduct } from './pages/admin/edit-product/edit-product';

export const routes: Routes = [
    {path:'auth', component:AuthComponent},
    {path:'',component:Home,canActivate: [authGuard]},
    {path:'products',component:Products},
    {path:'product/:id',component:ProductDetails},
    {path:'cart',component:Cart},
    {path:'profile',component:Profile},
    {path:'orders',component:Orders},
     {path:'checkout', component:Checkout},
     {path:'admin/products',component:ProductList},
     {path:'admin/add-product',component:AddProduct},
     {path:'admin/edit-product/:id',component:EditProduct},
     { path: '**', redirectTo: '' }
    
     

];
