using System;
using System.Linq;

namespace DataAnalysis.Hw2
{
    public class SinglePerceptron
    {
        private readonly int _inputsCount;
        private readonly double[] _weights;
        private readonly double _tempo;

        public SinglePerceptron(int inputsCount, double tempo)
        {
            _inputsCount = inputsCount;
            _tempo = tempo;

            var random = new Random();
            _weights = Enumerable
                .Range(0, inputsCount)
                .Select(_ => random.NextDouble())
                .ToArray();
        }

        private int Predict(int[] input)
        {
            var sum = 0d;
            for (var i = 0; i < _inputsCount; i++)
                sum += _weights[i] * input[i];

            return sum > 0.5 ? 1 : 0;
        }

        public void Fit(int[][] inputs, int[] answers)
        {
            var age = 0;
            var minErrorsCount = inputs.Length;
            while (age < 100)
            {
                var errorsCount = 0;
                for (var i = 0; i < inputs.Length; i++)
                {
                    var input = inputs[i];
                    var prediction = Predict(input);
                    if (prediction != answers[i])
                    {
                        errorsCount += 1;
                    }
                    for (var j = 0; j < _inputsCount; j++)
                    {
                        _weights[j] += _tempo * input[j] * (answers[i] - prediction);
                    }
                }

                minErrorsCount = Math.Min(errorsCount, minErrorsCount);
                age++;
            }
            
            Console.WriteLine($"Input count: {_inputsCount}, right answers percent: {100 - minErrorsCount * 100 / 720}%");
            Console.Write($"Weights: ");
            foreach (var weight in _weights)
            {
                Console.Write(weight + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}