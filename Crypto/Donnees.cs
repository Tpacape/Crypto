using Crypto.Constantes;
using Crypto.Indicators;
using Crypto.Neurones;
using Crypto.Reseau;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Crypto
{
    public class Donnees
    {
        public MonnaieBTC Monnaie { get; set; }
        public String MonnaieReference { get; set; }
        public String Interval { get; set; }
        public String Limit { get; set; }

        public Chandeliers Chandeliers { get; set; }
        public IndicatorMA MA { get; set; }
        public IndicatorMACD MACD { get; set; }
        public IndicatorDMI DMI { get; set; }
        public IndicatorBOLL BOLL { get; set; }
        public IndicatorRSI RSI { get; set; }
        public IndicatorSTOCHRSI STOCHRSI { get; set; }
        public IndicatorMTM MTM { get; set; }


        public Neurone Neurone { get; set; }


        /// <summary>
        /// Constructeur des données. Récupère les Chandeliers et les différents indicateurs.
        /// </summary>
        public Donnees(MonnaieBTC monnaie, String monnaieReference = "BTC", String interval = "1h", String limit = "500")
        {
            // Récupération de la monnaie.
            Monnaie = monnaie;
            MonnaieReference = monnaieReference;
            Interval = interval;
            Limit = limit;

            RecuperationMaxDonnees();

            // Récupération de l'indicateur MA.
            MA = new IndicatorMA(this);

            // Récupération de l'indicateur MACD.
            MACD = new IndicatorMACD(this);

            // Récupération de l'indicateur DMI.
            DMI = new IndicatorDMI(this);

            // Récupération de l'indicateur BOLL.
            BOLL = new IndicatorBOLL(this);

            // Récupération de l'indicateur RSI.
            RSI = new IndicatorRSI(this);

            // Récupération de l'indicateur MTM.
            MTM = new IndicatorMTM(this);

            // Création du réseau de neurones.
            Neurone = new Neurone(this);
        }

        /// <summary>
        /// Récupère les données par groupe de 500 (limite API binance)
        /// </summary>
        private void RecuperationMaxDonnees()
        {
            // Récupération des 500 premières données.
            String donnees = RecuperationDonnees();
            this.FormalisationDonnees(donnees);

            // On cherche les N premières données
            if (Limit != "500")
            {
                return;
            }

            //Récupération des anciennes données.
            int max;
            do
            {
                max = this.Chandeliers.Count;

                long closeTime = long.Parse(this.Chandeliers.Last().openTime.ToString()) - 1;
                String donneesSuite = RecuperationDonnees(closeTime);
                this.FormalisationDonnees(donneesSuite);

            } while (max != this.Chandeliers.Count);

            // Suppréssion des 10% données les plus anciennes
            int aSupprimer = this.Chandeliers.Count / 10;
            this.Chandeliers.RemoveRange(this.Chandeliers.Count - 1 - aSupprimer, aSupprimer);
        }

        /// <summary>
        /// Récupération des données depuis le site internet.
        /// </summary>
        /// <returns> L'ensemble des datas non formalisées. </returns>
        private String RecuperationDonnees(long closeTime = long.MinValue)
        {
            String api = "https://api.binance.com";
            String apiChandelier = "/api/v1/klines";
            String apiParametres = "?";
            String apiParametresEt = "&";
            String apiSymbol = "symbol=";
            String apiInterval = "interval=";
            String apiCloseTime = "endTime=";
            String apiLimit = "limit=";

            String url = api + apiChandelier
                + apiParametres + apiSymbol
                + Monnaie.ToString() + MonnaieReference
                + apiParametresEt + apiInterval + Interval
                + apiParametresEt + apiLimit + Limit;

            if (closeTime != long.MinValue)
            {
                url += apiParametresEt + apiCloseTime + closeTime;
            }

        

            HttpWebRequest requete = (HttpWebRequest)WebRequest.Create(url);
            requete.Method = WebRequestMethods.Http.Get;
            requete.Accept = "application/json";
            try
            {
                WebResponse reponse = requete.GetResponse();
                using (Stream reponseStream = reponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(reponseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                }
            }

            return null;
        }

        /// <summary>
        /// Formalisation des données récupérées.
        /// </summary>
        /// <param name="donnees"> Les données à l'état brut. </param>
        private void FormalisationDonnees(String donnees)
        {
            String[] separator = new string[] { "[", ";", "]" };
            List<String> donneesSeparees = new List<string>(donnees.Split(separator, StringSplitOptions.RemoveEmptyEntries));
            donneesSeparees.RemoveAll(X => X.Equals(","));

            // On inverse les données de sorte à avoir l'élément le plus récent en élément 0.
            donneesSeparees.Reverse();

            if (this.Chandeliers == null)
            {
                // On supprime l'élément de la liste qui est l'instant en cours.
                donneesSeparees.RemoveAt(0);

                // Convertit les strings en chandelier.
                this.Chandeliers = new Chandeliers(ParseStringIntoChandelier(donneesSeparees));
            }
            else
            {
                this.Chandeliers.AddRange(ParseStringIntoChandelier(donneesSeparees));
            }
        }

        /// <summary>
        /// Transforme une donnée String en Chandelier.
        /// </summary>
        /// <param name="donnees"></param>
        /// <returns></returns>
        public List<Chandelier> ParseStringIntoChandelier(List<String> donnees)
        {
            List<Chandelier> chandeliers = new List<Chandelier>();
            List<String> attributs = new List<string>();

            // Pour chaque ligne on crée un chandelier.
            foreach (String d in donnees)
            {
                // On split la ligne en attribut.
                String[] separator = new string[] { ",", "\\" };
                attributs = new List<string>(d.Split(separator, StringSplitOptions.RemoveEmptyEntries));

                // On crée l'objet.
                Chandelier c = new Chandelier();
                c.openTime = Double.Parse(attributs[0]);
                c.open = TransformationCorrecte(attributs[1]);
                c.high = TransformationCorrecte(attributs[2]);
                c.low = TransformationCorrecte(attributs[3]);
                c.close = TransformationCorrecte(attributs[4]);
                c.volume = TransformationCorrecte(attributs[5]);
                c.closeTime = Double.Parse(attributs[6]);
                c.quoteAssetVolume = TransformationCorrecte(attributs[7]);
                c.numberOfTrades = Double.Parse(attributs[8]);
                c.takerBuyBaseAssetVolume = TransformationCorrecte(attributs[9]);
                c.takerBuyQuoteAssetVolume = TransformationCorrecte(attributs[10]);
                c.canBeIgnored = TransformationCorrecte(attributs[11]);

                c.evolution = ((Double.Parse(c.close) - Double.Parse(c.open)) / Double.Parse(c.open)).ToString();
                c.evolutionExtrema = ((Double.Parse(c.high) - Double.Parse(c.low)) / Double.Parse(c.low)).ToString();

                chandeliers.Add(c);
            }

            return chandeliers;
        }

        /// <summary>
        /// Permet de transformer le string correctement.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private String TransformationCorrecte(String s)
        {
            return s.Replace("\\", String.Empty).Replace("\"", String.Empty).Replace(".", ",");
        }
    }
}
