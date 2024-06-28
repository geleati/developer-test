using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;
using Taxually.TechnicalTest.Core;
using Taxually.TechnicalTest.Core.Processors;

namespace Taxually.TechnicalTest.Test.VatRegistrationFactoryTests.Integration;

[TestFixture]
public class CreateTests
{
    private IVatRegistrationProcessorFactory _vatRegistrationProcessorFactory;
    private IServiceProvider ServiceProvider { get; }

    public CreateTests()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .Build();

        var startup = new Startup(configuration);
        startup.ConfigureServices(services);

        ServiceProvider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        });
    }

    [SetUp]
    public void Setup()
    {
        _vatRegistrationProcessorFactory = ServiceProvider.GetRequiredService<IVatRegistrationProcessorFactory>();
    }

    [TestCase("GB", typeof(IApiVatRegistrationProcessor))]
    [TestCase("FR", typeof(ICsvVatRegistrationProcessor))]
    [TestCase("DE", typeof(IXmlVatRegistrationProcessor))]
    public void GivenCountryCode_WhenCreateProcessor_ThenProcessorWithExpectedTypeShouldBeReturned(string countryCode, Type processorType)
    {
        // Act
        var processor = _vatRegistrationProcessorFactory.Create(countryCode);

        // Assert
        processor.Should().BeAssignableTo(processorType);
    }

    [Test]
    public void GivenUnsupportedCountryCode_WhenCreateProcessor_ThenExceptionShouldBeThrown()
    {
        // Act
        var action = () => _vatRegistrationProcessorFactory.Create("HU");

        // Assert
        action.Should().Throw<NotSupportedException>().WithMessage("Not supported country");
    }
}
