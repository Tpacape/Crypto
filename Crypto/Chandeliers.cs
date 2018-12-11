using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Crypto
{
    public class Chandeliers : List<Chandelier>
    {
        public Chandeliers(IEnumerable<Chandelier> collection) : base(collection)
        {
        }


        /// <summary>
        /// Récupère tous les éléments open des chandeliers.
        /// </summary>
        /// <returns></returns>
        public List<Double> RecupererOpen()
        {
            List<String> stringOpen = new List<string>(this.Select(X => X.open));

            return ConvertirEnDouble(stringOpen);
        }

        /// <summary>
        /// Récupère tous les éléments close des chandeliers.
        /// </summary>
        /// <returns></returns>
        public List<Double> RecupererClose()
        {
            List<String> stringClose = new List<string>(this.Select(X => X.close));

            return ConvertirEnDouble(stringClose);
        }

        /// <summary>
        /// Récupère tous les éléments high des chandeliers.
        /// </summary>
        /// <returns></returns>
        public List<Double> RecupererHigh()
        {
            List<String> stringHigh = new List<string>(this.Select(X => X.high));

            return ConvertirEnDouble(stringHigh);
        }

        /// <summary>
        /// Récupère tous les éléments low des chandeliers.
        /// </summary>
        /// <returns></returns>
        public List<Double> RecupererLow()
        {
            List<String> stringLow = new List<string>(this.Select(X => X.low));

            return ConvertirEnDouble(stringLow);
        }

        /// <summary>
        /// Convertit tous les éléments des chandeliers.
        /// </summary>
        /// <returns></returns>
        private List<Double> ConvertirEnDouble (List<String> liste)
        {
            List<Double> listeDouble = new List<double>();
            listeDouble = liste.ConvertAll(new Converter<string, double>(Double.Parse));

            return listeDouble;
        }
    }
}
