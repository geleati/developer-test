using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taxually.TechnicalTest.Core.Processors;

namespace Taxually.TechnicalTest.Core;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
     {
        services.AddScoped<ITaxuallyHttpClient, TaxuallyHttpClient>();
        services.AddScoped<ITaxuallyQueueClient, TaxuallyQueueClient>();
        services.AddScoped<IVatRegistrationProcessorFactory, VatRegistrationProcessorFactory>();
        services.AddScoped<IVatRegistrationProcessor, ApiVatRegistrationProcessor>();
        services.AddScoped<IVatRegistrationProcessor, CsvVatRegistrationProcessor>();
        services.AddScoped<IVatRegistrationProcessor, XmlVatRegistrationProcessor>();
        services.AddScoped(typeof(IXmlSerialization<>), typeof(XmlSerialization<>));
    }
}

