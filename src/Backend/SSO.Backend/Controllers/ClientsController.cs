using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Data;
using SSO.Service.CreateModel.Client;
using SSO.Services;
using SSO.Services.CreateModel.Client;
using SSO.Services.ViewModel;
using SSO.Services.ViewModel.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers
{
    public partial class ClientsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientStore _clientStore;
        private readonly ConfigurationDbContext _configurationDbContext;

        public ClientsController(IClientStore clientStore,
            ConfigurationDbContext configurationDbContext,
            ApplicationDbContext context)
        {
            _context = context;
            _clientStore = clientStore;
            _configurationDbContext = configurationDbContext;
        }

        //Tạo client mới với 1 số thông tin cơ bản, nhưng chưa cài đặt các thuộc tính đầy đủ
        //Sau khi tạo xong thì sẽ bắt đầu thêm mới và cập nhật các cài đặt cho client. 
        [HttpPost]
        public async Task<IActionResult> PostClient([FromBody]ClientQuickRequest request)
        {
            var clientFind = await _clientStore.FindClientByIdAsync(request.ClientId);

            if (clientFind != null)
                return BadRequest($"ClientId {request.ClientId} already exist");
            var client = new IdentityServer4.EntityFramework.Entities.Client()
            {
                ClientId = request.ClientId,
                ClientName = request.ClientName,
                Description = request.Description,
                ClientUri = request.ClientUri,
                LogoUri = request.LogoUri,
                Created = DateTime.UtcNow
            };
            _configurationDbContext.Clients.Add(client);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("{clientId}")]
        public async Task<IActionResult> PutClient(string clientId,[FromBody]ClientRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if(client == null)
            {
                return NotFound();
            }
            client.Enabled = request.Enabled;
            client.ClientId = request.ClientId;
            client.ProtocolType = request.ProtocolType;
            client.RequireClientSecret = request.RequireClientSecret;
            client.ClientName = request.ClientName;
            client.Description = request.Description;
            client.ClientUri = request.ClientUri;
            client.LogoUri = request.LogoUri;
            client.RequireConsent = request.RequireConsent;
            client.AllowRememberConsent = request.AllowRememberConsent;
            client.AlwaysIncludeUserClaimsInIdToken = request.AlwaysIncludeUserClaimsInIdToken;
            client.RequirePkce = request.RequirePkce;
            client.AllowPlainTextPkce = request.AllowPlainTextPkce;
            client.AllowAccessTokensViaBrowser = request.AllowAccessTokensViaBrowser;
            client.FrontChannelLogoutUri = request.FrontChannelLogoutUri;
            client.FrontChannelLogoutSessionRequired = request.FrontChannelLogoutSessionRequired;
            client.BackChannelLogoutUri = request.BackChannelLogoutUri;
            client.BackChannelLogoutSessionRequired = request.BackChannelLogoutSessionRequired;
            client.AllowOfflineAccess = request.AllowOfflineAccess;
            client.IdentityTokenLifetime = request.IdentityTokenLifetime;
            client.AccessTokenLifetime = request.AccessTokenLifetime;
            client.AuthorizationCodeLifetime = request.AuthorizationCodeLifetime;
            client.ConsentLifetime = request.ConsentLifetime;
            client.AbsoluteRefreshTokenLifetime = request.AbsoluteRefreshTokenLifetime;
            client.SlidingRefreshTokenLifetime = request.SlidingRefreshTokenLifetime;
            client.RefreshTokenUsage = request.RefreshTokenUsage;
            client.UpdateAccessTokenClaimsOnRefresh = request.UpdateAccessTokenClaimsOnRefresh;
            client.RefreshTokenExpiration = request.RefreshTokenExpiration;
            client.AccessTokenType = request.AccessTokenType;
            client.EnableLocalLogin = request.EnableLocalLogin;
            client.IncludeJwtId = request.IncludeJwtId;
            client.AlwaysSendClientClaims = request.AlwaysSendClientClaims;
            client.ClientClaimsPrefix = request.ClientClaimsPrefix;
            client.PairWiseSubjectSalt = request.PairWiseSubjectSalt;
            client.Updated = DateTime.UtcNow;
            client.LastAccessed = request.LastAccessed;
            client.UserSsoLifetime = request.UserSsoLifetime;
            client.UserCodeType = request.UserCodeType;
            client.DeviceCodeLifetime = request.DeviceCodeLifetime;
            client.NonEditable = request.NonEditable;

            _configurationDbContext.Clients.Update(client);
            var result = await _configurationDbContext.SaveChangesAsync();
            if(result > 0)
            {
                return Ok();
            }
            return BadRequest();        
        }                 
        //Lấy thông tin cơ bản của các client
        [HttpGet]
        public async Task<IActionResult> GetClient()
        {
            var client = await _configurationDbContext.Clients.Select(x => new ClientQuickView()
            {
                ClientId = x.ClientId,
                ClientName = x.ClientName,
                LogoUri = x.LogoUri
            }).ToListAsync();
            return Ok(client);
        }
        // Tìm kiếm client
        [HttpGet("filter")]
        public async Task<IActionResult> GetClientPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _configurationDbContext.Clients.AsQueryable();
            
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.ClientId.Contains(filter) || x.ClientName.Contains(filter));
               
            }
            var totalReconds = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ClientQuickView()
                {
                    ClientId = x.ClientId,
                    ClientName = x.ClientName,
                    LogoUri = x.LogoUri
                }).ToListAsync();

            var pagination = new Pagination<ClientQuickView>
            {
                Items = items,
                TotalRecords = totalReconds
            };
            return Ok(pagination);
        }
        //Xóa client
        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if(client == null)
            {
                return NotFound();
            }
            _configurationDbContext.Clients.Remove(client);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }  
    } 
        
}