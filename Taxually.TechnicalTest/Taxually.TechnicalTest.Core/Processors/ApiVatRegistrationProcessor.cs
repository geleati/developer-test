using Taxually.TechnicalTest.Core.Model;

namespace Taxually.TechnicalTest.Core.Processors;

public interface IApiVatRegistrationProcessor : IVatRegistrationProcessor 
{
}

public class ApiVatRegistrationProcessor : IApiVatRegistrationProcessor
{
    private readonly ITaxuallyHttpClient _taxuallyHttpClient;

    public ApiVatRegistrationProcessor(ITaxuallyHttpClient taxuallyHttpClient)
    {
        _taxuallyHttpClient = taxuallyHttpClient;
    }

    public async Task Process(VatRegistrationRequest request)
    {
        await _taxuallyHttpClient.PostAsync("https://api.uktax.gov.uk", request);
    }
}