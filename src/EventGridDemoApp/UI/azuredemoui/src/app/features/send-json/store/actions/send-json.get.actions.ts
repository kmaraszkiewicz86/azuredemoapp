import { createAction, props } from '@ngrx/store';
import { JsonModel } from '../../send-json.models';

//get json actions
export const getJsonAction = createAction(
  '[getJsonAction] Get Json Action'
);

export const getJsonSuccessAction = createAction(
  '[getJsonSuccessAction] Get Json Success Action',
  props<{ data: JsonModel[] }>()
);

export const getJsonFailureAction = createAction(
  '[getJsonFailureAction] Get Json Failure Action',
  props<{ error: string }>()
);
