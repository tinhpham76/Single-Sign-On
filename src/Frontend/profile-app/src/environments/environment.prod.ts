////// Linux container https//////
// export const environment = {
//   production: false,
//   apiUrl: 'https://localhost:5002',
//   authority: 'https://localhost:5000',
//   client_id: 'angular_user_manager',
//   redirect_uri: 'http://localhost:4300/auth-callback',
//   post_logout_redirect_uri: 'http://localhost:4300/',
//   scope: 'USER_API SSO_API openid profile',
//   silent_redirect_uri: 'http://localhost:4300/silent-refresh.html'
// };
////// Linux container http//////
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5003',
  authority: 'http://localhost:5001',
  client_id: 'angular_user_manager',
  redirect_uri: 'http://localhost:4300/auth-callback',
  post_logout_redirect_uri: 'http://localhost:4300/',
  scope: 'USER_API SSO_API openid profile',
  silent_redirect_uri: 'http://localhost:4300/silent-refresh.html'
};
//////Linux host 172.16.80.54 linux container https//////
// export const environment = {
//   production: false,
//   apiUrl: 'https://172.16.80.54:5002',
//   authority: 'https://172.16.80.54:5000',
//   client_id: 'angular_user_manager',
//   redirect_uri: 'http://localhost:4300/auth-callback',
//   post_logout_redirect_uri: 'http://localhost:4300/',
//   scope: 'USER_API SSO_API openid profile',
//   silent_redirect_uri: 'http://localhost:4300/silent-refresh.html'
// };
//////Linux host 172.16.80.54 linux container http//////
// export const environment = {
//   production: false,
//   apiUrl: 'http://172.16.80.54:5003',
//   authority: 'http://172.16.80.54:5001',
//   client_id: 'angular_user_manager',
//   redirect_uri: 'http://localhost:4300/auth-callback',
//   post_logout_redirect_uri: 'http://localhost:4300/',
//   scope: 'USER_API SSO_API openid profile',
//   silent_redirect_uri: 'http://localhost:4300/silent-refresh.html'
// };