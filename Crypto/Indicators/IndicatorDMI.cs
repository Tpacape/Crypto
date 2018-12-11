using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacTec.TA.Library;
using static TicTacTec.TA.Library.Core;

namespace Crypto.Indicators
{
    public class IndicatorDMI : Indicator
    {
        // Les données des MA
        public List<Double> ADX { get; set; }
        public List<Double> PlusDI { get; set; }
        public List<Double> MoinsDI { get; set; }

        public List<Double> DiffPlusDIMoinsDI { get; set; }

        /// <summary>
        /// Calcul du DMI
        /// </summary>
        public IndicatorDMI(Donnees donnees) : base(donnees)
        {

            double[] outputADX = new double[ValeursClose.Count];
            double[] outputPlusDI = new double[ValeursClose.Count];
            double[] outputMoinsDI = new double[ValeursClose.Count];

            int begin;
            int length;

            List<Double> valeursInverseesClose = new List<double>(ValeursClose);
            valeursInverseesClose.Reverse();

            List<Double> valeursInverseesHigh = new List<double>(ValeursHigh);
            valeursInverseesHigh.Reverse();

            List<Double> valeursInverseesLow = new List<double>(ValeursLow);
            valeursInverseesLow.Reverse();

            RetCode retCode = Core.Adx(0, valeursInverseesClose.Count - 1, valeursInverseesHigh.ToArray(), valeursInverseesLow.ToArray(), valeursInverseesClose.ToArray(), 14, out begin, out length, outputADX);

            ADX = new List<double>(outputADX);
            ADX = new List<double>(ADX.Where(X => Math.Abs(X) > Double.Epsilon));
            ADX.Reverse();

            retCode = Core.PlusDI(0, valeursInverseesClose.Count - 1, valeursInverseesHigh.ToArray(), valeursInverseesLow.ToArray(), valeursInverseesClose.ToArray(), 14, out begin, out length, outputPlusDI);

            PlusDI = new List<double>(outputPlusDI);
            PlusDI = new List<double>(PlusDI.Where(X => Math.Abs(X) > Double.Epsilon));
            PlusDI.Reverse();

            retCode = Core.MinusDI(0, valeursInverseesClose.Count - 1, valeursInverseesHigh.ToArray(), valeursInverseesLow.ToArray(), valeursInverseesClose.ToArray(), 14, out begin, out length, outputMoinsDI);

            MoinsDI = new List<double>(outputMoinsDI);
            MoinsDI = new List<double>(MoinsDI.Where(X => Math.Abs(X) > Double.Epsilon));
            MoinsDI.Reverse();

            //CalculDerivees();
        }

        protected override void CalculDerivees()
        {
            DiffPlusDIMoinsDI = new List<double>();
            for (int i = 0; i < PlusDI.Count; i++)
            {
                DiffPlusDIMoinsDI.Add(PlusDI[i] - MoinsDI[i]);
            }
        }
    }
}
