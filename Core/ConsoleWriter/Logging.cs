using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using Rocket;

namespace ConsoleWriter
{
    public class Writer
    {
        private static bool mDisabled;

        public static bool DisabledState
        {
            get { return mDisabled; }
            set { mDisabled = value; }
        }

        public static void WriteLine(string Line, ConsoleColor Colour = ConsoleColor.Gray)
        {
            if (!mDisabled)
            {
                Console.ForegroundColor = Colour;
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + Line);
            }
        }

        public static void LogException(string logText)
        {
            WriteToFile(@"rocket_system\rocketerros\exceptions.txt", logText + "\r\n\r\n");
            //WriteLine("Exception has been saved", ConsoleColor.Red);
        }

        public static void LogCriticalException(string logText)
        {
            WriteToFile(@"rocket_system\rocketerros\criticalexceptions.txt", logText + "\r\n\r\n");
            //WriteLine("CRITICAL ERROR LOGGED", ConsoleColor.Red);
        }

        public static void LogMySQLError(string logText)
        {
            WriteToFile(@"rocket_system\rocketerros\mysql_error.txt", logText + "\r\n\r\n");
            //WriteLine("CRITICAL ERROR LOGGED", ConsoleColor.Red);
        }

        public static void LogWiredException(string logText)
        {
            WriteToFile(@"rocket_system\rocketerros\wiredexceptions.txt", logText + "\r\n\r\n");
        }

        public static void LogCacheException(string logText)
        {
            WriteToFile(@"rocket_system\rocketerros\cacheexceptions.txt", logText + "\r\n\r\n");
        }

        public static void LogPathfinderException(string logText)
        {
            WriteToFile(@"rocket_system\rocketerros\pathfinderexceptions.txt", logText + "\r\n\r\n");
        }

        public static void LogThreadException(string Exception, string Threadname)
        {
            WriteToFile(@"rocket_system\rocketerros\threaderror.txt", "Error in thread " + Threadname + ": \r\n" + Exception + "\r\n\r\n");
            //WriteLine("Error in " + Threadname + " caught", ConsoleColor.Red);
        }

        public static void LogQueryError(Exception Exception, string query)
        {
            WriteToFile(@"rocket_system\rocketerros\MySQLerrors.txt", "Error in query: \r\n" + query + "\r\n" + Exception + "\r\n\r\n");
            //WriteLine("Error in query caught", ConsoleColor.Red);
        }

        public static void LogPacketException(string Packet, string Exception)
        {
            WriteToFile(@"rocket_system\rocketerros\packeterror.txt", "Error in packet " + Packet + ": \r\n" + Exception + "\r\n\r\n");
            //WriteLine("Packet error!", ConsoleColor.Red);
        }

        public static void HandleException(Exception pException, string pLocation)
        {
            var ExceptionData = new StringBuilder();
            ExceptionData.AppendLine("Exception logged " + DateTime.Now.ToString() + " in " + pLocation + ":");
            ExceptionData.AppendLine(pException.ToString());
            if (pException.InnerException != null)
            {
                ExceptionData.AppendLine("Inner exception:");
                ExceptionData.AppendLine(pException.InnerException.ToString());
            }
            if (pException.HelpLink != null)
            {
                ExceptionData.AppendLine("Help link:");
                ExceptionData.AppendLine(pException.HelpLink);
            }
            if (pException.Source != null)
            {
                ExceptionData.AppendLine("Source:");
                ExceptionData.AppendLine(pException.Source);
            }
            if (pException.Data != null)
            {
                ExceptionData.AppendLine("Data:");
                foreach (DictionaryEntry Entry in pException.Data)
                {
                    ExceptionData.AppendLine("  Key: " + Entry.Key + "Value: " + Entry.Value);
                }
            }
            if (pException.Message != null)
            {
                ExceptionData.AppendLine("Message:");
                ExceptionData.AppendLine(pException.Message);
            }
            if (pException.StackTrace != null)
            {
                ExceptionData.AppendLine("Stack trace:");
                ExceptionData.AppendLine(pException.StackTrace);
            }
            ExceptionData.AppendLine();
            ExceptionData.AppendLine();
            LogException(ExceptionData.ToString());
        }

        public static void DisablePrimaryWriting(bool ClearConsole)
        {
            mDisabled = true;
            if (ClearConsole)
                Console.Clear();
        }

        private static void WriteToFile(string path, string content)
        {
            try
            {
                FileStream Writer = new FileStream(path, FileMode.Append, FileAccess.Write);
                byte[] Msg = Encoding.ASCII.GetBytes(Environment.NewLine + content);
                Writer.Write(Msg, 0, Msg.Length);
                Writer.Dispose();
            }
            catch (Exception e)
            {
                WriteLine("Could not write to file: " + e + ":" + content);
            }
        }

        private static void WriteCallback(IAsyncResult callback)
        {
            var stream = (FileStream)callback.AsyncState;
            stream.EndWrite(callback);
            stream.Dispose();
        }
    }
}