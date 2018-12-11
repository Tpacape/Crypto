using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacTec.TA.Library;
using static TicTacTec.TA.Library.Core;

namespace Crypto.Indicators
{
    public class IndicatorMACD : Indicator
    {
        // Les données des MACD
        public List<Double> MACD { get; set; }
        public List<Double> MACDSignal { get; set; }
        public List<Double> MACDHist { get; set; }

        public IndicatorMACD(Donnees donnees) : base (donnees)
        {
            double[] outMACD = new double[ValeursClose.Count];
            double[] outMACDSignal = new double[ValeursClose.Count];
            double[] outMACDHist = new double[ValeursClose.Count];
            int begin;
            int length;

            List<Double> valeursInversees = new List<double>(ValeursClose);
            valeursInversees.Reverse();

            RetCode retCode =  Core.Macd(0, valeursInversees.Count - 1, valeursInversees.ToArray(), 12, 26, 9, out begin, out length, outMACD, outMACDSignal, outMACDHist);

            MACD = new List<double>(outMACD);
            MACD = new List<double>(MACD.Where(X => Math.Abs(X) > Double.Epsilon));
            MACD.Reverse();

            MACDSignal = new List<double>(outMACDSignal);
            MACDSignal = new List<double>(MACDSignal.Where(X => Math.Abs(X) > Double.Epsilon));
            MACDSignal.Reverse();

            MACDHist = new List<double>(outMACDHist);
            MACDHist = new List<double>(MACDHist.Where(X => Math.Abs(X) > Double.Epsilon));
            MACDHist.Reverse();
        }

        protected override void CalculDerivees()
        {
            throw new NotImplementedException();
        }
    }
}
