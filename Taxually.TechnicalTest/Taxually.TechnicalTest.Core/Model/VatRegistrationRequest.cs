using System.Xml.Serialization;

namespace Taxually.TechnicalTest.Core.Model;

public class VatRegistrationRequest : IEquatable<VatRegistrationRequest>
{
    public string CompanyName { get; set; }
    public string CompanyId { get; set; }
    public string Country { get; set; }

    public override bool Equals(object obj)
    {
        return Equals(obj as VatRegistrationRequest);
    }

    public bool Equals(VatRegistrationRequest other)
    {
        return other != null &&
               Country == other.Country &&
               CompanyName == other.CompanyName &&
               CompanyId == other.CompanyId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CompanyId, Country, CompanyName);
    }

    public string Serialize()
    {
        using (var stringWriter = new StringWriter())
        {
            var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
            serializer.Serialize(stringWriter, this);
            return stringWriter.ToString();
        }
    }
}
