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

    /// <summary>
    /// Instantiate a particular instance of IVatRegistrationProcessor implementation for the given country
    /// </summary>
    /// <param name="countryCode">Country code</param>
    /// <returns>Instance of IVatRegistrationProcessor implementation</returns>
    /// <exception cref="NotSupportedException">Exception is thrown if there is no implementation for the given country</exception>
    public IVatRegistrationProcessor Create(string countryCode)
    {
        if (!_vatRegistrationProcessorsFactrory.TryGetValue(countryCode, out var processorFactory))
        {
            throw new NotSupportedException("Not supported country");
        }

        return processorFactory();
    }
}

public interface IVatRegistrationProcessor
{
    /// <summary>
    /// Processing the given request for vat registration
    /// </summary>
    /// <param name="request">VatRegistrationRequest</param>
    /// <returns></returns>
    Task Process(VatRegistrationRequest request);
}