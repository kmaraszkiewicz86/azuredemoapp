import { createReducer, on } from '@ngrx/store';
import * as sendJsonAction from './send-json.actions';
import { SendJsonModels } from './send-json.models';

export interface SendJsonState {
  jsonData: SendJsonModels | null;
  loading: boolean;
  error?: string;
};

export const initialState: SendJsonState = {
  jsonData: null,
  loading: false,
  error: undefined
};

export const sendJsonReducer = createReducer(
  initialState,
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
