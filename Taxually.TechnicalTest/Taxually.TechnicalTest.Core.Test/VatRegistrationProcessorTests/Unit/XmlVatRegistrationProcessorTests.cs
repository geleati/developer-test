using NSubstitute;
using Taxually.TechnicalTest.Core.Model;
using Taxually.TechnicalTest.Core.Processors;

namespace Taxually.TechnicalTest.Core.Test.VatRegistrationProcessorTests.Unit;

[TestFixture]
public class XmlVatregistrationProcessorTests
{
    [Test]
    public async Task GivenVatRequestForCountryDE_WhenPost_ThenQueueShouldBeCalled()
    {
        // Arrange
        var queueClient = Substitute.For<ITaxuallyQueueClient>();
        var processor = new XmlVatRegistrationProcessor(queueClient);
        var vatRegistrationRequest = new VatRegistrationRequest
        {
            Country = "DE",
            CompanyId = "CompanyId_1",
            CompanyName = "CompanyName_1"
        };
        var xml = await File.ReadAllTextAsync(@"..\..\..\VatRegistrationProcessorTests\Unit\DE.xml");

        // Act
        await processor.Process(vatRegistrationRequest);

        // Assert
        await queueClient.Received(1).EnqueueAsync("vat-registration-xml", xml);
    }
}
