using System.Collections.Generic;
using System.Linq;

namespace DataModel
{
    public class DataSet
    {
        private IReadOnlyList<Data> Data { get; }

        public DataSet(string[] lines)
        {
            Data = lines
                .Select((line, i) => new Data(line, i))
                .ToList();
        }

        public double[] GetSetT()
            => Data.Select(model => model.Time).ToArray();

        public double[] GetSetO()
            => Data.Select(model => model.Open).ToArray();
        
        public double[] GetSetH()
            => Data.Select(model => model.High).ToArray();
        
        public double[] GetSetL()
            => Data.Select(model => model.Low).ToArray();
        
        public double[] GetSetC()
            => Data.Select(model => model.Close).ToArray();
    }
}