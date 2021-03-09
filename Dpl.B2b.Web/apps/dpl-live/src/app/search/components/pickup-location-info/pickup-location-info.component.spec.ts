/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PickupLocationInfoComponent } from './pickup-location-info.component';

describe('PickupLocationInfoComponent', () => {
  let component: PickupLocationInfoComponent;
  let fixture: ComponentFixture<PickupLocationInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PickupLocationInfoComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PickupLocationInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
