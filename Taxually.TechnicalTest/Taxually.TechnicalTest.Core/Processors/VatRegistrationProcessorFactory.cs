using Taxually.TechnicalTest.Core.Model;

namespace Taxually.TechnicalTest.Core.Processors;

public interface IVatRegistrationProcessorFactory
{
    IVatRegistrationProcessor Create(string countryCode);
}

public class VatRegistrationProcessorFactory : IVatRegistrationProcessorFactory
{
    private readonly IEnumerable<IVatRegistrationProcessor> _vatRegistrationProcessors;

    public VatRegistrationProcessorFactory(IEnumerable<IVatRegistrationProcessor> vatRegistrationProcessors)
    {
        _vatRegistrationProcessors = vatRegistrationProcessors;
    }

    /// <summary>
    /// Instantiate a particular instance of IVatRegistrationProcessor implementation for the given country
    /// </summary>
    /// <param name="countryCode">Country code</param>
    /// <returns>Instance of IVatRegistrationProcessor implementation</returns>
    /// <exception cref="NotSupportedException">Exception is thrown if there is no implementation for the given country</exception>
    public IVatRegistrationProcessor Create(string countryCode)
    {
        return _vatRegistrationProcessors.FirstOrDefault(vrp => vrp.CanHandle(countryCode)) ?? throw new NotSupportedException($"Not supported country: {countryCode}");
    }
}

public interface IVatRegistrationProcessor : IVatRegistrationProcessorBase
{
    /// <summary>
    /// Processing the given request for vat registration
    /// </summary>
    /// <param name="request">VatRegistrationRequest</param>
    /// <returns></returns>
    Task Process(VatRegistrationRequest request);
}

public interface IVatRegistrationProcessorBase
{
    bool CanHandle(string countryCode);
}