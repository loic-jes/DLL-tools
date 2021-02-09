using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Tools.IO;

namespace TestFile
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Facture> factures = new List<Facture>() {
                new Facture("1","Alain Dupont",119.90, DateTime.Now),
                new Facture("2","Bernard Durant",79.90, DateTime.Now),
                new Facture("3","Christine Dupuis",149.90, DateTime.Now),
            };


            Serializer<List<Facture>> serializerBin = new Serializer<List<Facture>>("test.bin");
            serializerBin.Write(factures);
            List<Facture> readBin = serializerBin.Read();

            Serializer<List<Facture>>  serializerXml = new Serializer<List<Facture>>("test.xml", SerializeFormat.Xml);
            serializerXml.Write(factures);
            List<Facture> readXml = serializerXml.Read();

            Serializer<List<Facture>> serializerJson = new Serializer<List<Facture>>("test.json", SerializeFormat.Json);
            serializerJson.Write(factures);
            List<Facture> readJson = serializerJson.Read();

            /*Logger logger = new Logger(LoggerTerm.Renew);
            logger.Write("ABC");
            logger.Write("DEF");*/

        }
    }

    [Serializable]//Pour binary uniquement (pas obligatoire pour Json ou Xml)
    public class Facture
    {
        public Facture()
        {

        }
        public Facture(string numero, string client, double totalHT, DateTime date)
        {
            Numero = numero;
            Client = client;
            TotalHT = totalHT;
            Date = date;
        }

        public string Numero { get; set; }
        public string Client { get; set; }
        public double TotalHT { get; set; }
        public DateTime Date { get; set; }

    }
}
