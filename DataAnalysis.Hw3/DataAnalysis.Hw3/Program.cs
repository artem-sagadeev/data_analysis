using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAnalysis.Hw3
{
    static class Program
    {
        static void Main(string[] args)
        {
            var data = InputData();
            var inputs = PrepareInput(data).ToArray();
            var answers = PrepareAnswers(data).ToArray();
            
            var perceptron = new DoublePerceptron(0.001d);
            perceptron.Fit(inputs, answers);
        }

        static double[] InputData()
            => File
                .ReadAllLines("../../../lines.csv")
                .Select(line => line.Split(';')[7].Replace('.', ','))
                .Select(double.Parse)
                .ToArray();

        static IEnumerable<double[]> PrepareInput(double[] data)
        {
            for (var i = 0; i < data.Length; i += 10)
            {
                yield return data.Skip(i).Take(10).ToArray();
            }
        }
        
        static IEnumerable<int> PrepareAnswers(this double[] data)
        {
            for (var i = 10; i < data.Length; i += 3)
            {
                yield return GetTrendDirection(
                    data.Skip(i).Take(3).Select((_, x) => x).ToList(),
                    data.Skip(i).Take(3).Select((y, _) => y).ToList());
            }
        }

        static int GetTrendDirection(List<int> x, List<double> y)
        {
            var xAverage = x.Average();
            var yAverage = y.Average();
            var xyAverage = y.Select((t, i) => x[i] * t).Sum() / y.Count;
            var x2 = x.Select(e => e * e).Sum() / x.Count;
            return (xyAverage - xAverage * yAverage) / (x2 - xAverage * xAverage) > 0 ? 1 : -1;
        }
    }
}