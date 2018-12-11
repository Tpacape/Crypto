using Crypto.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Reseau
{
    public class Neurone
    {

        public Donnees Donnees { get; set; }

        public List<Double> listeCloseNormalise { get; set; }
        public List<Double> listeOpenNormalise { get; set; }
        public List<Double> listeHighNormalise { get; set; }
        public List<Double> listeLowNormalise { get; set; }
        public List<Double> listeVolumeNormalise { get; set; }
        public List<Double> listeEvolutionNormalise { get; set; }
        public List<Double> listeEvolutionExtremumNormalise { get; set; }

        public List<Double> listeMA7Normalise { get; set; }
        public List<Double> listeMA25Normalise { get; set; }
        public List<Double> listeMA99Normalise { get; set; }
        public List<Double> listeDIFFMA7MA25Normalise { get; set; }
        public List<Double> listeDIFFMA7MA99Normalise { get; set; }
        public List<Double> listeDIFFMA25MA99Normalise { get; set; }

        public List<Double> listeMACDNormalise { get; set; }
        public List<Double> listeMACDHistNormalise { get; set; }
        public List<Double> listeMACDSignalNormalise { get; set; }

        public List<Double> listeADXNormalise { get; set; }
        public List<Double> listePlusDINormalise { get; set; }
        public List<Double> listeMoinsDINormalise { get; set; }

        public List<Double> listeBOLLMANormalise { get; set; }
        public List<Double> listeBOLLINFNormalise { get; set; }
        public List<Double> listeBOLLSUPNormalise { get; set; }
        public List<Double> listeBOLLDIFFSUPINFNormalise { get; set; }

        public List<Double> listeRSINormalise { get; set; }
        public List<Double> listeRSICOUPE30Normalise { get; set; }
        public List<Double> listeRSICOUPE50Normalise { get; set; }
        public List<Double> listeRSICOUPE70Normalise { get; set; }

        public List<Double> listeMTMNormalise { get; set; }

        /// <summary>
        /// Le constructeur des données pour le réseau de neurones.
        /// </summary>
        /// <param name="donnees"> Les données. </param>
        public Neurone(Donnees donnees)
        {
            Donnees = donnees;

            NormalisationChandeliers(donnees.Chandeliers);

            NormalisationMA(donnees.MA);

            NormalisationMACD(donnees.MACD);

            NormalisationDMI(donnees.DMI);

            NormalisationBOLL(donnees.BOLL);

            NormalisationRSI(donnees.RSI);

            NormalisationMTM(donnees.MTM);
        }

        /// <summary>
        /// Normalise l'ensemble des données des chandeliers.
        /// </summary>
        /// <param name="donnees"> Les chandeliers. </param>
        private void NormalisationChandeliers(List<Chandelier> donnees)
        {
            List<Double> open = new List<double>();
            List<Double> high = new List<double>();
            List<Double> low = new List<double>();
            List<Double> close = new List<double>();
            List<Double> volume = new List<double>();
            List<Double> evolution = new List<double>();
            List<Double> evolutionExtrem = new List<double>();

            // Pour chaque chandelier on enregistre les valeurs dans les listes
            foreach (Chandelier d in donnees)
            {
                open.Add(Double.Parse(d.open));
                high.Add(Double.Parse(d.high));
                low.Add(Double.Parse(d.low));
                close.Add(Double.Parse(d.close));
                volume.Add(Double.Parse(d.volume));

                evolution.Add(Double.Parse(d.evolution));
                evolutionExtrem.Add(Double.Parse(d.evolutionExtrema));
            }

            listeCloseNormalise = new List<double>(NormaliserDonnees(close));
            listeOpenNormalise = new List<double>(NormaliserDonnees(open));
            listeHighNormalise = new List<double>(NormaliserDonnees(high));
            listeLowNormalise = new List<double>(NormaliserDonnees(low));
            listeVolumeNormalise = new List<double>(NormaliserDonnees(volume));

            listeEvolutionNormalise = new List<double>(NormaliserDonnees(evolution));
            listeEvolutionExtremumNormalise = new List<double>(NormaliserDonnees(evolutionExtrem));
        }

        /// <summary>
        /// Normalise l'ensemble des données MA.
        /// </summary>
        /// <param name="donnees"> Les MA. </param>
        private void NormalisationMA(IndicatorMA donnees)
        {
            listeMA7Normalise = new List<double>(NormaliserDonnees(donnees.MA7));
            listeMA25Normalise = new List<double>(NormaliserDonnees(donnees.MA25));
            listeMA99Normalise = new List<double>(NormaliserDonnees(donnees.MA99));

            listeDIFFMA7MA25Normalise = new List<double>(NormaliserDonnees(donnees.DIFFMA7MA25));
            listeDIFFMA7MA99Normalise = new List<double>(NormaliserDonnees(donnees.DIFFMA7MA99));
            listeDIFFMA25MA99Normalise = new List<double>(NormaliserDonnees(donnees.DIFFMA25MA99));
        }

        /// <summary>
        /// Normalise l'ensemble des données MACD.
        /// </summary>
        /// <param name="donnees"> Le MACD. </param>
        private void NormalisationMACD(IndicatorMACD donnees)
        {
            listeMACDNormalise = new List<double>(NormaliserDonnees(donnees.MACD));
            listeMACDHistNormalise = new List<double>(NormaliserDonnees(donnees.MACDHist));
            listeMACDSignalNormalise = new List<double>(NormaliserDonnees(donnees.MACDSignal));
        }

        /// <summary>
        /// Normalise l'ensemble des données DMI.
        /// </summary>
        /// <param name="donnees"> Le DMI. </param>
        private void NormalisationDMI(IndicatorDMI donnees)
        {
            listeADXNormalise = new List<double>(NormaliserDonnees(donnees.ADX));
            listePlusDINormalise = new List<double>(NormaliserDonnees(donnees.PlusDI));
            listeMoinsDINormalise = new List<double>(NormaliserDonnees(donnees.MoinsDI));
        }

        /// <summary>
        /// Normalise l'ensemble des données BOLL.
        /// </summary>
        /// <param name="donnees"> Le BOLL. </param>
        private void NormalisationBOLL(IndicatorBOLL donnees)
        {
            listeBOLLMANormalise = new List<double>(NormaliserDonnees(donnees.BOLLMID));
            listeBOLLINFNormalise = new List<double>(NormaliserDonnees(donnees.BOLLINF));
            listeBOLLSUPNormalise = new List<double>(NormaliserDonnees(donnees.BOLLSUP));

            listeBOLLDIFFSUPINFNormalise = new List<double>(NormaliserDonnees(donnees.DIFFSUPINF));
        }

        /// <summary>
        /// Normalise l'ensemble des données RSI.
        /// </summary>
        /// <param name="donnees"> Le RSI. </param>
        private void NormalisationRSI(IndicatorRSI donnees)
        {
            listeRSINormalise = new List<double>(NormaliserDonnees(donnees.RSI));

            listeRSICOUPE30Normalise = new List<double>(NormaliserDonnees(donnees.COUPE30));
            listeRSICOUPE50Normalise = new List<double>(NormaliserDonnees(donnees.COUPE50));
            listeRSICOUPE70Normalise = new List<double>(NormaliserDonnees(donnees.COUPE70));
        }

        /// <summary>
        /// Normalise l'ensemble des données MTM.
        /// </summary>
        /// <param name="donnees"> Le MTM. </param>
        private void NormalisationMTM(IndicatorMTM donnees)
        {
            listeMTMNormalise = new List<double>(NormaliserDonnees(donnees.MTM));
        }

        /// <summary>
        /// On normalise les données
        /// </summary>
        /// <param name="donnees"></param>
        private IEnumerable<double> NormaliserDonnees(List<Double> donnees, double scaleMin = -1, double scaleMax = 1)
        {
            double valueMax = donnees.Max();
            double valueMin = donnees.Min();
            double valueRange = valueMax - valueMin;
            double scaleRange = scaleMax - scaleMin;

            IEnumerable<double> normalized =
                donnees.Select(i =>
                   ((scaleRange * (i - valueMin))
                       / valueRange)
                   + scaleMin);

            return normalized;
        }

        /// <summary>
        /// Récupère toutes les données pour un indice donné.
        /// </summary>
        /// <param name="indice"> L'indice. </param>
        /// <returns> L'ensemble des valeurs. </returns>
        public List<Double> RecupererToutesDonnees(int indice, int nbPasse = 1)
        {
            List<Double> result = new List<Double>();

            for (int i = 0; i < nbPasse; i++)
            {
                RecupererIndice(result, indice+i);
            }

            return result;
        }

        /// <summary>
        /// Récupère toutes les données pour un indice donné.
        /// </summary>
        /// <param name="indice"> L'indice. </param>
        public void RecupererIndice(List<Double> result, int indice)
        {

            denormaliser();

            result.Add(listeCloseNormalise[indice]);
            result.Add(listeOpenNormalise[indice]);
            result.Add(listeHighNormalise[indice]);
            result.Add(listeLowNormalise[indice]);
            result.Add(listeVolumeNormalise[indice]);

            //result.Add(listeEvolutionNormalise[indice]);
            //result.Add(listeEvolutionExtremumNormalise[indice]);

            //result.Add(listeMA7Normalise[indice]);
            //result.Add(listeMA25Normalise[indice]);
            //result.Add(listeMA99Normalise[indice]);
            //result.Add(listeDIFFMA7MA25Normalise[indice]);
            //result.Add(listeDIFFMA7MA99Normalise[indice]);
            //result.Add(listeDIFFMA25MA99Normalise[indice]);

            //result.Add(listeMACDNormalise[indice]);
            //result.Add(listeMACDHistNormalise[indice]);
            //result.Add(listeMACDSignalNormalise[indice]);

            //result.Add(listeADXNormalise[indice]);
            //result.Add(listePlusDINormalise[indice]);
            //result.Add(listeMoinsDINormalise[indice]);

            //result.Add(listeBOLLMANormalise[indice]);
            //result.Add(listeBOLLINFNormalise[indice]);
            //result.Add(listeBOLLSUPNormalise[indice]);
            //result.Add(listeBOLLDIFFSUPINFNormalise[indice]);

            //result.Add(listeRSINormalise[indice]);
            //result.Add(listeRSICOUPE30Normalise[indice]);
            //result.Add(listeRSICOUPE50Normalise[indice]);
            //result.Add(listeRSICOUPE70Normalise[indice]);

            //result.Add(listeMTMNormalise[indice]);
        }

        /// <summary>
        /// Fonction qui denormalise pour tester.
        /// </summary>
        private void denormaliser ()
        {
            List<Double> open = new List<double>();
            List<Double> high = new List<double>();
            List<Double> low = new List<double>();
            List<Double> close = new List<double>();
            List<Double> volume = new List<double>();
            List<Double> evolution = new List<double>();
            List<Double> evolutionExtrem = new List<double>();
            foreach (Chandelier d in Donnees.Chandeliers)
            {
                open.Add(Double.Parse(d.open));
                high.Add(Double.Parse(d.high));
                low.Add(Double.Parse(d.low));
                close.Add(Double.Parse(d.close));
                volume.Add(Double.Parse(d.volume));

                evolution.Add(Double.Parse(d.evolution));
                evolutionExtrem.Add(Double.Parse(d.evolutionExtrema));
            }
            listeCloseNormalise = new List<double>(close);
            listeOpenNormalise = new List<double>(open);
            listeHighNormalise = new List<double>(high);
            listeLowNormalise = new List<double>(low);
            listeVolumeNormalise = new List<double>(volume);
            listeEvolutionNormalise = new List<double>(evolution);
            listeEvolutionExtremumNormalise = new List<double>(evolutionExtrem);



            listeMA7Normalise = new List<double>(Donnees.MA.MA7);
            listeMA25Normalise = new List<double>(Donnees.MA.MA25);
            listeMA99Normalise = new List<double>(Donnees.MA.MA99);
            listeDIFFMA7MA25Normalise = new List<double>(Donnees.MA.DIFFMA7MA25);
            listeDIFFMA7MA99Normalise = new List<double>(Donnees.MA.DIFFMA7MA99);
            listeDIFFMA25MA99Normalise = new List<double>(Donnees.MA.DIFFMA25MA99);
            

            
            listeMACDNormalise = new List<double>(Donnees.MACD.MACD);
            listeMACDHistNormalise = new List<double>(Donnees.MACD.MACDHist);
            listeMACDSignalNormalise = new List<double>(Donnees.MACD.MACDSignal);



            listeADXNormalise = new List<double>(Donnees.DMI.ADX);
            listePlusDINormalise = new List<double>(Donnees.DMI.PlusDI);
            listeMoinsDINormalise = new List<double>(Donnees.DMI.MoinsDI);



            listeBOLLMANormalise = new List<double>(Donnees.BOLL.BOLLMID);
            listeBOLLINFNormalise = new List<double>(Donnees.BOLL.BOLLINF);
            listeBOLLSUPNormalise = new List<double>(Donnees.BOLL.BOLLSUP);
            listeBOLLDIFFSUPINFNormalise = new List<double>(Donnees.BOLL.DIFFSUPINF);


            listeRSINormalise = new List<double>(Donnees.RSI.RSI);
            listeRSICOUPE30Normalise = new List<double>(Donnees.RSI.COUPE30);
            listeRSICOUPE50Normalise = new List<double>(Donnees.RSI.COUPE50);
            listeRSICOUPE70Normalise = new List<double>(Donnees.RSI.COUPE70);


            listeMTMNormalise = new List<double>(Donnees.MTM.MTM);
        }

    }
}
