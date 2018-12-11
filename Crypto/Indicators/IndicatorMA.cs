using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacTec.TA.Library;
using static TicTacTec.TA.Library.Core;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using System.Windows;

namespace Crypto.Indicators
{
    public class IndicatorMA : Indicator
    {
        // Les données des MA.
        public List<Double> MA7 { get; set; }
        public List<Double> MA25 { get; set; }
        public List<Double> MA99 { get; set; }

        // Les différences entre les MA.
        public List<Double> DIFFMA7MA25 { get; set; }
        public List<Double> DIFFMA7MA99 { get; set; }
        public List<Double> DIFFMA25MA99 { get; set; }


        /// <summary>
        /// Calcul de la moyenne mobile à 7, 25 et 99.
        /// </summary>
        public IndicatorMA(Donnees donnees) : base(donnees)
        {
            double[] output7 = new double[ValeursClose.Count];
            double[] output25 = new double[ValeursClose.Count];
            double[] output99 = new double[ValeursClose.Count];
            int begin;
            int length;

            List<Double> valeursInversees = new List<double>(ValeursClose);
            valeursInversees.Reverse();

            RetCode retCode = Core.Sma(0, valeursInversees.Count - 1, valeursInversees.ToArray(), 7, out begin, out length, output7);

            MA7 = new List<double>(output7);
            MA7 = new List<double>(MA7.Where(X => Math.Abs(X) > Double.Epsilon));
            MA7.Reverse();

            retCode = Core.Sma(0, valeursInversees.Count - 1, valeursInversees.ToArray(), 25, out begin, out length, output25);

            MA25 = new List<double>(output25);
            MA25 = new List<double>(MA25.Where(X => Math.Abs(X) > Double.Epsilon));
            MA25.Reverse();

            retCode = Core.Sma(0, valeursInversees.Count - 1, valeursInversees.ToArray(), 99, out begin, out length, output99);

            MA99 = new List<double>(output99);
            MA99 = new List<double>(MA99.Where(X => Math.Abs(X) > Double.Epsilon));
            MA99.Reverse();

            CalculDerivees();
        }

        /// <summary>
        /// Calcul les différences entre les moyennes mobiles.
        /// </summary>
        protected override void CalculDerivees()
        {
            DIFFMA7MA25 = new List<double>();
            for (int i = 0; i < MA25.Count; i++)
            {
                DIFFMA7MA25.Add(MA7[i] - MA25[i]);
            }

            DIFFMA7MA99 = new List<double>();
            for (int i = 0; i < MA99.Count; i++)
            {
                DIFFMA7MA99.Add(MA7[i] - MA99[i]);
            }

            DIFFMA25MA99 = new List<double>();
            for (int i = 0; i < MA99.Count; i++)
            {
                DIFFMA25MA99.Add(MA25[i] - MA99[i]);
            }
        }
    }
}
