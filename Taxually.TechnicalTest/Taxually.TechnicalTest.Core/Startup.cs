using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taxually.TechnicalTest.Core.Processors;

namespace Taxually.TechnicalTest.Core;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
     {
        services.AddSingleton<ITaxuallyHttpClient, TaxuallyHttpClient>();
        services.AddSingleton<ITaxuallyQueueClient, TaxuallyQueueClient>();
        services.AddSingleton<IVatRegistrationProcessorFactory, VatRegistrationProcessorFactory>();
        services.AddSingleton<IApiVatRegistrationProcessor, ApiVatRegistrationProcessor>();
        services.AddSingleton<ICsvVatRegistrationProcessor, CsvVatRegistrationProcessor>();
        services.AddSingleton<IXmlVatRegistrationProcessor, XmlVatRegistrationProcessor>();
    }
}