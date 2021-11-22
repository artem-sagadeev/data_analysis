using System;
using System.Linq;

namespace DataAnalysis.Hw3
{
    public class DoublePerceptron
    {
        private readonly int _inputsCount;
        private readonly double _tempo;
        private readonly int _agesCount;

        private readonly double[,] _weightsFirst;
        private readonly double[] _weightsSecond;
        
        public DoublePerceptron(int inputsCount, double tempo, int agesCount)
        {
            _tempo = tempo;
            _inputsCount = inputsCount;
            _agesCount = agesCount;
            
            var random = new Random();
            _weightsFirst = new double[_inputsCount, _inputsCount];
            for (var i = 0; i < _inputsCount; i++)
            for (var j = 0; j < _inputsCount; j++)
            {
                _weightsFirst[i, j] = random.NextDouble();
            }

            _weightsSecond = new double[_inputsCount];
            for (var i = 0; i < _inputsCount; i++)
            {
                _weightsSecond[i] = random.NextDouble();
            }
        }

        public double Predict(double[][] inputs, int[] answers)
        {
            var globalError = 0d;
            for (var k = 0; k < inputs.Length; k++)
            {
                var middle = CalculateMiddle(inputs[k]);
                var predict = CalculatePredict(middle);
                var mainError = (predict - answers[k]) * predict * (1 - predict);
                var errors = CalculateErrors(mainError, middle);
                globalError += mainError + errors.Sum();
            }

            return globalError;
        }
        
        public void Study(double[][] inputs, int[] answers)
        {
            var globalError = 0d;
            var age = 0;
            do
            {
                age++;
                globalError = 0d;
                
                for (var k = 0; k < inputs.Length; k++)
                {
                    var input = inputs[k];
                    var answer = answers[k];
                    var middle = CalculateMiddle(input);
                    var predict = CalculatePredict(middle);
                    
                    var mainError = (predict - answer) * predict * (1 - predict);
                    var errors = CalculateErrors(mainError, middle);

                    for (var i = 0; i < _inputsCount; i++)
                    for (var j = 0; j < _inputsCount; j++)
                    {
                        _weightsFirst[i, j] -= _tempo * errors[j] * input[i];
                    }

                    for (var i = 0; i < _inputsCount; i++)
                        _weightsSecond[i] -= _tempo * mainError * middle[i];

                    globalError += mainError + errors.Sum();
                }
            } while (globalError != 0d && age < _agesCount);
        }

        private double[] CalculateErrors(double mainError, double[] middle)
        {
            var errors = new double[_inputsCount];
            for (var i = 0; i < _inputsCount; i++)
                errors[i] = mainError * _weightsSecond[i] * middle[i] * (1 - middle[i]);

            return errors;
        }
        
        private double[] CalculateMiddle(double[] input)
        {
            var middle = new double[_inputsCount];
            for (var i = 0; i < _inputsCount; i++)
            for (var j = 0; j < _inputsCount; j++)
                middle[j] += input[i] * _weightsFirst[i, j];

            for (var i = 0; i < _inputsCount; i++)
                middle[i] = SigmaFunction(middle[i]);

            return middle;
        }

        private double CalculatePredict(double[] middle)
        {
            var predict = 0d;
            for (var i = 0; i < _inputsCount; i++)
                predict += middle[i] * _weightsSecond[i];

            return SigmaFunction(predict);
        }

        private double SigmaFunction(double t) => 
            1 / (1 + Math.Exp(-t));
    }
}