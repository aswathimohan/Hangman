using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;



[XmlRoot("PlayerDetails")]
public class LogPlayer
{
    [XmlElement("timeTaken")]
    public string timeTaken { get; set; }

    [XmlElement("correctWord")]
    public string correctWord { get; set; }

    [XmlElement("userAnswer")]
    public string userAnswer { get; set; }

    [XmlElement("hintsUsed")]
    public bool hintsUsed { get; set; }

    [XmlElement("userScore")]
    public int userScore { get; set; }


}

