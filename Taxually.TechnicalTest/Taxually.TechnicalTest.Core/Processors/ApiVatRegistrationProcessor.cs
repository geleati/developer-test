using Taxually.TechnicalTest.Core.Model;

namespace Taxually.TechnicalTest.Core.Processors;

public class ApiVatRegistrationProcessor : VatRegistrationProcessorBase, IVatRegistrationProcessor
{
    private readonly ITaxuallyHttpClient _taxuallyHttpClient;
    public override string HandledCountryCode => "GB";

    public ApiVatRegistrationProcessor(ITaxuallyHttpClient taxuallyHttpClient)
    {
        _taxuallyHttpClient = taxuallyHttpClient;
    }

    public async Task Process(VatRegistrationRequest request)
    {
        await _taxuallyHttpClient.PostAsync("https://api.uktax.gov.uk", request);
    }
}