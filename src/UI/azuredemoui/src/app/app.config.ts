import { ApplicationConfig, provideZoneChangeDetection, isDevMode } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AzureFunctionTokenInterceptor } from './interceptors/azure-function-token-interceptor';
import { combineReducers } from '@ngrx/store';
import { sendJsonFeatureReducers } from './features/send-json/store/send-json.reducer';
import { SendJsonEffects } from './features/send-json/store/effects/send-json.send.effects';
import { GetJsonEffects } from './features/send-json/store/effects/send-json.get.effects';
import { GlobalErrorInterceptor } from './interceptors/global-error-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideStore({
      sendJson: combineReducers(sendJsonFeatureReducers)
    }),
    provideEffects(SendJsonEffects, GetJsonEffects),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AzureFunctionTokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: GlobalErrorInterceptor,
      multi: true
    }
  ]
};
