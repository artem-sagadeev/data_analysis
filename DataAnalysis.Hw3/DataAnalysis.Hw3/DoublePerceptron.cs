using System;
using System.Linq;

namespace DataAnalysis.Hw3
{
    public class DoublePerceptron
    {
        private readonly int _inputsCount;
        private readonly double _tempo;
        private readonly int _agesCount;

        private readonly double[,] _weightsXtoH;
        private readonly double[] _weightsHtoY;
        
        public DoublePerceptron(int inputsCount, double tempo, int agesCount)
        {
            _tempo = tempo;
            _inputsCount = inputsCount;
            _agesCount = agesCount;
            
            var random = new Random();
            _weightsXtoH = new double[_inputsCount, _inputsCount];
            for (var i = 0; i < _inputsCount; i++)
            for (var j = 0; j < _inputsCount; j++)
            {
                _weightsXtoH[i, j] = random.NextDouble();
            }

            _weightsHtoY = new double[_inputsCount];
            for (var i = 0; i < _inputsCount; i++)
            {
                _weightsHtoY[i] = random.NextDouble();
            }
        }

        private int Predict(double[] input)
        {
            var firstSum = 0d;
            for (var i = 0; i < _inputsCount; i++)
            {
                for (var j = 0; j < _inputsCount; j++)
                    firstSum += _weightsXtoH[i, j] * input[j];

                firstSum = firstSum > 0 ? 1 : -1;
            }

            var secondSum = 0d;
            for (var i = 0; i < _inputsCount; i++)
                secondSum += _weightsHtoY[i] * firstSum;

            return secondSum > 0 ? 1 : -1;
        }

        public void Fit(double[][] inputs, int[] answers)
        {
            var age = 0;
            var minErrorsCount = int.MaxValue;
            while (age < _agesCount)
            {
                var errorsCount = 0;
                for (var k = 0; k < inputs.Length; k++)
                {
                    var input = inputs[k];
                    if (Predict(input) != answers[k])
                    {
                        errorsCount++;
                        for (var i = 0; i < _inputsCount; i++)
                        {
                            for (var j = 0; j < _inputsCount; j++)
                                _weightsXtoH[i, j] += _tempo * input[i] * answers[k];
                            _weightsHtoY[i] += _tempo * input[i] * answers[k];
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