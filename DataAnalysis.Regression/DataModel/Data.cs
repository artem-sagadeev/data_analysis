using System;
using System.Linq;

namespace DataModel
{
    public class Data
    {
        public double Time { get; }
        public double Open { get; }
        public double High { get; }
        public double Low { get; }
        public double Close { get; }

        public Data(string line, double time)
        {
            var parsedParts = line
                .Split(';')
                .Skip(3)
                .Take(5)
                .Select(double.Parse)
                .ToArray();
            
            Time = time;
            Open = parsedParts[1];
            High = parsedParts[2];
            Low = parsedParts[3];
            Close = parsedParts[4];
        }
    }
}