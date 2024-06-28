using Microsoft.Extensions.DependencyInjection;
using Taxually.TechnicalTest.Core.Model;

namespace Taxually.TechnicalTest.Core.Processors;

public interface IVatRegistrationProcessorFactory
{
    IVatRegistrationProcessor Create(string countryCode);
}

public class VatRegistrationProcessorFactory : IVatRegistrationProcessorFactory
{
    private readonly Dictionary<string, Func<IVatRegistrationProcessor>> _vatRegistrationProcessorsFactrory;

    public VatRegistrationProcessorFactory(IServiceProvider serviceProvider)
    {
        _vatRegistrationProcessorsFactrory = new Dictionary<string, Func<IVatRegistrationProcessor>>()
        {
            { "GB", () => serviceProvider.GetRequiredService<IApiVatRegistrationProcessor>() },
            { "FR", () => serviceProvider.GetRequiredService<ICsvVatRegistrationProcessor>() },
            { "DE", () => serviceProvider.GetRequiredService<IXmlVatRegistrationProcessor>() }
        };
    }

    public IVatRegistrationProcessor Create(string countryCode)
    {
        if (!_vatRegistrationProcessorsFactrory.TryGetValue(countryCode, out var processorFactrory))
        {
            throw new NotSupportedException("Not supported country");
        }

        return processorFactrory();
    }
}

public interface IVatRegistrationProcessor
{
    Task Process(VatRegistrationRequest request);
}