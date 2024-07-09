using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;
using Taxually.TechnicalTest.Core;
using Taxually.TechnicalTest.Core.Processors;

namespace Taxually.TechnicalTest.Test.VatRegistrationProcessorFactoryTests.Integration;

[TestFixture]
public class CreateTests
{
    private IVatRegistrationProcessorFactory _vatRegistrationProcessorFactory;
    private IServiceProvider _serviceProvider;

    public CreateTests()
    {
        var services = new ServiceCollection();

        var startup = new Startup();
        startup.ConfigureServices(services);

        _serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions());
    }

    [SetUp]
    public void Setup()
    {
        _vatRegistrationProcessorFactory = _serviceProvider.GetRequiredService<IVatRegistrationProcessorFactory>();
    }

    [TestCase("GB", typeof(ApiVatRegistrationProcessor))]
    [TestCase("FR", typeof(CsvVatRegistrationProcessor))]
    [TestCase("DE", typeof(XmlVatRegistrationProcessor))]
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
        action.Should().Throw<NotSupportedException>().WithMessage("Not supported country: HU");
    }
}
