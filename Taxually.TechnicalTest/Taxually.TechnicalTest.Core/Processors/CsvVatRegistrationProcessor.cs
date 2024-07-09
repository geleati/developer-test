using System.Text;
using Taxually.TechnicalTest.Core.Model;

namespace Taxually.TechnicalTest.Core.Processors;

public class CsvVatRegistrationProcessor : VatRegistrationProcessorBase, IVatRegistrationProcessor
{
    private readonly ITaxuallyQueueClient _taxuallyQueueClient;
    public override string HandledCountryCode => "FR";
    public CsvVatRegistrationProcessor(ITaxuallyQueueClient taxuallyQueueClient)
    {
        _taxuallyQueueClient = taxuallyQueueClient;
    }

    public async Task Process(VatRegistrationRequest request)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("CompanyName,CompanyId");
        csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
        var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
        // Queue file to be processed
        await _taxuallyQueueClient.EnqueueAsync("vat-registration-csv", csv);
    }
}
