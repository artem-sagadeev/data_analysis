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
                .ReadAllLines("lines.csv")
                .Select(e => double.Parse(e.Split(";")[7].Replace(".", ",")))
                .ToList();
            var i = 0;
            var x = y.Select(_ => i++).ToList();
            var newX = x.Sum() / x.Count;
            var newY = y.Sum() / y.Count;
            var xy = 0d;
            for (var j = 0; j < y.Count; j++)
            {
                xy += x[j] * y[j];
            }

            var x2 = x.Select(e => e * e).Sum() / x.Count;
            var a = (xy - newX * newY) / (x2 - newX * newX);
            var b = newY - a * newX;
            Console.WriteLine($"a = {a}, b = {b}");
            var answers = new List<double>();
            for (var j = 0; j < x.Count; j++)
            {
                answers.Add(a * x[j] + b <= y[j] ? 1 : -1);
            }

            i = 0;
            var inputs = y.Select(e => new double[] { i, y[i++] }).ToArray();
            foreach (var e in x)
            {
                Console.Write(e + " ");
            }
            foreach (var e in y)
            {
                Console.Write(e + " ");
            }

            var model = new SinglePerceptron(2, 3134.8901260195853);
            model.Fit(inputs, answers.ToArray());
        }
    }
}