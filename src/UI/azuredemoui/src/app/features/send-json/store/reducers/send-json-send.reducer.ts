import { createReducer, on } from '@ngrx/store';
import * as sendJsonAction from '../actions/send-json.send.actions';
import { JsonModel } from '../../send-json.models';

export interface SendJsonState {
  jsonData: JsonModel | null;
  loading: boolean;
  error?: string;
}

export const sendInitialState: SendJsonState = {
  jsonData: null,
  loading: false,
  error: undefined
};

export const sendJsonReducer = createReducer(
  sendInitialState,
  on(sendJsonAction.sendJsonAction, (state) => ({
    ...state,
    loading: true,
    error: undefined,
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
