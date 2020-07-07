export const environment = {
  production: false,
  apiUrl: 'https://localhost:5000',
  authority: 'https://localhost:5000',
  client_id: 'angular_admin',
  redirect_uri: 'http://localhost:4200/auth-callback',
  post_logout_redirect_uri: 'http://localhost:4200/',
  scope: 'SSO_API openid profile',
  silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
};
