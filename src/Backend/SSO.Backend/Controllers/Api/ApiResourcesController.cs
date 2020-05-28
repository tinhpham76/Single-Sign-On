

using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Backend.Data;
using SSO.Services;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;
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

        //Show basic infor all Api Resource
        [HttpGet]
        public async Task<IActionResult> GetApiResources()
        {
            var apiResources = await _configurationDbContext.ApiResources.Select(x => new ApiResourcesQuickView()
            {
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
            return Ok(apiResources);
        }

        // Find clients with client name or id
        [HttpGet("filter")]
        public async Task<IActionResult> GetApiResourcePaging(string filter, int pageIndex, int pageSize)
        {
            var query = _configurationDbContext.ApiResources.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter));

            }
            var totalReconds = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ApiResourcesQuickView()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();

            var pagination = new Pagination<ApiResourcesQuickView>
            {
                Items = items,
                TotalRecords = totalReconds
            };
            return Ok(pagination);
        }

        //Get infor basic api resource
        [HttpGet("{apiResourceName}")]
        public async Task<IActionResult> GetApiResource(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var apiResourceViewModel = new ApiResourceViewModel()
            {
                Name = apiResource.Name,
                DisplayName = apiResource.DisplayName,
                Description = apiResource.Description,
                Enabled = apiResource.Enabled
            };
            return Ok(apiResourceViewModel);
        }

        //Post basic infor api resource
        [HttpPost]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiResource([FromBody]ApiResourceRequest request)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == request.Name);
            if (apiResource != null)
                return BadRequest($"Api Resource name {request.Name} already exist");
            var apiResourceRequest = new ApiResource()
            {
                Name = request.Name,
                DisplayName = request.DisplayName,
                Description = request.Description,
                Enabled = request.Enabled
            };
            _configurationDbContext.ApiResources.Add(apiResourceRequest.ToEntity());
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }


        //Edit basic infor api resource
        [HttpPut("{apiResourceName}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PutApiResourceBasic(string apiResourceName, [FromBody]ApiResourceRequest request)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return BadRequest();
            apiResource.Name = request.Name;
            apiResource.DisplayName = request.DisplayName;
            apiResource.Description = request.Description;
            apiResource.Enabled = request.Enabled;
            apiResource.Updated = DateTime.UtcNow;
            _configurationDbContext.ApiResources.Update(apiResource);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        //Delete api resource
        [HttpDelete("{apiResourceName}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteApiResource(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            _configurationDbContext.ApiResources.Remove(apiResource);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }
        #endregion
    }
}