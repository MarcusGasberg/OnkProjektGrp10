export const environment = {
  production: true,
  authority: `http://identity-server.stocks`,
  clientId: 'angularClient',
  redirectUri: window.location.origin,
  responseType: 'id_token token',
  scope:
    'openid profile taxingController paymentController stockMarketController bankController',
  taxingControllerUrl: '',
  paymentControllerUrl: '',
  stockMarketController: '',
  websocketUrl: 'ws://stockmarket',
  bankUrl: `http://bank-api`,
  testApiUrl: `http://bank-api`,
};
