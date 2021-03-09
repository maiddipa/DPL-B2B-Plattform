import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VoucherFilterTemplateSaveDialogComponent } from './voucher-filter-template-save-dialog.component';

describe('VoucherFilterTemplateSaveDialogComponent', () => {
  let component: VoucherFilterTemplateSaveDialogComponent;
  let fixture: ComponentFixture<VoucherFilterTemplateSaveDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VoucherFilterTemplateSaveDialogComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VoucherFilterTemplateSaveDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
