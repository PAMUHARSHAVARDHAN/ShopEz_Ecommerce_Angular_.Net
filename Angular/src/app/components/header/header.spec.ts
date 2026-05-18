import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Header } from './header';

describe('Header', () => {
  let component: Header;
  let fixture: ComponentFixture<Header>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Header],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(Header);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should navigate to home page', () => {

  const routerSpy = spyOn(
    (component as any).router,
    'navigate'
  );

  (component as any).router.navigate(['/']);

  expect(routerSpy).toHaveBeenCalledWith(['/']);

});
it('should navigate to products page', () => {

  const routerSpy = spyOn(
    (component as any).router,
    'navigate'
  );

  (component as any).router.navigate(['/products']);

  expect(routerSpy).toHaveBeenCalledWith(['/products']);

});
it('should navigate to cart page', () => {

  const routerSpy = spyOn(
    (component as any).router,
    'navigate'
  );

  (component as any).router.navigate(['/cart']);

  expect(routerSpy).toHaveBeenCalledWith(['/cart']);

});
});
