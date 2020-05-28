export const environment = {
  production: true,
  authority: `${window.location.origin}/account-api`,
  clientId: 'angularDockerClient',
  redirectUri: window.location.origin,
  responseType: 'id_token token',
  scope:
    'openid profile taxingController paymentController stockMarketController api1',
  taxingControllerUrl: '',
  paymentControllerUrl: '',
  stockMarketController: '',
  bankUrl: `${window.location.origin}/bank-api`,
  testApiUrl: `${window.location.origin}/bank-api`,
};
