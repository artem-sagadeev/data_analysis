using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAnalysis.Hw1
{
    public class SinglePerceptron
    {
        private readonly int _inputsCount;
        private readonly double[] _weights;
        private readonly double _bias;
        private readonly double _tempo;

        public SinglePerceptron(int inputsCount, double bias, double tempo)
        {
            _inputsCount = inputsCount;
            _bias = bias;
            _tempo = tempo;

            var random = new Random();
            _weights = Enumerable
                .Range(0, inputsCount)
                .Select(_ => random.NextDouble())
                .ToArray();
        }

        private int Predict(double[] input)
        {
            var sum = _bias;
            for (var i = 0; i < _inputsCount; i++)
                sum += _weights[i] * input[i];

            return sum > 0 ? 1 : -1;
        }

        public void Fit(double[][] inputs, int[] answers)
        {
            var errorsCount = -1;
            var minErrorsCount = inputs.Length;
            while (errorsCount != 0)
            {
                errorsCount = 0;
                for (var i = 0; i < inputs.Length; i++)
                {
                    var input = inputs[i];
                    if (Predict(input) != answers[i])
                    {
                        errorsCount += 1;
                        for (var j = 0; j < _inputsCount; j++)
                        {
                            _weights[j] += _tempo * input[j] * answers[i];
                        }
                    }
                }

                if (errorsCount < minErrorsCount)
                {
                    minErrorsCount = errorsCount;
                    Console.WriteLine();
                    Console.WriteLine($"Errors count : {minErrorsCount}");
                    Console.Write("Weights: ");
                    foreach (var weight in _weights)
                    {
                        Console.Write(weight + " ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}