﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAnalysis.Hw3
{
    static class Program
    {
        static void Main(string[] args)
        {
            var educationalData = InputData("../../../EducationalData.csv");
            var educationalInputs = PrepareInput(educationalData).ToArray();
            var educationalAnswers = PrepareAnswers(educationalData).ToArray();
            
            var testData = InputData("../../../TestData.csv");
            var testInputs = PrepareInput(testData).ToArray();
            var testAnswers = PrepareAnswers(testData).ToArray();
            
            var perceptron = new DoublePerceptron(10, 0.0001d, 5000);
            perceptron.Study(educationalInputs, educationalAnswers);
            
            var testError = perceptron.GetErrorsCount(testInputs, testAnswers);
            
            Console.WriteLine($"Errors count on test dataset: {testError}");
        }

        static double[] InputData(string path)
            => File
                .ReadAllLines(path)
                .Select(line => line.Split(';')[7])
                .Select(double.Parse)
                .ToArray();

        static IEnumerable<double[]> PrepareInput(double[] data)
        {
            for (var i = 0; i < data.Length - 13; i++)
            {
                yield return data.Skip(i).Take(10).ToArray();
            }
        }
        
        static IEnumerable<double> PrepareAnswers(this double[] data)
        {
            for (var i = 10; i < data.Length - 3; i++)
            {
                yield return GetTrendDirection(
                    data.Skip(i).Take(3).Select((_, x) => x).ToList(),
                    data.Skip(i).Take(3).Select((y, _) => y).ToList());
            }
        }

        static double GetTrendDirection(List<int> x, List<double> y)
        {
            var xAverage = x.Average();
            var yAverage = y.Average();
            var xyAverage = y.Select((t, i) => x[i] * t).Sum() / y.Count;
            var x2 = x.Select(e => e * e).Sum() / x.Count;
            return (xyAverage - xAverage * yAverage) / (x2 - xAverage * xAverage) > 0 ? 1 : 0;
        }
    }
}