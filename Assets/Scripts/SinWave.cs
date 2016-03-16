using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

[XmlType("Data")]
public class Data
{
    [XmlArray("ListOfWaves")]
    public SinWave[] list { get; set; }
}


[XmlType("Wave")]
public class SinWave
{
    [XmlElement("A")]
    public float A { get; set; }
    [XmlElement("B")]
    public float B { get; set; }
    [XmlElement("C")]
    public float C { get; set; }
    [XmlElement("D")]
    public float D { get; set; }
    [XmlElement("Angle")]
    public float angle{ get; set; }

    public SinWave()
    {
        A = 0.0f;
        B = 0.0f;
        C = 0.0f;
        D = 0.0f;
        angle = 0.0f;
    }

    public float ComputeSinValue(float x, float z)
    {
        //return Mathf.Sin(x) * Mathf.Cos(z);

        //return Mathf.Sin(Mathf.Sqrt(x * x + z * z));
        return A * Mathf.Sin(B * (Mathf.Sqrt(x * x + z * z) - C)) + D;
    }
}

