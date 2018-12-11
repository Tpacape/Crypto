using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Indicators
{
    /// <summary>
    /// TODO
    /// </summary>
    public class IndicatorFibonacci
    {
        public Double ligne100 { get; set; }
        public Double ligne78 { get => (ligne100 - ligne0) * 0.786; }
        public Double ligne61 { get => (ligne100 - ligne0) * 0.681; }
        public Double ligne50 { get => (ligne100 - ligne0) * 0.5; }
        public Double ligne38 { get => (ligne100 - ligne0) * 0.382; }
        public Double ligne23 { get => (ligne100 - ligne0) * 0.236; }
        public Double ligne0 { get; set; }

        public IndicatorFibonacci(List<Double> min, List<Double> max)
        {
            ligne100 = max.Max();
            ligne0 = min.Min();
        }




    }
}
