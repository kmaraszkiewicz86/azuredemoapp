import { createReducer, on } from '@ngrx/store';
import * as getJsonAction from '../actions/send-json.get.actions';
import { JsonModel } from '../../send-json.models';

export interface GetJsonState {
  jsonDataItems: JsonModel[] | null;
  loading: boolean;
  error: string | null;
}

export const getInitialState: GetJsonState = {
  jsonDataItems: [],
  loading: false,
  error: null
};

export const getJsonReducer = createReducer(
  getInitialState,
  on(getJsonAction.getJsonAction, (state) => ({
    ...state,
    loading: true,
    error: null,
    jsonDataItems: []
  })),
  on(getJsonAction.getJsonSuccessAction, (state, { data }) => ({
    ...state,
    loading: false,
    error: null,
    jsonDataItems: data
  })),
  on(getJsonAction.getJsonFailureAction, (state, { error }) => ({
    ...state,
    loading: false,
    error,
    jsonDataItems: []
  }))
);
