import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DateFromToDialogComponent } from './date-from-to-dialog.component';

describe('DateFromToDialogComponent', () => {
  let component: DateFromToDialogComponent;
  let fixture: ComponentFixture<DateFromToDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DateFromToDialogComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DateFromToDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
