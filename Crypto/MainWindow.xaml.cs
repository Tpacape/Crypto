using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using TicTacTec.TA.Library;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using static TicTacTec.TA.Library.Core;
using Crypto.Indicators;
using Crypto.Constantes;
using Crypto.Ecritures;
using Crypto.Neurones;
using MathNet.Numerics.Statistics;
using Crypto.Reseau;
using Encog.Neural.Networks;
using System.Threading;
using System.Diagnostics;

namespace Crypto
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Les données contenant les chandeliers et les valeurs des différents indicateurs.
        /// </summary>
        public Donnees donnees;

        public List<IndicatorEncog> indicatorEncogTest2;

        public SauvegardeExcel sauvegardeExcel;

        public SauvegardeEncog sauvegardeEncog;

        

        public MainWindow()
        {
            InitializeComponent();

            sauvegardeEncog = new SauvegardeEncog();

            this.TestReseauEncogTest2();
        }

        public void TestReseauEncogTest2()
        {
            
            indicatorEncogTest2 = new List<IndicatorEncog>();
            Stopwatch sw = new Stopwatch();
            sw.Start();



            //// On crée 10 réseau qui s'entraine.
            //for (int i = 0; i < 1; i++)
            //{
            //    indicatorEncogTest2.Add(new IndicatorEncogTest2(13, 10));

            //    indicatorEncogTest2[i].EntrainementReseau();
            //}

            // Le passé
            int count = 0;
            //for (int i = 14; i < 18; i = i + 2)
            //{
            // neuronne caché
            //for (int j = 18; j < 48; j++)
            //{
            //indicatorEncogTest2.Add(new IndicatorEncog(1, 1));
            //indicatorEncogTest2[count].EntrainementReseau();
            //indicatorEncogTest2[count].PublierResultat();
            //count++;

            indicatorEncogTest2.Add(new IndicatorEncog(6, 7));
            indicatorEncogTest2[count].EntrainementReseau();
            indicatorEncogTest2[count].PublierResultat();
            //count++;

            //indicatorEncogTest2.Add(new IndicatorEncog(12, 12));
            //indicatorEncogTest2[count].EntrainementReseau();
            //indicatorEncogTest2[count].PublierResultat();
            //count++;

            //indicatorEncogTest2.Add(new IndicatorEncog(16, 16));
            //indicatorEncogTest2[count].EntrainementReseau();
            //indicatorEncogTest2[count].PublierResultat();
            //count++;

            //}
            //}

            sw.Stop();
            MessageBox.Show(sw.Elapsed.ToString());


            //// On test les 10 réseau et enregistre dans excel.
            //SauvegardeExcel s = new SauvegardeExcel(DateTime.Now.ToLongDateString() + " " + DateTime.Now.Hour);
            //for (int i = 0; i < indicatorEncogTest2.Count; i++)
            //{
            //    indicatorEncogTest2[i].TesterReseau(s, i);
            //}
            //s.FermerExcel();


        }



        private void buttonEntrainement_Click(object sender, RoutedEventArgs e)
        {
            int nbReseau = 2;
            indicatorEncogTest2 = new List<IndicatorEncog>();
            List<Thread> th = new List<Thread>();

            // On crée 10 réseau qui s'entraine.
            for (int i = 0; i < nbReseau; i++)
            {
                th.Add(new Thread(Entrainement));
                th[i].Start();
            }

            for (int i = 0; i < nbReseau; i++)
            {
                th[i].Join();
            }

            MessageBox.Show("Fin de l'entrainement.");
        }

        public void Entrainement()
        {
            Console.WriteLine("start");
            IndicatorEncog i = new IndicatorEncog(5, 9);
            i.EntrainementReseau();
            Console.WriteLine("save");
            lock (indicatorEncogTest2)
                indicatorEncogTest2.Add(i);
            Console.WriteLine("end");
        }

        private void buttonTester_Click(object sender, RoutedEventArgs e)
        {
            // On test les 10 réseau et enregistre dans excel.
            SauvegardeExcel s = new SauvegardeExcel(DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + " " + DateTime.Now.Hour + "_" + DateTime.Now.Minute);
            for (int i = 0; i < indicatorEncogTest2.Count; i++)
            {
                indicatorEncogTest2[i].TesterReseau(s, i);
            }
            s.FermerExcel();

            //MessageBox.Show("Fin du test.");
        }

        private void buttonTesterAutomatique_Click(object sender, RoutedEventArgs e)
        {
            do
            {
                if (DateTime.Now.Minute % 5 == 0)
                {
                    this.buttonTester_Click(this, new RoutedEventArgs());

                    // Dors plus d'une minute.
                    System.Threading.Thread.Sleep(100000);
                }
            } while (true);
        }


        private void buttonEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            sauvegardeEncog.Enregistrement(indicatorEncogTest2);

            MessageBox.Show("Fin de l'enregistrement.");
        }

        private void buttonCharger_Click(object sender, RoutedEventArgs e)
        {
            indicatorEncogTest2 = sauvegardeEncog.Chargement();

            MessageBox.Show("Fin du chargement.");
        }
    }
}
