import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BankRegisterComponent } from './bank-register.component';

describe('BankRegisterComponent', () => {
  let component: BankRegisterComponent;
  let fixture: ComponentFixture<BankRegisterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BankRegisterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BankRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
