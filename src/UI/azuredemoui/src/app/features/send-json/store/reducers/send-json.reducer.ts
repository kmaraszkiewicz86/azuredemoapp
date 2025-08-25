import { ActionReducerMap } from '@ngrx/store';
import { SendJsonState, sendJsonReducer } from './send-json-send.reducer';
import { GetJsonState, getJsonReducer } from './send-json-get.reducer';

export interface SendJsonFeatureState {
  send: SendJsonState;
  get: GetJsonState;
}

export const sendJsonFeatureReducers: ActionReducerMap<SendJsonFeatureState> = {
  send: sendJsonReducer,
  get: getJsonReducer,
};
