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

        private readonly double[] _middle;
        private readonly double[] _errors;
        
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

            _middle = new double[inputsCount];
            _errors = new double[inputsCount];
        }

        public int GetErrorsCount(double[][] inputs, double[] answers)
        {
            var errorsCount = 0;
            for (var k = 0; k < inputs.Length; k++)
            {
                CalculateMiddle(inputs[k]);
                var predict = CalculatePredict();
                if (IsPredictWrong(predict, answers[k]))
                    errorsCount++;
            }

            return errorsCount;
        }
        
        public void Study(double[][] inputs, double[] answers)
        {
            var minErrorsCount = int.MaxValue;
            var age = 0;
            
            while (minErrorsCount > 0 && age < _agesCount)
            {
                age++;
                var errorsCount = 0;

                for (var k = 0; k < inputs.Length; k++)
                {
                    var input = inputs[k];
                    var answer = answers[k];
                    CalculateMiddle(input);
                    var predict = CalculatePredict();

                    if (IsPredictWrong(predict, answer))
                    {
                        errorsCount++;
                        
                        var mainError = (predict - answer) * predict * (1 - predict);
                        CalculateErrors(mainError);

                        for (var i = 0; i < _inputsCount; i++)
                        for (var j = 0; j < _inputsCount; j++)
                        {
                            _weightsFirst[i, j] -= _tempo * _errors[j] * input[i];
                        }

                        for (var i = 0; i < _inputsCount; i++)
                            _weightsSecond[i] -= _tempo * mainError * _middle[i];
                    }
                }

                if (errorsCount < minErrorsCount)
                {
                    minErrorsCount = errorsCount;
                    Console.WriteLine($"Age: {age}, errors count {errorsCount}");
                }
            }
        }

        private bool IsPredictWrong(double predict, double answer)
            => predict > 0.5 && answer < 0.5 ||
               predict < 0.5 && answer > 0.5;
        
        private void CalculateErrors(double mainError)
        {
            for (var i = 0; i < _inputsCount; i++)
                _errors[i] = mainError * _weightsSecond[i] * _middle[i] * (1 - _middle[i]);
        }
        
        private void CalculateMiddle(double[] input)
        {
            for (var i = 0; i < _inputsCount; i++)
            for (var j = 0; j < _inputsCount; j++)
                _middle[j] += input[i] * _weightsFirst[i, j];

            for (var i = 0; i < _inputsCount; i++)
                _middle[i] = SigmaFunction(_middle[i]);
        }

        private double CalculatePredict()
        {
            var predict = 0d;
            for (var i = 0; i < _inputsCount; i++)
                predict += _middle[i] * _weightsSecond[i];

            return SigmaFunction(predict);
        }

        private double SigmaFunction(double t) => 
            1 / (1 + Math.Exp(-t));
    }
}