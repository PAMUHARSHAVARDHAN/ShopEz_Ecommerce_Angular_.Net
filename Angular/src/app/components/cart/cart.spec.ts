import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Cart } from './cart';
import{vi} from 'vitest';

describe('Cart', () => {
  let component: Cart;
  let fixture: ComponentFixture<Cart>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Cart],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(Cart);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
   it('should have cart service', () => {
    expect(component.cartService).toBeTruthy();
  });

  it('should navigate to checkout', () => {

    const routerSpy = vi.spyOn(
      (component as any).router,
      'navigate'
    );

    component.goCheckout();

    expect(routerSpy).toHaveBeenCalledWith(['/checkout']);

  });
});
