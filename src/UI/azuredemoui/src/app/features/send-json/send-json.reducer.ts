import { createReducer, on } from '@ngrx/store';
import * as sendJsonAction from './send-json.actions';
import { JsonModel } from './send-json.models';

interface JsonState {
  loading: boolean;
  error?: string;
}

export interface SendJsonState extends JsonState {
  jsonData: JsonModel | null;
};

export interface GetJsonState extends JsonState {
  jsonDataItems: JsonModel[] | null;
};

export const sendInitialState: SendJsonState = {
  jsonData: null,
  loading: false,
  error: undefined
};

export const getInitialState: GetJsonState = {
  jsonDataItems: [],
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

export const getJsonReducer = createReducer(
  getInitialState,
  on(sendJsonAction.getJsonAction, (state) => ({
    ...state,
    loading: true,
    error: undefined,
  })),
  on(sendJsonAction.getJsonSuccessAction, (state, { data }) => ({
    ...state,
    loading: false,
    jsonData: data,
  })),
  on(sendJsonAction.getJsonFailureAction, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  }))
);
