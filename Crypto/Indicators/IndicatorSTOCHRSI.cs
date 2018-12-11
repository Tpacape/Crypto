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
    /// <summary>
    /// TODO
    /// </summary>
    public class IndicatorSTOCHRSI : Indicator
    {
        // Les données des MA
        public List<Double> STOCK_K { get; set; }
        public List<Double> STOCK_D { get; set; }


        public List<Double> DIFFSTOCK_KSTOCK_D { get; set; }


        /// <summary>
        /// Calcul de la moyenne mobile à 7, 25 et 99.
        /// </summary>
        public IndicatorSTOCHRSI(Donnees donnees) : base(donnees)
        {
            double[] stock_K = new double[ValeursClose.Count];
            double[] stock_D = new double[ValeursClose.Count];
            int begin;
            int length;

            double[] rsi = new double[ValeursClose.Count];
            List<Double> valeursInversees = new List<double>(ValeursClose);
            valeursInversees.Reverse();

            RetCode retCode = Core.Rsi(0, valeursInversees.Count - 1, valeursInversees.ToArray(), 14, out begin, out length, rsi);

            List<Double> RSI = new List<double>(rsi);
            RSI = new List<double>(RSI.Where(X => X != 0));
            RSI.Reverse();

            STOCK_K = new List<double>();
            STOCK_D = new List<double>();

            for (int i = 0; i < RSI.Count-15; i++)
            {
                double min = RSI.GetRange(i, 9).Min();
                double max = RSI.GetRange(i, 9).Max();
                double k = 100 * (RSI[i] - min) / (max - min);
                STOCK_K.Add(k);
            }

            Core.Sma(0, STOCK_K.Count - 1, STOCK_K.ToArray(), 3, out begin, out length, stock_D);
            STOCK_D = new List<double>(stock_D);

            CalculDerivees();
        }

        protected override void CalculDerivees()
        {
            DIFFSTOCK_KSTOCK_D = new List<double>();
            for (int i = 0; i < STOCK_D.Count; i++)
            {
                DIFFSTOCK_KSTOCK_D.Add(STOCK_K[i] - STOCK_D[i]);
            }
        }
    }
}
