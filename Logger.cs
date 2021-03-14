using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProxieAsync
{
    enum LogLevel
    {
        Info, Debug, Error
    }
    class Logger : IDisposable
    {
        private readonly Task logTask;
        private readonly BlockingCollection<string> bc = new BlockingCollection<string>();
        private const string LOG_PATH = "process-logs.csv";
        private readonly Dictionary<LogLevel, string> LogLevels = new Dictionary<LogLevel, string>
        {
            {LogLevel.Debug, "Debug"},
            {LogLevel.Error, "Error"},
            {LogLevel.Info, "Info"}
        };

        public Logger(string header)
        {
            InitializeLogFile(header);
            logTask = Task.Factory.StartNew(() => {
                foreach (var row in bc.GetConsumingEnumerable())
                {
                    File.AppendAllText(LOG_PATH, row);
                }
            });
        }

        public void AddRow(LogLevel logLevel, ICsvSerializable row)
        {
            bc.Add(row.ToCsv(LogLevels[logLevel]));
        }
        
        private void InitializeLogFile(string header)
        {
            if (File.Exists(LOG_PATH))
            {
                File.Delete(LOG_PATH);
            }
            File.AppendAllText(LOG_PATH, header);
        }

        public void Dispose()
        {
            bc.CompleteAdding();
            logTask.GetAwaiter().GetResult();
        }
    }
}