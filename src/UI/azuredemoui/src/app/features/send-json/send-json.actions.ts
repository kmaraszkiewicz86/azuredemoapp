import { createAction, props } from '@ngrx/store';
import { SendJsonModels } from './send-json.models';

export const sendJsonAction = createAction(
  '[sendJsonAction] Send Json Action',
  props<{ payload: SendJsonModels }>()
);

export const sendJsonSuccessAction = createAction(
  '[sendJsonSuccessAction] Send Json Success Action'
);

export const sendJsonFailureAction = createAction(
  '[sendJsonFailureAction] Send Json Failure Action',
  props<{ error: string }>()
);
