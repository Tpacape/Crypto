using Crypto.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;
using Accord.Controls;
using Accord.IO;
using Accord;
using Accord.Statistics.Distributions.Univariate;
using Accord.MachineLearning.Bayes;
using Encog;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Engine.Network.Activation;
using Encog.Neural.Data.Basic;
using Encog.Neural.NeuralData;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Networks.Training;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Util;
using Encog.MathUtil.Error;
using Encog.ML.Train;
using Crypto.Ecritures;
using Encog.Neural.SOM.Training.Neighborhood;

namespace Crypto.Neurones
{
    public class IndicatorEncog
    {
        public Donnees Donnees;

        // Données initiales pour le réseau.
        public BasicNetwork Reseau { get; set; }

        // Données initiales pour le réseau optimal.
        public BasicNetwork ReseauOptimalEntrainement { get; set; }
        public BasicNetwork ReseauOptimalValidation { get; set; }
        public Double ErreurOptimaleEntrainement { get; set; }
        public Double ErreurOptimaleValidation { get; set; }
        public List<Double> ListeErreurEntrainement { get; set; }
        public List<Double> ListeErreurValidation { get; set; }

        // Les sets d'entrainements et de validations.
        IMLDataSet TrainingSet;
        IMLDataSet ValidationSet;

        // Le trainer.
        IMLTrain train;

        // Le nombre de neurones en entré.
        private const int NbNeuronesEntre = 5;
        private const int NbEpochMax = 10000;
        private const int NbFutur = 6;
        public int NbPasse;
        public int NbHidden;
        

        // L'ensemble des données de tests et de validation.
        protected List<List<Double>> DonneesEntreesTest { get; set; }
        protected List<List<Double>> DonneesSortiesTest { get; set; }
        protected List<List<Double>> DonneesEntreesValidation { get; set; }
        protected List<List<Double>> DonneesSortiesValidation { get; set; }


        
        /// <summary>
        /// Le constructeur qui le réseau de neurones.
        /// </summary>
        public IndicatorEncog(int nbPasse, int nbHidden)
        {
            // Initialisation des données. 
            NbPasse = nbPasse;
            NbHidden = nbHidden;

            // Initialisation du réseau.
            ErrorCalculation.Mode = ErrorCalculationMode.RMS;

            Reseau = new BasicNetwork();
            Reseau.AddLayer(new BasicLayer(null, true, NbNeuronesEntre * NbPasse));
            Reseau.AddLayer(new BasicLayer(new ActivationTANH(), true, NbHidden));
            //Reseau.AddLayer(new BasicLayer(new ActivationTANH(), true, NbHidden / 2));
            //Reseau.AddLayer(new BasicLayer(new ActivationTANH(), true, NbHidden / 4));
            //Reseau.AddLayer(new BasicLayer(new ActivationStep(0,0,1), false, 1));
            Reseau.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
            Reseau.Structure.FinalizeStructure();
            Reseau.Reset();

            // Initialisation des listes.
            DonneesEntreesTest = new List<List<double>>();
            DonneesSortiesTest = new List<List<double>>();
            DonneesEntreesValidation = new List<List<double>>();
            DonneesSortiesValidation = new List<List<double>>();


            //// Récupère les données d'entrées et de sorties pour toutes les monnaies.
            Donnees = new Donnees(MonnaieBTC.XRP);
            this.AggregationDonnees(Donnees);

            // Création du set d'entrainement.
            double[][] donneesEntreesTest = this.DonneesEntreesTest.Select(a => a.ToArray()).ToArray();
            double[][] donneesSortiesTest = this.DonneesSortiesTest.Select(a => a.ToArray()).ToArray();
            TrainingSet = new BasicMLDataSet(donneesEntreesTest, donneesSortiesTest);
            
            // Création du set de validation.
            double[][] donneesEntreesValidation = this.DonneesEntreesValidation.Select(a => a.ToArray()).ToArray();
            double[][] donneesSortiesValidation = this.DonneesSortiesValidation.Select(a => a.ToArray()).ToArray();
            ValidationSet = new BasicMLDataSet(donneesEntreesValidation, donneesSortiesValidation);

            // Création du type d'entrainement.
            //train = new Backpropagation(Reseau, TrainingSet, 0.001, 0.01);
            train = new ResilientPropagation(Reseau, TrainingSet);

            // Initialise les listes d'erreurs et les erreurs
            ListeErreurEntrainement = new List<double>();
            ListeErreurValidation = new List<double>();
            ErreurOptimaleEntrainement = 100;
            ErreurOptimaleValidation = 100;
        }

