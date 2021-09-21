using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataAnalysis.Hw1
{
    public class Perceptron
    {
        private readonly int _inputsCount;
        private readonly double[] WEIGHTS;
        private readonly double BIAS;
        private readonly double TEMPO;


        public Perceptron(int inputsCount, double bias, double tempo)
        {
            _inputsCount = inputsCount;
            BIAS = bias;
            TEMPO = tempo;

            var random = new Random();
            WEIGHTS = Enumerable
                .Range(0, inputsCount)
                .Select(_ => random.NextDouble())
                .ToArray();
        }


        private async Task<int> Predict(double[] input)
        {
            var sum = BIAS;
            for (var i = 0; i < _inputsCount; i++)
                sum += WEIGHTS[i] * input[i];

            var trueResult = 1;
            var falseResult = -1;
            return sum > 0 ? await Task.FromResult(trueResult) : await Task.FromResult(falseResult);
        }


        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: <Predict>d__5")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.Threading.Tasks.Task`1[System.Int32]")]
        public async Task Fit(double[][] inputs, int[] answers)
        {
            var errorsCount = -1;
            var minErrorsCount = inputs.Length;
            while (errorsCount != 0)
            {
                errorsCount = 0;
                for (var i = 0; i < inputs.Length; i++)
                {
                    var input = inputs[i];
                    if (await Predict(input).ConfigureAwait(false) == answers[i]) continue;
                    errorsCount += 1;
                    for (var j = 0; j < _inputsCount; j++)
                    {
                        WEIGHTS[j] += TEMPO * input[j] * answers[i];
                    }
                }

                if (errorsCount < minErrorsCount)
                {
                    minErrorsCount = errorsCount;
                    Console.WriteLine();
                    Console.WriteLine($"Количество ошибок : {minErrorsCount}");
                    Console.Write("ВЕС: ");
                    foreach (var weight in WEIGHTS)
                    {
                        Console.Write(weight + " ");
                    }

                    Console.WriteLine();
                }
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var y = File
                .ReadAllLinesAsync("../../../lines.csv").Result
                .Select(e => double.Parse(e.Split(";")[7].Replace('.', ',')))
                .ToList();
            var x = Enumerable.Range(1, y.Count).ToList();

            var xAverage = x.Average();
            var yAverage = y.Average();
            var xyAverage = y.Select((t, i) => x[i] * t).Sum() / y.Count;
            var x2 = x.Select(e => e * e).Sum() / x.Count;
            var a = (xyAverage - xAverage * yAverage) / (x2 - xAverage * xAverage);
            var b = yAverage - a * xAverage;


            Console.WriteLine($"a = {a}, b = {b}");

            var inputs = y.Select((yi, i) => new[] {i, yi}).ToArray();
            var answers = x.Select((xi, i) => a * xi + b >= y[i] ? 1 : -1).ToArray();

            var model = new Perceptron(2, 123.456d, 0.00001d);
            await model.Fit(inputs, answers);
        }
    }
}