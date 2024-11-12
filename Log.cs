using System;
using System.IO;
namespace Bosch_ImportData
{
    public enum LogName
    {
        AppLog,
        ProductLog,
        ArquivosFaltantes
    };

    public static class Log
    {
        static string logPath { get; set; } = @"C:\KeepCAD\ImportacaoArquivos\Log\";
     

        public static void GravarLog(string msg, LogName logName = LogName.AppLog)
        {
            string logFileName = Path.Combine(logPath, $"{logName}.txt");

            if (!Directory.Exists(Path.GetDirectoryName(logFileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(logFileName));

            File.AppendAllText(logFileName, $"{DateTime.Now}: {msg}{Environment.NewLine}");
        }
    }
}
