

using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Data;
using SSO.Services.RequestModel.ApiResource;
using SSO.Services.ViewModel.ApiResource;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Api
{
    
    public partial class ApiResourcesController : BaseController
    {
        #region ApiResource
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly ApplicationDbContext _context;
        public ApiResourcesController(ApplicationDbContext context,
            ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
            _context = context;
        }
        
        //Get Api Resource basic info
        [HttpGet]
        public async Task<IActionResult> GetApiResources()
        {
            var apiResource = await _context.ApiResources.Select(x => new ApiResourceQuickView()
            {
                Id = x.Id,
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description
            }).ToListAsync();
            return Ok(apiResource);
        }

        //Get Api Resource detail
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApiResourceDetail(int id)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            var apiResourceViewModel = new ApiResourceViewModel()
            {
               Id = apiResource.Id,
               Enabled = apiResource.Enabled,
               Name = apiResource.Name,
               DisplayName = apiResource.DisplayName,
               Description = apiResource.Description,
               Created = apiResource.Created,
               Updated = apiResource.Updated,
               LastAccessed = apiResource.LastAccessed
            };
            return Ok(apiResourceViewModel);
        }


        //Create basic info api resource
        [HttpPost]
        public async Task<IActionResult> PostApiResource([FromBody]ApiResourceQuickRequest request)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x=>x.Name == request.Name);
            if (apiResource != null)
                return BadRequest($"Api Resource {request.Name} already exist");
            var apiResourceRequest = new ApiResource()
            {
                Name = request.Name,
                DisplayName = request.DisplayName,
                Description = request.Description
            };
            _context.ApiResources.Add(apiResourceRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        //Put Api Resource with id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApiResource(int id, [FromBody]ApiResourceRequest request)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            apiResource.Enabled = request.Enabled;
            apiResource.Name = request.Name;
            apiResource.DisplayName = request.DisplayName;
            apiResource.Description = request.Description;
            apiResource.Updated = DateTime.UtcNow;

            _context.ApiResources.Update(apiResource);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        //Delelete Api Resource with id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApiResource(int id)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            _configurationDbContext.ApiResources.Remove(apiResource);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }



        #endregion
    }
}