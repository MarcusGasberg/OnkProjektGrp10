export const environment = {
  production: true,
  authority: `${window.location.origin}/account-api`,
  clientId: 'angularClient',
  redirectUri: window.location.origin,
  responseType: 'id_token token',
  scope:
    'openid profile taxingController paymentController stockMarketController api1 bankController',
  taxingControllerUrl: '',
  paymentControllerUrl: '',
  stockMarketController: '',
  websocketUrl: 'ws://stockmarket:5010',
  bankUrl: `${window.location.origin}/bank-api`,
  testApiUrl: `${window.location.origin}/bank-api`,
};
