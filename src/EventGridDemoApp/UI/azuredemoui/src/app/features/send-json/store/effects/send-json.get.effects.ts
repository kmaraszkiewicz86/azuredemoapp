import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { SendJsonService } from '../../send-json.service';
import * as GetJsonActions from '../actions/send-json.get.actions';
import { catchError, map, mergeMap, of } from 'rxjs';

@Injectable()
export class GetJsonEffects {
  getJson$;

  constructor(private actions$: Actions, private service: SendJsonService) {
    this.getJson$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GetJsonActions.getJsonAction),
      mergeMap(() =>
        this.service.getJsonData().pipe(
          map(data => GetJsonActions.getJsonSuccessAction({ data })),
          catchError(error => of(GetJsonActions.getJsonFailureAction({ error })))
        )
      )
    )
  );
  }
}
