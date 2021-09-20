using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAnalysis.Hw1
{
    class Program
    {
        static void Main(string[] args)
        {
            var y = File
                .ReadAllLines("../../../lines.csv")
                .Select(e => double.Parse(e.Split(";")[7]))
                .ToList();
            var x = Enumerable.Range(1, y.Count).ToList();
            
            var xAverage = x.Average();
            var yAverage = y.Average();
            var xyAverage = y.Select((t, i) => x[i] * t).Sum() / y.Count;
            var x2 = x.Select(e => e * e).Sum() / x.Count;
            var a = (xyAverage - xAverage * yAverage) / (x2 - xAverage * xAverage);
            var b = yAverage - a * xAverage;
            Console.WriteLine($"a = {a}, b = {b}");
            
            var inputs = y.Select((yi, i) => new [] { i, yi }).ToArray();
            var answers = x.Select((xi, i) => a * xi + b >= y[i] ? 1 : -1).ToArray();
            
            var model = new SinglePerceptron(2, 123.456d, 0.00001d);
            model.Fit(inputs, answers);
        }
    }
}