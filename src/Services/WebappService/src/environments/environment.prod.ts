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
  stockMarketController: '',
  websocketUrl: '',
  bankUrl: `http://bank.stocks`,
  testApiUrl: `http://bank.stocks`,
};
