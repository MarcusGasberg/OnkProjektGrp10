// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  authority: `http://localhost:5000`,
  clientId: 'angularDebugClient',
  redirectUri: window.location.origin,
  responseType: 'id_token token',
  scope:
    'openid profile api1 taxingController paymentController stockMarketController bankController',
  taxingControllerUrl: '',
  paymentControllerUrl: '',
  stockMarketController: 'http://localhost:5010/stockmarket',
  stockBrokerController: 'http://localhost:5010/stockbroker',
  websocketUrl: 'ws://localhost:5010',
  bankUrl: 'http://localhost:5004',
  testApiUrl: 'http://localhost:5004',
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
