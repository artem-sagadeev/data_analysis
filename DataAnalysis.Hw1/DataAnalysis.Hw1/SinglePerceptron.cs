using System;
using System.Collections.Generic;

namespace DataAnalysis.Hw1
{
    public class SinglePerceptron
    {
        private readonly int _inputsCount;
        private readonly double[] _weights;

        public SinglePerceptron(int inputsCount, double delta)
        {
            _inputsCount = inputsCount;
            _weights = new double[inputsCount];
            var random = new Random();
            for (var i = 0; i < inputsCount; i++)
            {
                _weights[i] = 1;
            }
        }

        private int Predict(double[] input)
        {
            if (input.Length != _inputsCount)
                throw new ArgumentException();

            var sum = 0d;
            for (var i = 0; i < _inputsCount; i++)
            {
                sum += _weights[i] * input[i];
            }

            return sum > 0 ? 1 : -1;
        }

        public void Fit(double[][] input, double[] output)
        {
            var errs = -1;
            var minErrs = input.Length;
            while (errs != 0)
            {
                errs = 0;
                for (var i = 0; i < input.Length; i++)
                {
                    var row = input[i];
                    var prediction = Predict(row);
                    if (prediction * output[i] < 0)
                    {
                        errs += 1;
                        _weights[0] += 0.001d * row[0] * output[i];
                    }
                }
                var temp = minErrs;
                minErrs = Math.Min(errs, minErrs);
                if (temp != minErrs)
                {
                    Console.WriteLine(minErrs);
                    foreach (var weight in _weights)
                    {
                        Console.Write(weight + " ");
                    }
                }
            }
        }
    }
}