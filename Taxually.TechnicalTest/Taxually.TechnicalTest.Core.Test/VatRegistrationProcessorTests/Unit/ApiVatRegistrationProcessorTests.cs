using NSubstitute;
using Taxually.TechnicalTest.Core.Model;
using Taxually.TechnicalTest.Core.Processors;

namespace Taxually.TechnicalTest.Core.Test.VatRegistrationProcessorTests.Unit;

[TestFixture]
public class ApiVatregistrationProcessorTests
{
    [Test]
    public async Task GivenVatRequestForCountryGB_WhenPost_ThenApiShouldBeCalled()
    {
        // Arrange
        var httpClient = Substitute.For<ITaxuallyHttpClient>();
        var processor = new ApiVatRegistrationProcessor(httpClient);
        var vatRegistrationRequest = new VatRegistrationRequest
        {
            Country = "GB",
            CompanyId = "CompanyId_1",
            CompanyName = "CompanyName_1"
        };

        // Act
        await processor.Process(vatRegistrationRequest);

        // Assert
        await httpClient.Received(1).PostAsync("https://api.uktax.gov.uk", vatRegistrationRequest);
    }
}
