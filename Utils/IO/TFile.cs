using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils.IO
{
    public class TFile
    {
        /*public TFile()
        {

        }*/
        public static TFile GetInstance(string fileName, string directoryPath = "") 
        {
            return new TFile(fileName, directoryPath);
        }
        //TODO try catch write/read

        public TFile(string fileName, string directoryPath = "")
        {
            if(fileName == "" || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                fileName = "default.txt";
            }
            
            char[] arr = Path.GetInvalidPathChars();
            string badChars = new string(Path.GetInvalidFileNameChars()).Replace(@"\","");
            Regex regex = new Regex("[" + Regex.Escape(badChars) + "]");
            bool test = regex.IsMatch(directoryPath) || string.IsNullOrWhiteSpace(directoryPath);
            if (test) 
                directoryPath = "";

            string fullFileName = Path.Combine(directoryPath, fileName);
            Info = new FileInfo(fullFileName);
            
        }

        public FileInfo Info { get; private set; }
        public bool Exists { get { return File.Exists(Info.FullName); } }

        public TFile Create(bool overwrite = false)
        {
            if (!Info.Directory.Exists)
            {
                try
                {
                    Info.Directory.Create();
                }
                catch
                {
                }
            }
            if(overwrite || !Exists)
            {
                Info.Create().Close();
            }
            return this;
        }

        public bool Delete()
        {
            if (Exists)
            {
                try
                {
                    Info.Delete();
                }
                catch
                {
                }
            }
            return !Exists;
        }

        public void WriteBytes(byte[] bytes, bool overwrite = false)
        {
            //Create(overwrite);

            if (!overwrite)
            {
                try
                {
                    bytes = File.ReadAllBytes(Info.FullName).Concat(bytes).ToArray();
                }
                catch
                {
                }
            }

            try
            {
                File.WriteAllBytes(Info.FullName, bytes);
            }
            catch
            {
            }
        }

        public void WriteText(string text, bool owerwrite = false)
        {
            Create(owerwrite);
            try
            {
                File.AppendAllText(Info.FullName, text);
            }
            catch
            {
            }

        }

        public void WriteLine(string line, bool owerwrite = false)
        {
            Create(owerwrite);
            
            //Methodes 1
            //StreamWriter sw = File.AppendText(Info.FullName);
            //sw.WriteLine(line);
            //sw.Close();
            //Methode 2
            //File.AppendAllLines(Info.FullName, new string[] { line });

            //rn permet le passage à la ligne si le fichier n'est pas vide
            string rn = "\r\n";
            int length = 0;
            try
            {
                length = File.ReadAllBytes(Info.FullName).Length;
            }
            catch
            {
            }
            if (length == 0)
            {
                rn = "";
            }
            try
            {
                File.AppendAllText(Info.FullName, String.Concat(rn, line));
            }
            catch
            {
            }

        }

        public void WriteAllLines(string[] lines, bool owerwrite = false)
        {
            Create(owerwrite);
            //rn permet le passage à la ligne si le fichier n'est pas vide
            string rn = "\r\n";
            int length = 0;
            try
            {
                length = File.ReadAllBytes(Info.FullName).Length;
            }
            catch
            {
            }
            if (length == 0)
            {
                rn = "";
            }
            foreach (string line in lines)
            {
                try
                {
                    File.AppendAllText(Info.FullName, String.Concat(rn, line));
                }
                catch
                {
                }
                rn = "\r\n";
            }
        }

        public byte[] ReadBytes()
        {
            if (!Exists) return null;
            try
            {
                return File.ReadAllBytes(Info.FullName);
            }
            catch
            {
            }
            return null;
        }

        public string ReadText()
        {
            if (!Exists) return null;
            try
            {
                return File.ReadAllText(Info.FullName);
            }
            catch
            {
            }
            return null;
        }

        public string ReadLine(int indice)
        {
            if (!Exists) return null;
            try
            {
                return File.ReadLines(Info.FullName).ElementAt(indice);
            }
            catch
            {
            }
            return null;
        }

        public string[] ReadAllLines()
        {
            if (!Exists) return null;
            try
            {
                return File.ReadAllLines(Info.FullName);
            }
            catch
            {
            }
            return null;
        }

        public List<string> ReadAllLines(bool list = true)
        {
            if (!Exists) return null;
            try
            {
                return File.ReadLines(Info.FullName).ToList();
            }
            catch
            {
            }
            return null;
        }

        public bool CopyTo(string directoryPath, string renamedFileName = "", bool overwrite = false)
        {
            if(renamedFileName == "")
            {
                renamedFileName = Info.Name;
            }
            try
            {
                File.Copy(Info.FullName, String.Concat(directoryPath, renamedFileName), overwrite);
            }
            catch
            {
            }
            TFile copy = new TFile(renamedFileName, directoryPath);
            return copy.Exists;
        }

        public bool MoveTo(string directoryPath, string renamedFileName = "", bool overwrite = false)
        {
            if (renamedFileName == "")
            {
                renamedFileName = Info.Name;
            }
            try
            {
                File.Move(Info.FullName, String.Concat(directoryPath, renamedFileName));//, overwrite);
            }
            catch
            {
            }
            TFile copy = new TFile(renamedFileName, directoryPath);
            return copy.Exists;

        }

    }
}
