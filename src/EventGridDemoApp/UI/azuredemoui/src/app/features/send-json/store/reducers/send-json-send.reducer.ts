import { createReducer, on } from '@ngrx/store';
import * as sendJsonAction from '../actions/send-json.send.actions';
import { JsonModel } from '../../send-json.models';

export interface SendJsonState {
  jsonData: JsonModel | null;
  loading: boolean;
  error: string | null;
}

export const sendInitialState: SendJsonState = {
  jsonData: null,
  loading: false,
  error: null
};

export const sendJsonReducer = createReducer(
  sendInitialState,
  on(sendJsonAction.sendJsonAction, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(sendJsonAction.sendJsonSuccessAction, (state) => ({
    ...state,
    loading: false,
  })),
  on(sendJsonAction.sendJsonFailureAction, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  }))
);
