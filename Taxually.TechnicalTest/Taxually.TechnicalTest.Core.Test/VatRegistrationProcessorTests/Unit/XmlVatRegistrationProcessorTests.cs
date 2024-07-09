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
        var xmlSerialization = Substitute.For<IXmlSerialization<VatRegistrationRequest>>();     
        var processor = new XmlVatRegistrationProcessor(queueClient, xmlSerialization);
        var vatRegistrationRequest = new VatRegistrationRequest
        {
            Country = "DE",
            CompanyId = "CompanyId_1",
            CompanyName = "CompanyName_1"
        };
        var xml = await File.ReadAllTextAsync(@"..\..\..\VatRegistrationProcessorTests\Unit\DE.xml");
        xmlSerialization.Serialize(vatRegistrationRequest).Returns(xml);

        // Act
        await processor.Process(vatRegistrationRequest);

        // Assert
        await queueClient.Received(1).EnqueueAsync("vat-registration-xml", xml);
    }
}
