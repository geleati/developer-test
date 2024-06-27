using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Taxually.TechnicalTest.Core;
using Taxually.TechnicalTest.Core.Model;

namespace Taxually.TechnicalTest.Test.VatRegistrationControllerTests.Integration;

public class PostTests
{
    private IntegrationTestWebApplicationFactory _factory;
    private HttpClient _client;
    private ITaxuallyHttpClient _taxuallyHttpClient;
    private ITaxuallyQueueClient _taxuallyQueueClient;

    [OneTimeSetUp]
    public void FixtureSetup()
    {
        _factory = new IntegrationTestWebApplicationFactory();
        _taxuallyHttpClient = Substitute.For<ITaxuallyHttpClient>();
        _taxuallyQueueClient = Substitute.For<ITaxuallyQueueClient>();
        _client = _factory.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
        {
            services.AddSingleton(_taxuallyHttpClient);
            services.AddSingleton(_taxuallyQueueClient);
        })).CreateClient();
    }

    [Test]
    public async Task GivenRequest_WhenPost_ThenHttpClientCalledAndOkResponseShouldBeReturned()
    {
        // Arrange
        string inputUrl = "http://localhost/api/VatRegistration";
        var vatRegistrationRequest = new VatRegistrationRequest
        {
            Country = "GB",
            CompanyId = "CompanyId_1",
            CompanyName = "CompanyName_1"
        };

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(vatRegistrationRequest),
            Encoding.UTF8,
            "application/json");

        // Act
        var result = await _client.PostAsync(inputUrl, jsonContent);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        await _taxuallyHttpClient.Received(1).PostAsync("https://api.uktax.gov.uk", vatRegistrationRequest);
    }

    [Test]
    public async Task GivenRequest_WhenPost_ThenQueueClientCalledAndOkResponseShouldBeReturned()
    {
        // Arrange
        string inputUrl = "http://localhost/api/VatRegistration";
        var vatRegistrationRequest = new VatRegistrationRequest
        {
            Country = "FR",
            CompanyId = "CompanyId_1",
            CompanyName = "CompanyName_1"
        };
        var csv = await File.ReadAllTextAsync(@"..\..\..\VatRegistrationControllerTests\Integration\FR.csv");

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(vatRegistrationRequest),
            Encoding.UTF8,
            "application/json");

        // Act
        var result = await _client.PostAsync(inputUrl, jsonContent);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        await _taxuallyQueueClient.Received(1).EnqueueAsync("vat-registration-csv", Arg.Is<byte[]>(bytes => Encoding.UTF8.GetString(bytes) == csv));
    }

    [Test]
    public async Task GivenVatRequestForCountryDE_WhenPost_ThenInvalidOperationExceptionShouldBeThrown()
    {
        // Arrange
        string inputUrl = "http://localhost/api/VatRegistration";
        var vatRegistrationRequest = new VatRegistrationRequest
        {
            Country = "DE",
            CompanyId = "CompanyId_1",
            CompanyName = "CompanyName_1"
        };
        using StringContent jsonContent = new(
           JsonSerializer.Serialize(vatRegistrationRequest),
           Encoding.UTF8,
           "application/json");

        // Act
        var result = await _client.PostAsync(inputUrl, jsonContent);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        await _taxuallyQueueClient.DidNotReceive().EnqueueAsync(Arg.Any<string>(), Arg.Any<VatRegistrationRequest>());
    }

    [Test]
    public async Task GivenVatRequestForUnsupportedCountry_WhenPost_ThenInvalidOperationExceptionShouldBeThrown()
    {
        // Arrange
        string inputUrl = "http://localhost/api/VatRegistration";
        var vatRegistrationRequest = new VatRegistrationRequest
        {
            Country = "HU",
            CompanyId = "CompanyId_1",
            CompanyName = "CompanyName_1"
        };
        using StringContent jsonContent = new(
           JsonSerializer.Serialize(vatRegistrationRequest),
           Encoding.UTF8,
           "application/json");

        // Act
        var result = await _client.PostAsync(inputUrl, jsonContent);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        await _taxuallyQueueClient.DidNotReceive().EnqueueAsync(Arg.Any<string>(), Arg.Any<VatRegistrationRequest>());
    }

}

public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((host, configurationBuilder) => { });

    }
}