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
    public class IndicatorBOLL : Indicator
    {
        // Les données des MA
        public List<Double> BOLLMID { get; set; }
        public List<Double> BOLLSUP { get; set; }
        public List<Double> BOLLINF { get; set; }


        // La différence entre la borne sup et inf.
        public List<Double> DIFFSUPINF { get; set; }
        


        /// <summary>
        /// Calcul de la moyenne mobile à 7, 25 et 99.
        /// </summary>
        public IndicatorBOLL(Donnees donnees) : base(donnees)
        {
            double[] outputBOLLMID = new double[ValeursClose.Count];
            double[] outputBOLLSUP = new double[ValeursClose.Count];
            double[] outputBOLLINF = new double[ValeursClose.Count];
            int begin;
            int length;

            List<Double> valeursInversees = new List<double>(ValeursClose);
            valeursInversees.Reverse();

            RetCode retCode = Core.Bbands(0, valeursInversees.Count - 1, valeursInversees.ToArray(), 21, 2, 2, MAType.Ema, out begin, out length, outputBOLLSUP, outputBOLLMID, outputBOLLINF);

            BOLLMID = new List<double>(outputBOLLMID);
            BOLLMID = new List<double>(BOLLMID.Where(X => Math.Abs(X) > Double.Epsilon));
            BOLLMID.Reverse();

            BOLLSUP = new List<double>(outputBOLLSUP);
            BOLLSUP = new List<double>(BOLLSUP.Where(X => Math.Abs(X) > Double.Epsilon));
            BOLLSUP.Reverse();

            BOLLINF = new List<double>(outputBOLLINF);
            BOLLINF = new List<double>(BOLLINF.Where(X => Math.Abs(X) > Double.Epsilon));
            BOLLINF.Reverse();

            CalculDerivees();
        }

        /// <summary>
        /// Calcul de la différence borne inférieur/supéerieur.
        /// </summary>
        protected override void CalculDerivees()
        {
            DIFFSUPINF = new List<double>();
            for (int i = 0; i < BOLLINF.Count; i++)
            {
                DIFFSUPINF.Add(BOLLSUP[i] - BOLLINF[i]);
            }
        }
    }
}
