import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSummaryRowComponent } from './account-summary-row.component';

describe('AccountSummaryRowComponent', () => {
  let component: AccountSummaryRowComponent;
  let fixture: ComponentFixture<AccountSummaryRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AccountSummaryRowComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSummaryRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
