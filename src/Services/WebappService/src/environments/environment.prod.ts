export const environment = {
  production: true,
  authority: 'http://localhost:5000',
  clientId: 'angularClient',
  redirectUri: 'http://localhost:4200',
  responseType: 'id_token token',
  scope:
    'openid profile taxingController paymentController stockMarketController api1',
  taxingControllerUrl: '',
  paymentControllerUrl: '',
  stockMarketController: '',
};
