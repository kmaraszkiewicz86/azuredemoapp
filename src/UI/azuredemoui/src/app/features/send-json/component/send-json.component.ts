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

  sendMessage() {
    this.store.dispatch(sendJsonAction({ message: 'Wiadomość z nadrzędnego komponentu!' }));
  }

  handleNotify(event: SendJsonModels) {
    alert(`Odebrano z dziecka: ${JSON.stringify(event)}`);
  }
}
