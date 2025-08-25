import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AppState } from '../../../app-state';
import { JsonModel } from '../send-json.models';
import { SendJsonViewComponent } from './form/send-json-view.component';
import { getJsonAction } from '../store/actions/send-json.get.actions';
import { sendJsonAction } from '../store/actions/send-json.send.actions';
import { SendJsonListViewComponent } from './list/send-json-list-view.component';

@Component({
  selector: 'app-send-json',
  standalone: true,
  imports: [CommonModule, SendJsonViewComponent, SendJsonListViewComponent],
  templateUrl: './send-json.component.html',
  styleUrl: './send-json.component.scss',
})
export class SendJsonComponent {
  items$: Observable<JsonModel[] | null>;

  formLoading$: Observable<boolean>;
  listLoading$: Observable<boolean>;
  formError$: Observable<string | undefined>;
  listError$: Observable<string | undefined>;

  constructor(private store: Store<AppState>) {
    this.formLoading$ = this.store.select(
      (state: AppState) => state.sendJson.send.loading
    );
    this.formError$ = this.store.select(
      (state: AppState) => state.sendJson.send.error
    );

    this.listLoading$ = this.store.select(
      (state: AppState) => state.sendJson.get.loading
    );
    this.listError$ = this.store.select(
      (state: AppState) => state.sendJson.get.error
    );
    this.items$ = this.store.select(
      (state: AppState) => state.sendJson.get.jsonDataItems
    );
  }

  handleNotify(event: JsonModel) {
    this.store.dispatch(sendJsonAction({ payload: event }));
    this.store.dispatch(getJsonAction());
  }
}
