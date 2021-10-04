using System;
using System.Linq;

namespace DataAnalysis.Hw3
{
    public class DoublePerceptron
    {
        private const int InputsCount = 10;
        private readonly double[] _weights;
        private readonly double _tempo;

        public DoublePerceptron(double tempo)
        {
            _tempo = tempo;

            var random = new Random();
            _weights = Enumerable
                .Range(0, InputsCount)
                .Select(_ => random.NextDouble())
                .ToArray();
        }

        private int Predict(double[] input)
        {
            var sum = 0d;
            for (var i = 0; i < InputsCount; i++)
                sum += _weights[i] * input[i];

            return sum > 0 ? 1 : -1;
        }

        public void Fit(double[][] inputs, int[] answers)
        {
            var age = 0;
            var minErrorsCount = int.MaxValue;
            while (age < 100)
            {
                var errorsCount = 0;
                for (var i = 0; i < inputs.Length; i++)
                {
                    var input = inputs[i];
                    if (Predict(input) != answers[i])
                    {
                        errorsCount++;
                        for (var j = 0; j < InputsCount; j++)
                        {
                            _weights[j] += _tempo * input[j] * answers[i];
                        }
                    }
                }

                if (errorsCount < minErrorsCount)
                {
                    minErrorsCount = errorsCount;
                    Console.WriteLine($"Age: {age}, errors count: {errorsCount}");
                }
                age++;
            }
        }
    }
}