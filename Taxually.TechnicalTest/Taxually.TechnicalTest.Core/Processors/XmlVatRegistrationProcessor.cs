using Taxually.TechnicalTest.Core.Model;

namespace Taxually.TechnicalTest.Core.Processors;

public interface IXmlVatRegistrationProcessor : IVatRegistrationProcessor 
{
}

public class XmlVatRegistrationProcessor : IXmlVatRegistrationProcessor
{
    private readonly ITaxuallyQueueClient _taxuallyQueueClient;

    public XmlVatRegistrationProcessor(ITaxuallyQueueClient taxuallyQueueClient)
    {
        _taxuallyQueueClient = taxuallyQueueClient;
    }

    public async Task Process(VatRegistrationRequest request)
    {
        // Queue file to be processed
        await _taxuallyQueueClient.EnqueueAsync("vat-registration-xml", request.Serialize());
    }
}
