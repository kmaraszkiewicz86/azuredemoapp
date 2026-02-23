import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendJsonViewComponent } from './send-json-view.component';

describe('SendJsonViewComponent', () => {
  let component: SendJsonViewComponent;
  let fixture: ComponentFixture<SendJsonViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SendJsonViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SendJsonViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
