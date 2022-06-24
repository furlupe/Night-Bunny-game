using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string name;
    
    [TextArea(3, 10)]
    public string[] sentences;

    private XmlDocument _document = new();
}
