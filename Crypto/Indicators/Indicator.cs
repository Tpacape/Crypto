using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Indicators
{
    public abstract class Indicator
    {
        protected Donnees Donnees { get; set; }
        protected List<Double> ValeursOpen { get; set; }
        protected List<Double> ValeursClose { get; set; }
        protected List<Double> ValeursHigh { get; set; }
        protected List<Double> ValeursLow { get; set; }

        /// <summary>
        /// Sauvegarde les données lors de l'initialisation d'un indicateur
        /// </summary>
        public Indicator(Donnees donnees)
        {
            Donnees = donnees;
            ValeursOpen = new List<double>(donnees.Chandeliers.RecupererOpen());
            ValeursClose = new List<double>(donnees.Chandeliers.RecupererClose());
            ValeursHigh = new List<double>(donnees.Chandeliers.RecupererHigh());
            ValeursLow = new List<double>(donnees.Chandeliers.RecupererLow());
        }

        /// <summary>
        /// Fonction qui calcule les valeurs dérivées des valeurs recupérées via TA-LIB
        /// </summary>
        protected abstract void CalculDerivees();
    }
}
