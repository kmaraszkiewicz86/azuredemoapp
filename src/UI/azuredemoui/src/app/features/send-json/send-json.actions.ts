import { createAction, props } from '@ngrx/store';

export const sendJsonAction = createAction(
  '[sendJsonAction] Send Json Action',
  props<{ message: string }>()
);

export const sendJsonSuccessAction = createAction(
  '[sendJsonSuccessAction] Send Json Success Action'
);

export const sendJsonFailureAction = createAction(
  '[sendJsonFailureAction] Send Json Failure Action',
  props<{ error: string }>()
);