        /// <summary>
        /// Aggrege l'ensemble des données, sorties de tests et de validation de toutes les monnaies.
        /// </summary>
        /// <param name="donnees"> Les données. </param>
        private void AggregationDonnees(Donnees donnees)
        {
            // Définit le nombre max de valeurs utilisable. 
            // On se base sur MA99 car c'est ce qui contiendra le moins de valeurs.
            int nbMaxDeValeurs = donnees.Neurone.listeCloseNormalise.Count ;

            for (int i = 0; i < nbMaxDeValeurs - NbPasse - NbFutur; i++)
            {
                // 2 -> 50% validation. 4 -> 25% validation. 5 -> 20% validation 10 -> 10% validation...
                if (i % 5 == 0)
                {
                    AffectationEntreeSortie(DonneesEntreesValidation, DonneesSortiesValidation, donnees, i);
                }
                else
                {
                    AffectationEntreeSortie(DonneesEntreesTest, DonneesSortiesTest, donnees, i);
                }
            }
        }

        /// <summary>
        /// Affecte entrées et sorties.
        /// </summary>
        /// <param name="entrees"></param>
        /// <param name="sorties"></param>
        /// <param name="indice"></param>
        private void AffectationEntreeSortie(List<List<Double>> entrees, List<List<Double>> sorties, Donnees donnees, int indice)
        {
            // Enregistre les neurones d'entres.
            List<Double> listeEntrees = new List<double>(donnees.Neurone.RecupererToutesDonnees(indice + NbFutur, NbPasse));            
            entrees.Add(new List<double>(listeEntrees));

            // Récupération des closes
            List<Double> d = new List<double>(donnees.Chandeliers.RecupererClose());

            // Enregistre les neurones d'entrée
            Double closeEntree = d[indice + NbFutur];
            
            // Enregistre les neurones de sorties
            Double closeSortie = d[indice];

            // Si la fermeture de N est > à la fermeture de N+1 => baisse
            List<Double> list = new List<double>();
            if (closeEntree > closeSortie)
            {
                //list.Add(-1.0);
                list.Add(0);
            }
            else
            {
                list.Add(1.0);
            }
            sorties.Add(new List<double>(list));
        }

        /// <summary>
        /// Entraine le réseau.
        /// </summary>
        public void EntrainementReseau()
        {
            // Entrainement du réseau.
            double erreurEntrainement = 0;
            double erreurValidation = 0;
            int epoch = 1;

            do
            {
                train.Iteration();

                // Entrainement du réseau.
                erreurEntrainement = Reseau.CalculateError(TrainingSet);
                ListeErreurEntrainement.Add(erreurEntrainement);

                if (erreurEntrainement < ErreurOptimaleEntrainement)
                {
                    ErreurOptimaleEntrainement = erreurEntrainement;
                    ReseauOptimalEntrainement = (BasicNetwork)Reseau.Clone();
                }


                // Validation du réseau.
                erreurValidation = Reseau.CalculateError(ValidationSet);
                ListeErreurValidation.Add(erreurValidation);

                if (erreurValidation < ErreurOptimaleValidation)
                {
                    ErreurOptimaleValidation = erreurValidation;
                    ReseauOptimalValidation = (BasicNetwork)Reseau.Clone();
                }

                epoch++;

            } while (epoch < NbEpochMax);// && erreurEntrainement != 0 && erreurValidation != 0);

            train.FinishTraining();
        }
        

        public void TesterReseau(SauvegardeExcel s, int indice)
        {
            // On récupère les 200 dernières données pour tester.
            Donnees = new Donnees(MonnaieBTC.XRP, "BTC", "5m", "200");

            // Les données
            double[] output = new double[1];
            List<Double> listeInput = Donnees.Neurone.RecupererToutesDonnees(0, NbPasse);
            double[] input = listeInput.ToArray();

            // Erreur sur le réseau.
            double erreur = Reseau.CalculateError(TrainingSet); 
            Reseau.Compute(input, output);
            double prevision = output[0];

            // Erreur sur le réseau optimal entrainement.
            double erreurEntrainement = this.ReseauOptimalEntrainement.CalculateError(TrainingSet);
            ReseauOptimalEntrainement.Compute(input, output);
            double previsionEntrainement = output[0];

            // Erreur sur le réseau optimal validation.
            double erreurValidation = this.ReseauOptimalValidation.CalculateError(ValidationSet);
            ReseauOptimalValidation.Compute(input, output);
            double previsionValidation = output[0];

            s.EcritureDonnees(erreur, prevision, erreurEntrainement, previsionEntrainement, erreurValidation, previsionValidation, indice);
        }


        /// <summary>
        /// Publication des résultats.
        /// </summary>
        public void PublierResultat()
        {
            SauvegardeExcel s = new SauvegardeExcel(NbNeuronesEntre + "_" + NbPasse + "I_" + NbHidden + "H");

            s.EcritureDonnees(ListeErreurEntrainement, ListeErreurValidation);

            s.FermerExcel();
        }
    }
}
