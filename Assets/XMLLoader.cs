using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

class XMLLoader
{
    public XMLLoader()
    {        

    }

    public static SinWave[] LoadFile(string fileName)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);
        var serializer = new XmlSerializer(typeof(Data));
        Data list = (Data)serializer.Deserialize(new StringReader(doc.InnerXml));
        return list.list;
    }


}