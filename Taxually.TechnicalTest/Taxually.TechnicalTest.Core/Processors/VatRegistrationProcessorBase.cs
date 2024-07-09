namespace Taxually.TechnicalTest.Core.Processors;
public abstract class VatRegistrationProcessorBase : IVatRegistrationProcessorBase
{
    public abstract string HandledCountryCode { get; }

    public bool CanHandle(string countryCode)
    {
        return HandledCountryCode.Equals(countryCode);
    }
}
