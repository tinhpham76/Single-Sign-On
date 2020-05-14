

using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Data;
using SSO.Backend.Services;
using SSO.Service.CreateModel;
using SSO.Services.CreateModel;
using SSO.Services.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers
{
    public class ClientsController : BaseController
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


        #region Thêm mới Client


        [HttpPost]
        public async Task<IActionResult> PostClient([FromBody]ClientCreateRequest request)
        {
            var clientFind = await _clientStore.FindClientByIdAsync(request.ClientId);

            if (clientFind != null)            
                return BadRequest($"ClientId {request.ClientId} already exist");                        
            foreach (var client in SaveClient.ISaveClient(request))
            {
                _configurationDbContext.Clients.Add(client.ToEntity());
            }               
            _configurationDbContext.SaveChanges();
            return Ok();
        }
            
  

        #endregion

        #region Truy vấn thông tin client
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clientQuickViews = await _context.Clients.Select(u => new ClientQuickView()
            {
                ClientId = u.ClientId,
                ClientName = u.ClientName,
                LogoUri = u.LogoUri
            }).ToListAsync();
            if (clientQuickViews == null)
                return NotFound();
            return Ok(clientQuickViews);
        }     
        
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientByClientId(string clientId)
        {
            var client = await _clientStore.FindClientByIdAsync(clientId);
            if (client == null)
            {
                return NotFound();
            }
            
            return Ok(client);
        }

        ////Get client voi pageIndex, page Size
        //[HttpGet("filter")]
        //public async Task<IActionResult> GetClientsPaging(string filter, int pageIndex, int pageSize)
        //{

        //    if (!string.IsNullOrEmpty(filter))
        //    {
        //        var query = _context.Clients.Where(x =>
        //        x.ClientId.Contains(filter) ||
        //        x.ClientName.Contains(filter));

        //        var totalRecords = await _context.Clients.CountAsync();
        //        var items = await query.Skip((pageIndex - 1 * pageSize))
        //            .Take(pageSize)
        //            .Select(c => new ClientQuickView()
        //            {
        //                ClientId = c.ClientId,
        //                ClientName = c.ClientName
        //            }).ToListAsync();
        //        var pagination = new Pagination<ClientQuickView>
        //        {
        //            Items = items,
        //            TotalRecords = totalRecords,
        //        };
        //        return Ok(pagination);
        //    }
        //    return BadRequest();
        //}
        #endregion

        #region Sửa thông tin Client
        [HttpPut("{clientId}")]
        public async Task<IActionResult> PutClient(string clientId, [FromBody]ClientUpdateRequest request)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            };
            //Update bảng Client
            #region table Client
            client.Enabled = request.Enabled;
            client.ClientId = request.ClientId;
            client.ClientName = request.ClientName;
            client.Description = request.Description;
            client.ClientUri = request.ClientUri;
            client.LogoUri = request.LogoUri;
            client.RequireConsent = request.RequireConsent;
            client.AllowOfflineAccess = request.AllowOfflineAccess;
            client.IdentityTokenLifetime = request.IdentityTokenLifetime;
            client.AccessTokenLifetime = request.AccessTokenLifetime;
            client.AuthorizationCodeLifetime = request.AuthorizationCodeLifetime;
            client.ConsentLifetime = request.ConsentLifetime;
            client.AbsoluteRefreshTokenLifetime = request.AbsoluteRefreshTokenLifetime;
            client.SlidingRefreshTokenLifetime = request.SlidingRefreshTokenLifetime;
            client.DeviceCodeLifetime = request.DeviceCodeLifetime;

            _context.Clients.Update(client);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        #endregion






        #endregion

        #region Xóa Client

        //Delete client
        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient(string clientId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null)
            {
                return NotFound($"Can not found client with client id {clientId}");
            }
            //Xóa
            var id = client.Id;
            _context.Clients.Remove(client);
            //Xóa ClientClaims
            var clientClaims = await _context.ClientClaims.FirstOrDefaultAsync(ru => ru.ClientId == id);
            if (clientClaims != null)
                _context.ClientClaims.Remove(clientClaims);
            //Xoa ClientCorsOrigin
            var clientCorsOrigin = await _context.ClientCorsOrigins.FirstOrDefaultAsync(cc => cc.ClientId == id);
            if (clientCorsOrigin != null)
                _context.ClientCorsOrigins.Remove(clientCorsOrigin);
            //Xoa ClientGrantTypes
            var clientGrantType = await _context.ClientGrantTypes.FirstOrDefaultAsync(pl => pl.ClientId == id);
            if (clientGrantType != null)
                _context.ClientGrantTypes.Remove(clientGrantType);
            //Xoa RedirecUri
            var clientRedirectUri = await _context.ClientRedirectUris.FirstOrDefaultAsync(ru => ru.ClientId == id);
            if (clientRedirectUri != null)
                _context.ClientRedirectUris.Remove(clientRedirectUri);
            //Xoa ClientPostLogoutRedirectUri
            var clientPostLogoutRedirectUri = await _context.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(pl => pl.ClientId == id);
            if (clientPostLogoutRedirectUri != null)
                _context.ClientPostLogoutRedirectUris.Remove(clientPostLogoutRedirectUri);                     
            //Xoa Scope
            var clientScope = await _context.ClientScopes.FirstOrDefaultAsync(pl => pl.ClientId == id);
            if (clientScope != null)
                _context.ClientScopes.Remove(clientScope);
            //Xoa Secret
            var clientSecret = await _context.ClientSecrets.FirstOrDefaultAsync(pl => pl.ClientId == id);
            if (clientSecret != null)
                _context.ClientSecrets.Remove(clientSecret);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();


        }

    }
#endregion
}