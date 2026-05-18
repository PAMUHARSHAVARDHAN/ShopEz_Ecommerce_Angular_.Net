import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';

import { Products } from './products';

describe('Products', () => {
  let component: Products;
  let fixture: ComponentFixture<Products>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Products],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(Products);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  // INSERT BELOW HERE

  it('should have products array', () => {
    expect(component.products).toBeDefined();
  });

  it('should clear filters', () => {

    component.selectedCategory = 'Men';
    component.selectedGender = 'Male';

    component.clearFilters();

    expect(component.selectedCategory).toBe('');
    expect(component.selectedGender).toBe('');

  });

  it('should select category', () => {

    component.selectCategory('Shirts');

    expect(component.selectedCategory).toBe('Shirts');

  });

  it('should select gender', () => {

    component.selectGender('Male');

    expect(component.selectedGender).toBe('Male');

  });
});
