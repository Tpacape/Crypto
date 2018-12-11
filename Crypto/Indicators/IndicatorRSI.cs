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
    public class IndicatorRSI : Indicator
    {
        // Les données RSI
        public List<Double> RSI { get; set; }


        // Les coupes des lignes RSI.
        public List<Double> COUPE70 { get; set; }
        public List<Double> COUPE50 { get; set; }
        public List<Double> COUPE30 { get; set; }


        /// <summary>
        /// Calcul du RSI.
        /// </summary>
        public IndicatorRSI(Donnees donnees) : base(donnees)
        {
            double[] rsi = new double[ValeursClose.Count];
            int begin;
            int length;

            List<Double> valeursInversees = new List<double>(ValeursClose);
            valeursInversees.Reverse();
            
            RetCode retCode = Core.Rsi(0, valeursInversees.Count - 1, valeursInversees.ToArray(), 14, out begin, out length, rsi);

            RSI = new List<double>(rsi);
            RSI = new List<double>(RSI.Where(X => X != 0));
            RSI.Reverse();

            CalculDerivees();
        }

        /// <summary>
        /// Calcul de la position du rsi en fonction de 30, 50 et 70.
        /// </summary>
        protected override void CalculDerivees()
        {
            COUPE70 = new List<double>();
            COUPE50 = new List<double>();
            COUPE30 = new List<double>();

            for (int i = 0; i < RSI.Count -1; i++)
            {
                // Coupe à la hausse 30.
                if (RSI[i+1] < 30 && RSI[i] > 30)
                {
                    COUPE70.Add(0);
                    COUPE50.Add(0);
                    COUPE30.Add(1);
                }
                // Coupe à la baisse 30.
                else if (RSI[i+1] > 30 && RSI[i] < 30)
                {
                    COUPE70.Add(0);
                    COUPE50.Add(0);
                    COUPE30.Add(-1);
                }
                // Coupe à la hausse 50.
                else if (RSI[i+1] < 50 && RSI[i] > 50)
                {
                    COUPE70.Add(0);
                    COUPE50.Add(1);
                    COUPE30.Add(0);
                }
                // Coupe à la baise 50.
                else if (RSI[i + 1] > 50 && RSI[i] < 50)
                {
                    COUPE70.Add(0);
                    COUPE50.Add(-1);
                    COUPE30.Add(0);
                }
                // Coupe à la hausse 70.
                else if (RSI[i + 1] < 70 && RSI[i] > 70)
                {
                    COUPE70.Add(1);
                    COUPE50.Add(0);
                    COUPE30.Add(0);
                }
                // Coupe à la baisse 70.
                else if (RSI[i + 1] > 70 && RSI[i] < 70)
                {
                    COUPE70.Add(-1);
                    COUPE50.Add(0);
                    COUPE30.Add(0);
                }
                else
                {
                    COUPE70.Add(0);
                    COUPE50.Add(0);
                    COUPE30.Add(0);
                }
            }

            COUPE70.Add(0);
            COUPE50.Add(0);
            COUPE30.Add(0);
        }
    }
}
