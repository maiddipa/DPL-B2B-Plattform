import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VoucherFilterTemplateComponent } from './voucher-filter-template.component';

describe('VoucherFilterTemplateComponent', () => {
  let component: VoucherFilterTemplateComponent;
  let fixture: ComponentFixture<VoucherFilterTemplateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VoucherFilterTemplateComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VoucherFilterTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
