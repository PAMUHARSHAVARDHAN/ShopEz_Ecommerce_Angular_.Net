import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Checkout } from './checkout';
import { of } from 'rxjs';

describe('Checkout', () => {
  let component: Checkout;
  let fixture: ComponentFixture<Checkout>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Checkout],
    }).compileComponents();

    fixture = TestBed.createComponent(Checkout);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
    it('should have cart items array', () => {
    expect(component.cartItems).toBeDefined();
  });

  it('should have total amount', () => {
    expect(component.totalAmount).toBeDefined();
  });

  it('should have user object', () => {
    expect(component.user).toBeDefined();
  });

  it('should place order', () => {

    const orderSpy = spyOn(
      (component as any).orderService,
      'placeOrder'
    ).and.returnValue(of({}));

    component.placeOrder();

    expect(orderSpy).toHaveBeenCalled();

  });

});
