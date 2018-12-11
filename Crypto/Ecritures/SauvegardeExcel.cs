using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Crypto.Ecritures
{
    public class SauvegardeExcel
    {
        //private const String Path = @"d:\Profiles\ncharbit\Desktop\BOLL.xlsx";
        private const String Path = @"d:\Profiles\ncharbit\Desktop\Resultat\V.xlsx";
        private object M = System.Reflection.Missing.Value;

        private Microsoft.Office.Interop.Excel.Application appli { get; set; }
        private Microsoft.Office.Interop.Excel.Workbook classeur { get; set; }
        private Microsoft.Office.Interop.Excel.Worksheet feuille { get; set; }


        /// <summary>
        /// Récupère le fichier de sauvegarde.
        /// </summary>
        public SauvegardeExcel(String nomFeuille)
        {
            // Récupération de Excel.
            appli = new Microsoft.Office.Interop.Excel.Application();

            if (appli == null)
            {
                MessageBox.Show("Excel n'est pas installé correctement!!");
                return;
            }

            // Ouverture/Création du fichier.
            if (File.Exists(Path))
            {
                classeur = appli.Workbooks.Open(Path);
            }
            else
            {
                classeur = this.appli.Workbooks.Add();
                classeur.SaveAs(Path);
            }

            // Récupération de la page.
            feuille = appli.Worksheets.Add(Type.Missing, appli.Worksheets[appli.Worksheets.Count], 1, XlSheetType.xlWorksheet) as Worksheet;
            feuille.Name = nomFeuille + new Random().Next(1, 99);
        }


        /// <summary>
        /// Ecriture des données.
        /// </summary>
        /// <param name="donnees"></param>
        public void EcritureDonnees(List<Double> entrainement, List<Double> validation)
        {
            int ligne = 0;
            for (int i = 0; i < entrainement.Count; i=i+1)
            {
                ligne++;
                feuille.Cells[ligne, 1] = entrainement[i];
                feuille.Cells[ligne, 2] = validation[i];
            }


            // On enregistre le dernier numéro au cas ou.
            ligne++;
            feuille.Cells[ligne, 1] = entrainement.Last();
            feuille.Cells[ligne, 2] = validation.Last();
        }

        /// <summary>
        /// Ecriture des données.
        /// </summary>
        /// <param name="donnees"></param>
        public void EcritureDonnees(
            Double reseauErreur, Double reseauPrevision, 
            Double reseauOptimalEntrainementErreur, Double reseauOptimalEntrainementPrevision,
            Double reseauOptimalValidationErreur, Double reseauOptimalValidationPrevision,
            int indice)
        {
            feuille.Cells[indice + 1, 1] = reseauErreur;
            feuille.Cells[indice + 1, 2] = reseauPrevision;

            feuille.Cells[indice + 1, 3] = reseauOptimalEntrainementErreur;
            feuille.Cells[indice + 1, 4] = reseauOptimalEntrainementPrevision;

            feuille.Cells[indice + 1, 5] = reseauOptimalValidationErreur;
            feuille.Cells[indice + 1, 6] = reseauOptimalValidationPrevision;

        }

        /// <summary>
        /// Fermer EXCEL.
        /// </summary>
        public void FermerExcel()
        {
            classeur.Close(true);
        }
    }
}
