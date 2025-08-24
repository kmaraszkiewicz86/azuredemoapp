import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AppState } from '../../../app-state';
import { sendJsonAction } from '../send-json.actions';
import { JsonModel } from '../send-json.models';
import { SendJsonViewComponent } from './form/send-json-view.component';

@Component({
  selector: 'app-send-json',
  standalone: true,
  imports: [
    CommonModule,
    SendJsonViewComponent
  ],
  templateUrl: './send-json.component.html',
  styleUrl: './send-json.component.scss'
})
export class SendJsonComponent {
  loading$: Observable<boolean>;
  error$: Observable<string | undefined>;

  constructor(private store: Store<AppState>) {
  this.loading$ = this.store.select((state: AppState) => state.loading);
  this.error$ = this.store.select((state: AppState) => state.error);
  }

  handleNotify(event: JsonModel) {
    this.store.dispatch(sendJsonAction({ payload: event }));
  }
}
