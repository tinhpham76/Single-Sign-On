export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001',
  authority: 'https://localhost:5000',
  client_id: 'angular_user_manager',
  redirect_uri: 'http://localhost:4300/auth-callback',
  post_logout_redirect_uri: 'http://localhost:4300/',
  scope: 'USER_API SSO_API openid profile',
  silent_redirect_uri: 'http://localhost:4300/silent-refresh.html'
};
