using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utils.IO
{
    public class Logger
    {
        public Logger()
        {

        }

        public Logger(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }

        public Logger(LoggerTerm term)
        {
            Term = term;
        }

        public Logger(string directoryPath, LoggerTerm term)
        {
            DirectoryPath = directoryPath;
            Term = term;
        }

        public string DirectoryPath { get; private set; } = "Logs";
        public LoggerTerm Term { get; private set; } = LoggerTerm.Last;
        public string CurrentLog { get; private set; }

        private TFile LogFile { get; set; }

        public string GenerateLogName(DateTime? date = null)
        {
            if (date == null) date = DateTime.Now;
            string format = "yyyyMMdd_HHmmss_f";
            return "Log_" + ((DateTime)date).ToString(format) + ".log";
        }

        public void Write(params string[] messages)
        {
            
            if (Directory.Exists(DirectoryPath) && CurrentLog == null)
            {
                CurrentLog = Directory.GetFiles(DirectoryPath).Max();
                if (CurrentLog != null)
                {
                    CurrentLog = new FileInfo(CurrentLog).Name;
                }
            }
            
            if (CurrentLog == null)
            {
                CurrentLog = GenerateLogName();
            }
            
            if (Term != LoggerTerm.Last)
            {
                string logName = GenerateLogName();
                LoggerTerm comparison = CompareLogNames(logName, CurrentLog);
                if (Term < comparison)
                {
                    CurrentLog = GenerateLogName();
                }
            }
            LogFile = new TFile(CurrentLog, DirectoryPath);
            for (int i = 0; i<messages.Length; i++)
            {
                string prefix = DateTime.Now.ToString("dd/MM HH:mm:ss > ");
                messages[i] = prefix + messages[i];
            }
            LogFile.WriteAllLines(messages);
        }

        private static int CompareString(string str1, string str2)
        {
            int index = -1;
            for (int i = 0; i < Math.Min(str1.Length, str2.Length); i++)
            {
                if(str1[i] != str2[i])
                {
                    break;
                }
                index++;
            }
            return index;
        }

        private static LoggerTerm CompareLogNames(string logName1, string logName2)
        {
            LoggerTerm result = LoggerTerm.Last;
            int index = CompareString(logName1, logName2);
            //logName1.Zip(logName2, (c1, c2) => c1 == c2).TakeWhile(b => b).Count();
            if (index >= 15)
            {
                //SameHour
                result = LoggerTerm.Hour;
            }
            else if (index >= 12)
            {
                // SameDay
                result = LoggerTerm.Day;
            }
            else if (index >= 10)
            {
                //SameMonth
                result = LoggerTerm.Month;
            }
            return result;
        }

    }

    public enum LoggerTerm
    {
        Renew, Hour, Day, Month, Last
    }
}
