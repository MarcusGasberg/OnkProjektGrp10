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
  stockMarketController: 'stockmarket',
  websocketUrl: `stockmarketws`,
  bankUrl: `bank-api`,
  testApiUrl: `bank-api`,
};
