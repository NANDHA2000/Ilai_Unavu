import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanySideComponent } from './company-side.component';

describe('CompanySideComponent', () => {
  let component: CompanySideComponent;
  let fixture: ComponentFixture<CompanySideComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanySideComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanySideComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
