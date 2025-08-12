import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { SendJsonService } from './send-json.service';
import * as SendJsonActions from './send-json.actions';
import { catchError, map, mergeMap, of } from 'rxjs';

@Injectable()
export class SendJsonEffects {
  sendJson$;

  constructor(private actions$: Actions, private service: SendJsonService) {
    this.sendJson$ = createEffect(() =>
    this.actions$.pipe(
      ofType(SendJsonActions.sendJsonAction),
      mergeMap(({ payload }) =>
        this.service.sendJsonData(payload).pipe(
          map(() => SendJsonActions.sendJsonSuccessAction()),
          catchError(error => of(SendJsonActions.sendJsonFailureAction({ error })))
        )
      )
    )
  );
  }
}
