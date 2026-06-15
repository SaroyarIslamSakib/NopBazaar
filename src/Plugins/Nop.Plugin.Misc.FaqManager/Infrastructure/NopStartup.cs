using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.FaqManager.Admin.Factories;
using Nop.Plugin.Misc.FaqManager.Services;

namespace Nop.Plugin.Misc.FaqManager.Infrastructure;

public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<FaqService>();
        services.AddScoped<FaqInstallService>();
        services.AddScoped<FaqModelFactory>();
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order => 3000;
}