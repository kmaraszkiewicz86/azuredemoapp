import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendJsonListViewComponent } from './send-json-list-view.component';

describe('SendJsonListViewComponent', () => {
  let component: SendJsonListViewComponent;
  let fixture: ComponentFixture<SendJsonListViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SendJsonListViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SendJsonListViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
