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
    [XmlElement("Direction")]
    public string dir { get; set; }

    public SinWave()
    {
        A = 0.0f;
        B = 0.0f;
        C = 0.0f;
        D = 0.0f;
        dir = "X";
    }
    

    public Vector3 ComputeSinValue(float t)
	{
        Vector3 result = new Vector3(0, 0, 0);
		if (dir == "X")
			result.x = A* Mathf.Sin(B*(t - C)) + D;
		else if (dir == "Y")
            result.y = A* Mathf.Sin(B*(t - C)) + D;
		else if (dir == "Z")
            result.z = A* Mathf.Sin(B*(t - C)) + D;
        return result;
    }
}

