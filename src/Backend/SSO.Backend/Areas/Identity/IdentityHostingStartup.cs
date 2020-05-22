using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SSO.Backend.Areas.Identity.IdentityHostingStartup))]
namespace SSO.Backend.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}