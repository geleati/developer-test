using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Taxually.TechnicalTest.Core;
public class XmlSerialization<TModel> : IXmlSerialization<TModel>
    where TModel : class
{
    public string Serialize(TModel model)
    {
        using (var stringWriter = new StringWriter())
        {
            var serializer = new XmlSerializer(typeof(TModel));
            serializer.Serialize(stringWriter, model);
            return stringWriter.ToString();
        }
    }
}

public interface IXmlSerialization<TModel>
    where TModel : class
{
    string Serialize(TModel model);
}
