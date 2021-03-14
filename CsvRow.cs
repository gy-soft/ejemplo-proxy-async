using System;

namespace ProxieAsync
{
    interface ICsvSerializable
    {
        string ToCsv(string logLevel);
    }
    
    class CsvRow : ICsvSerializable
    {
        public const string HEADER = "Timestamp,Level,Original,Compressed\n";

        public DateTime Timestamp { get; set; }
        public string Original { get; set; }
        public string Compressed { get; set; }

        public CsvRow(string original, string compressed)
        {
            Timestamp = DateTime.Now;
            Original = original;
            Compressed = compressed;
        }

        public string ToCsv(string logLevel)
        {
            return $"{Timestamp.ToLongTimeString()},{logLevel},{Original},{Compressed}\n";
        }
    }
}