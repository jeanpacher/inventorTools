using System;
using System.IO;
namespace Bosch_ImportData
{
    public static class Log
    {
        static string logPath { get; set; } = @"C:\KeepSoftwares\Bosch\Log\LogGeral.txt";
        public static void gravarLog(string msg)
        {
            if (!Directory.Exists(Path.GetDirectoryName(logPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));

            File.AppendAllText(logPath, $"{DateTime.Now}: {msg} {System.Environment.NewLine}");
        }
    }
}
