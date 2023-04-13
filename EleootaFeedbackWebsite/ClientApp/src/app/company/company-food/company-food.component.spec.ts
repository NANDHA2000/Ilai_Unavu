import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyFoodComponent } from './company-food.component';

describe('CompanyFoodComponent', () => {
  let component: CompanyFoodComponent;
  let fixture: ComponentFixture<CompanyFoodComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyFoodComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyFoodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
