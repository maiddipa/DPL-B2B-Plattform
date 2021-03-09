import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSelectorArticlesComponent } from './account-selector-articles.component';

describe('AccountSelectorArticlesComponent', () => {
  let component: AccountSelectorArticlesComponent;
  let fixture: ComponentFixture<AccountSelectorArticlesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AccountSelectorArticlesComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSelectorArticlesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
