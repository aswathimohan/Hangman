using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Text;
using System.Collections.Generic;


public class XMLLogWriter {
    private string fileName;
    private List<LogPlayer> gamePlayers = new List<LogPlayer>();

    public void setFileName(string inFileName)
    {
        fileName = inFileName;
        if (File.Exists(this.filePath()))
        {
            File.Delete(this.filePath());
        }

        gamePlayers.Clear();
    }

    public string filePath()
    {
        string filepath = fileName;//Application.dataPath +
        return filepath;
    }

    public void log(LogPlayer player)
    {
        gamePlayers.Add(player);
        writeXml();
    }

    private void writeXml()
    {

        if (File.Exists(this.filePath()))
        {
            File.Delete(this.filePath());
        }

        XmlTextWriter textWriter = new XmlTextWriter(this.filePath(), null);
        // Opens the document
        textWriter.WriteStartDocument();
        textWriter.WriteComment("This document contains the player details that have been created.");
        textWriter.WriteStartElement("HangmanPlayers");
        textWriter.WriteWhitespace("\n");


        foreach (LogPlayer player in gamePlayers)
        {

            textWriter.WriteStartElement("LogPlayer");
            textWriter.WriteElementString("timeTaken", player.timeTaken);
            textWriter.WriteElementString("correctWord", player.correctWord);
            textWriter.WriteElementString("userAnswer", player.userAnswer);
            textWriter.WriteElementString("hintsUsed", player.hintsUsed.ToString());
            textWriter.WriteElementString("userScore", player.userScore.ToString());

            textWriter.WriteEndElement();
            textWriter.WriteWhitespace("\n");
        }



        textWriter.WriteEndElement();
        textWriter.WriteEndDocument();

       

        textWriter.Close();


    }

}
