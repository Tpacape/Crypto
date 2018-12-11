using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacTec.TA.Library;
using static TicTacTec.TA.Library.Core;

namespace Crypto.Indicators
{
    public class IndicatorMTM : Indicator
    {
        // Les données du MTM.
        public List<Double> MTM { get; set; }

        /// <summary>
        /// Calcul du MTMT
        /// </summary>
        public IndicatorMTM(Donnees donnees) : base(donnees)
        {

            double[] outputMTM = new double[ValeursClose.Count];

            int begin;
            int length;

            List<Double> valeursInverseesClose = new List<double>(ValeursClose);
            valeursInverseesClose.Reverse();


            RetCode retCode = Core.Mom(0, valeursInverseesClose.Count - 1, valeursInverseesClose.ToArray(), 14, out begin, out length, outputMTM);

            MTM = new List<double>(outputMTM);
            MTM = new List<double>(MTM.Where(X => Math.Abs(X) > Double.Epsilon));
            MTM.Reverse();
        }

        protected override void CalculDerivees()
        {
            throw new Exception();
        }
    }
}
