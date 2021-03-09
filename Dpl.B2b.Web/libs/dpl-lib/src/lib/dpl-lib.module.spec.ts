import { async, TestBed } from '@angular/core/testing';
import { DplLibModule } from './dpl-lib.module';

describe('DplLibModule', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [DplLibModule],
    }).compileComponents();
  }));

  it('should create', () => {
    expect(DplLibModule).toBeDefined();
  });
});
