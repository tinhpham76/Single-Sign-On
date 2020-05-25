using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Client;
using SSO.Services.ViewModel.Client;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Clients
{
    public partial class ClientsController
    {
        #region Client Device Flows
        //Get Device Flows infor client for edit
        [HttpGet("{clientId}/deviceFlows")]
        public async Task<IActionResult> GetClientDeviceFlow(string clientId)
        {
            var client = await _clientStore.FindClientByIdAsync(clientId);
            if (client == null)
                return NotFound();
            var clientDeviceFlowViewModel = new ClientDeviceFlowViewModel()
            {
                UserCodeType = client.UserCodeType,
                DeviceCodeLifetime = client.DeviceCodeLifetime
            };
            return Ok(clientDeviceFlowViewModel);

        }

        //Edit Device Flows infor
        [HttpPut("{clientId}/deviceFlows")]
        public async Task<IActionResult> PutClientDeviceFlow(string clientId, [FromBody]ClientDeviceFlowRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            client.UserCodeType = request.UserCodeType;
            client.DeviceCodeLifetime = client.DeviceCodeLifetime;
            _configurationDbContext.Update(client);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }
        #endregion
    }
}
