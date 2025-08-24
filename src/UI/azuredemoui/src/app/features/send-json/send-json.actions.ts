import { createAction, props } from '@ngrx/store';
import { JsonModel } from './send-json.models';

//send json actions
export const sendJsonAction = createAction(
  '[sendJsonAction] Send Json Action',
  props<{ payload: JsonModel }>()
);

export const sendJsonSuccessAction = createAction(
  '[sendJsonSuccessAction] Send Json Success Action'
);

export const sendJsonFailureAction = createAction(
  '[sendJsonFailureAction] Send Json Failure Action',
  props<{ error: string }>()
);

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
