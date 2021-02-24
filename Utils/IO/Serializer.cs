using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
//using System.Text.Json;
using System.Xml.Serialization;

namespace Utils.IO
{
    public class Serializer<T>
    {
        //Classe permettant de (dé)serialiser un objet dans l'un des formats json,xml ou binaire
        public Serializer(SerializeFormat format = SerializeFormat.Binary)
        {
            Format = format;
        }
        public Serializer(string fileName, SerializeFormat format = SerializeFormat.Binary)
        {
            FileName = fileName;
            Format = format;
        }

        public Serializer(string fileName, string directoryPath, SerializeFormat format = SerializeFormat.Binary)
        {
            FileName = fileName;
            DirectoryPath = directoryPath;
            Format = format;
        }

        public SerializeFormat Format { get; set; } = SerializeFormat.Binary;
        public string DirectoryPath { get; set; } = "";
        public string FileName { get; set; } = "";

        public void Write(T objectToSerialize, string fileFullName = "")
        {
            if (fileFullName == "")
            {
                fileFullName = Path.Combine(DirectoryPath, FileName);
            }
            if(Format == SerializeFormat.Binary)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, objectToSerialize);
                    new TFile(fileFullName).WriteBytes(ms.ToArray(), true);
                }
            }
            else if (Format == SerializeFormat.Xml)
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (StringWriter wr = new StringWriter())
                {
                    xs.Serialize(wr, objectToSerialize);
                    new TFile(fileFullName).WriteText(wr.ToString(), true);
                }
            }
            /*else //JSON
            {
                string jsonString = JsonSerializer.Serialize(objectToSerialize);
                new TFile(fileFullName).WriteText(jsonString, true);
            }*/
        }

        public T Read(string fileFullName = "")
        {
            if(fileFullName == "")
            {
                fileFullName = Path.Combine(DirectoryPath, FileName);
            }
            if (Format == SerializeFormat.Binary)
            {
                BinaryFormatter bf = new BinaryFormatter();
                byte[] bytes = new TFile(fileFullName).ReadBytes();
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    object obj = bf.Deserialize(ms);
                    return (T)obj;
                }
            }
            else// if (Format == SerializeFormat.Xml)
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                string xml = new TFile(fileFullName).ReadText();
                using (StringReader rd = new StringReader(xml))
                {
                    return (T)xs.Deserialize(rd);
                }
            }
            /*else //JSON
            {
                string json = new TFile(fileFullName).ReadText();
                return JsonSerializer.Deserialize<T>(json);
            }*/

        }
        

    }

    public enum SerializeFormat
    {
        Xml, /*Json,*/ Binary
    }
}
