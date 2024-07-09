using System.Xml.Serialization;

namespace Taxually.TechnicalTest.Core.Model;

public record VatRegistrationRequest
{
    public string CompanyName { get; set; }
    public string CompanyId { get; set; }
    public string Country { get; set; }
}
