export const environment = {
  production: true,
  authority: `${window.location.origin}/account-service`,
  clientId: 'angularDockerClient',
  redirectUri: window.location.origin,
  responseType: 'id_token token',
  scope:
    'openid profile taxingController paymentController stockMarketController api1',
  taxingControllerUrl: '',
  paymentControllerUrl: '',
  stockMarketController: '',
  testApiUrl: 'http://192.168.99.100:5001',
};
