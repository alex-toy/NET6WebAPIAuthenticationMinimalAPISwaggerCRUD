import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestPasswordComponent } from './request-password.component';

describe('RequestPasswordComponent', () => {
  let component: RequestPasswordComponent;
  let fixture: ComponentFixture<RequestPasswordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RequestPasswordComponent]
    });
    fixture = TestBed.createComponent(RequestPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
