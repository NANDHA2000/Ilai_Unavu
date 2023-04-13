import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageWeekdaysComponent } from './manage-weekdays.component';

describe('ManageWeekdaysComponent', () => {
  let component: ManageWeekdaysComponent;
  let fixture: ComponentFixture<ManageWeekdaysComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageWeekdaysComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageWeekdaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
