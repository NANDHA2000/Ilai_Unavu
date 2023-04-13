import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageFoodmenuComponent } from './manage-foodmenu.component';

describe('ManageFoodmenuComponent', () => {
  let component: ManageFoodmenuComponent;
  let fixture: ComponentFixture<ManageFoodmenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageFoodmenuComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageFoodmenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
