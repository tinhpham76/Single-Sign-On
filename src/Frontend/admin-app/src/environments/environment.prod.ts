////// Linux container https//////
//  export const environment = {
//    production: false,
//    apiUrl: 'https://localhost:5000',
//    authority: 'https://localhost:5000',
//    client_id: 'angular_admin',
//    redirect_uri: 'http://localhost:4200/auth-callback',
//    post_logout_redirect_uri: 'http://localhost:4200/',
//    scope: 'SSO_API openid profile',
//    silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
//  };
////// Linux container http//////
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5001',
  authority: 'http://localhost:5001',
  client_id: 'angular_admin',
  redirect_uri: 'http://localhost:4200/auth-callback',
  post_logout_redirect_uri: 'http://localhost:4200/',
  scope: 'SSO_API openid profile',
  silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
};
////// Linux host 172.16.80.54 linux container http//////
// export const environment = {
//   production: false,
//   apiUrl: 'http://172.16.80.54:5001',
//   authority: 'http://172.16.80.54:5001',
//   client_id: 'angular_admin',
//   redirect_uri: 'http://localhost:4200/auth-callback',
//   post_logout_redirect_uri: 'http://localhost:4200/',
//   scope: 'SSO_API openid profile',
//   silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
// };
////// Linux host 172.16.80.54 linux container https//////
// export const environment = {
//   production: false,
//   apiUrl: 'https://172.16.80.54:5000',
//   authority: 'https://172.16.80.54:5000',
//   client_id: 'angular_admin',
//   redirect_uri: 'http://localhost:4200/auth-callback',
//   post_logout_redirect_uri: 'http://localhost:4200/',
//   scope: 'SSO_API openid profile',
//   silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
// };