using System;
using System.IO;

namespace Loony.Tools
{
    public class Logger
    {
        //private readonly DataContext db;

        //public Logger(DbContext context)
        //{   
        //    db = context;
        //}

        public static void LogToTxt(string user, string controller, string action, string id)
        {
            string file = "\\Content\\Logs\\log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + ".txt";
            string rootDirectory = Directory.GetCurrentDirectory();
            var filePath = rootDirectory + file;

            FileStream fs;
            if (File.Exists(filePath))
                fs = new FileStream(filePath, FileMode.Append);
            else
                fs = new FileStream(filePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            var log = $"Date:{DateTime.Now}, UserId:{user}, Controller:{controller}, Action:{action}, RecordId:{id}";

            sw.Write(log + "\r\n");

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public static void LogToDb(string user, string controller, string action, string id)
        {

        }

    }
}
