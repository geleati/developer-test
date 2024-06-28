using NSubstitute;
using System.Text;
using Taxually.TechnicalTest.Core.Model;
using Taxually.TechnicalTest.Core.Processors;

namespace Taxually.TechnicalTest.Core.Test.VatRegistrationProcessorTests.Unit;

[TestFixture]
public class CsvVatregistrationProcessorTests
{
    [Test]
    public async Task GivenVatRequestForCountryFR_WhenPost_ThenQueueShouldBeCalled()
    {
        // Arrange
        var queueClient = Substitute.For<ITaxuallyQueueClient>();
        var processor = new CsvVatRegistrationProcessor(queueClient);
        var vatRegistrationRequest = new VatRegistrationRequest
        {
            Country = "FR",
            CompanyId = "CompanyId_1",
            CompanyName = "CompanyName_1"
        };
        var csv = await File.ReadAllTextAsync(@"..\..\..\VatRegistrationProcessorTests\Unit\FR.csv");

        // Act
        await processor.Process(vatRegistrationRequest);

        // Assert
        await queueClient.Received(1).EnqueueAsync("vat-registration-csv", Arg.Is<byte[]>(bytes => Encoding.UTF8.GetString(bytes) == csv));
    }
}
