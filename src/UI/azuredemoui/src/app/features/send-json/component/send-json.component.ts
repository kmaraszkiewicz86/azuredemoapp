import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/internal/Observable';
import { AppState } from '../../../app-state';
import { sendJsonAction } from '../send-json.actions';
import { SendJsonModels } from '../send-json.models';

@Component({
  selector: 'app-send-json',
  imports: [],
  templateUrl: './send-json.component.html',
  styleUrl: './send-json.component.scss'
})
export class SendJsonComponent {
  message$: Observable<SendJsonModels>;
  childData = 'Wiadomość do dziecka';

  constructor(private store: Store<AppState>) {
    this.message$ = this.store.select({
      jsonData: (state: AppState) => state.jsonData,
      loading: (state: AppState) => state.loading,
      error: (state: AppState) => state.error
    });
  }

  handleNotify(event: SendJsonModels) {
    alert(`Odebrano z dziecka: ${JSON.stringify(event)}`);

    this.store.dispatch(sendJsonAction({ payload: event }));
  }
}
