import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendJsonComponent } from './send-json.component';

describe('SendJsonComponent', () => {
  let component: SendJsonComponent;
  let fixture: ComponentFixture<SendJsonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SendJsonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SendJsonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
