using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Services.RequestModel.Client;
using SSO.Services.ViewModel.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Clients
{
    public partial class ClientsController
    {
        #region Client Token
        //Get token infor client for edit
        [HttpGet("{clientId}/tokens")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetClientToken(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            var clientTokenViewModel = new ClientTokenViewModel()
            {
                IdentityTokenLifetime = client.IdentityTokenLifetime,
                AccessTokenLifetime = client.AccessTokenLifetime,
                AccessTokenType = (client.AccessTokenType == 0) ? "Jwt" : "Reference",
                AuthorizationCodeLifetime = client.AuthorizationCodeLifetime,
                AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime,
                SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime,
                RefreshTokenUsage = (client.RefreshTokenUsage == 0) ? "ReUse" : "OneTimeOnly",
                RefreshTokenExpiration = (client.RefreshTokenUsage == 0) ? "Sliding" : "Absolute",
                UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh,
                IncludeJwtId = client.IncludeJwtId,
                AlwaysSendClientClaims = client.AlwaysSendClientClaims,
                AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken,
                PairWiseSubjectSalt = client.PairWiseSubjectSalt,
                ClientClaimsPrefix = client.ClientClaimsPrefix
            };
            return Ok(clientTokenViewModel);
        }

        //Edit token infor
        [HttpPut("{clientId}/tokens")]
        [ClaimRequirement(PermissionCode.SSO_UPDATE)]
        public async Task<IActionResult> PutClientBasic(string clientId, [FromBody] ClientTokenRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            //Table Clients
            client.IdentityTokenLifetime = request.IdentityTokenLifetime;
            client.AccessTokenLifetime = request.AccessTokenLifetime;
            client.AccessTokenType = request.AccessTokenType.Contains("Jwt") ? 0 : 1;
            client.AuthorizationCodeLifetime = request.AuthorizationCodeLifetime;
            client.AbsoluteRefreshTokenLifetime = request.AbsoluteRefreshTokenLifetime;
            client.SlidingRefreshTokenLifetime = request.SlidingRefreshTokenLifetime;
            client.RefreshTokenUsage = request.RefreshTokenUsage.Contains("ReUse") ? 0 : 1;
            client.RefreshTokenExpiration = request.RefreshTokenExpiration.Contains("Sliding") ? 0 : 1;
            client.UpdateAccessTokenClaimsOnRefresh = request.UpdateAccessTokenClaimsOnRefresh;
            client.IncludeJwtId = request.IncludeJwtId;
            client.AlwaysSendClientClaims = request.AlwaysSendClientClaims;
            client.AlwaysIncludeUserClaimsInIdToken = request.AlwaysIncludeUserClaimsInIdToken;
            client.PairWiseSubjectSalt = request.PairWiseSubjectSalt;
            client.ClientClaimsPrefix = request.ClientClaimsPrefix;
            client.Updated = DateTime.UtcNow;

            _configurationDbContext.Update(client);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        // Client Claim
        [HttpGet("{clientId}/tokens/clientClaims")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetClientClientClaims(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            var query = _context.ClientClaims.Where(x => x.ClientId.Equals(client.Id));
            var clientClaims = await query.Select(x => new ClientClaimViewModel()
            {
                Id = x.Id,
                Value = x.Value,
                Type = x.Type,
            }).ToListAsync();

            return Ok(clientClaims);
        }

        // Post client claim
        [HttpPost("{clientId}/tokens/clientClaims")]
        [ClaimRequirement(PermissionCode.SSO_CREATE)]
        public async Task<IActionResult> PostClientClaim(string clientId, [FromBody] ClientClaimRequest request)
        {
            //Check client
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            //If client not null, Check client Secret
            if (client != null)
            {
                var temp = await _context.ClientClaims.FirstOrDefaultAsync(x => x.Type == request.Type);
                if (temp != null)
                    return BadRequest();
                var clientClaimRequest = new IdentityServer4.EntityFramework.Entities.ClientClaim()
                {
                    Type = request.Type,
                    Value = request.Value,
                    ClientId = client.Id,
                };
                _context.ClientClaims.Add(clientClaimRequest);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    client.Updated = DateTime.UtcNow;
                    _configurationDbContext.Clients.Update(client);
                    await _configurationDbContext.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        //Delete client claim
        [HttpDelete("{clientId}/tokens/clientClaims/{claimId}")]
        [ClaimRequirement(PermissionCode.SSO_DELETE)]
        public async Task<IActionResult> DeleteClientClaim(string clientId, int claimId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            var clientClaim = await _context.ClientClaims.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.Id == claimId);
            if (clientClaim == null)
                return NotFound();
            _context.ClientClaims.Remove(clientClaim);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                client.Updated = DateTime.UtcNow;
                _configurationDbContext.Clients.Update(client);
                await _configurationDbContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        #endregion
    }
}
