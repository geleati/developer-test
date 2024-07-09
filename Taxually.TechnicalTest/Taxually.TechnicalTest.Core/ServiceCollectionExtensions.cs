using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Taxually.TechnicalTest.Core;

namespace Taxually.TechnicalTest;

public static class ServiceCollectionExtensions
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        var startup = new Startup();
        startup.ConfigureServices(services);
    }
}
