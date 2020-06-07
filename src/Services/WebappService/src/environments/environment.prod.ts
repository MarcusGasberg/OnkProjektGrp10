export const environment = {
  production: true,
  authority: `https://identity-server.stocks`,
  clientId: 'angularClient',
  redirectUri: window.location.origin,
  responseType: 'id_token token',
  scope:
    'openid profile taxingController paymentController stockMarketController bankController',
  taxingControllerUrl: '',
  paymentControllerUrl: '',
  stockMarketController: 'stockmarket',
  stockBrokerController: 'stockbroker',
  websocketUrl: `ws://${window.location.hostname}/stockmarketws`,
  bankUrl: `bank-api`,
  testApiUrl: `bank-api`,
};
