using Taxually.TechnicalTest.Core.Model;

namespace Taxually.TechnicalTest.Core.Processors;

public class XmlVatRegistrationProcessor : VatRegistrationProcessorBase, IVatRegistrationProcessor
{
    private readonly ITaxuallyQueueClient _taxuallyQueueClient;
    private readonly IXmlSerialization<VatRegistrationRequest> _xmlSerialization;

    public override string HandledCountryCode => "DE";

    public XmlVatRegistrationProcessor(ITaxuallyQueueClient taxuallyQueueClient, IXmlSerialization<VatRegistrationRequest> xmlSerialization)
    {
        _taxuallyQueueClient = taxuallyQueueClient;
        _xmlSerialization = xmlSerialization;
    }

    public async Task Process(VatRegistrationRequest request)
    {
        // Queue file to be processed
        await _taxuallyQueueClient.EnqueueAsync("vat-registration-xml", _xmlSerialization.Serialize(request));
    }
}
