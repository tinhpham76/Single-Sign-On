

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using USERAPI.Backend.Models;

namespace USERAPI.Backend.Services
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory,
          IConfiguration configuration,
          IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;           
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<bool> ChangePassword(string id, UserPasswordChangeRequest request)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
          
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync($"/api/users/{id}/change-password", data);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(string id)
        {
            return await DeleteAsync($"/api/users/{id}", true);
        }

        public async Task<UserViewModel> GetById(string id)
        {
            return await GetAsync<UserViewModel>($"/api/users/{id}", true);
        }
         
        public async Task<bool> PutUser(string id, UserUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);           

            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response =await client.PutAsync($"/api/users/{id}", data);
            return response.IsSuccessStatusCode;
        }
    }
}
