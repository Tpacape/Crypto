using Crypto.Neurones;
using Encog.Neural.Networks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Ecritures
{
    public class SauvegardeEncog
    {
        private const String Path = @"d:\Profiles\ncharbit\Desktop\Resultat\";

        private const String param = "param";

        private const String reseau = "reseau_";

        private const String reseauOptimalEntrainement = "reseauOptimalEntrainement_";

        private const String reseauOptimalValidation = "reseauOptimalValidation_";

        private const String ext = ".eg";

        private int count = 0;

        private int nbPasse = 0;

        private int nbHidden = 0;


        public SauvegardeEncog()
        {
        }

        public void Enregistrement(List<IndicatorEncog> indicatorEncogTest2)
        {
            if (indicatorEncogTest2 == null)
            {
                throw new Exception();
            }

            StreamWriter sw = new StreamWriter(Path + param);
            sw.WriteLine(indicatorEncogTest2.Count);
            sw.WriteLine(indicatorEncogTest2[0].NbPasse);
            sw.WriteLine(indicatorEncogTest2[0].NbHidden);
            sw.Close();

            FileInfo fichier;
            for (int i = 0; i < indicatorEncogTest2.Count; i++)
            {
                fichier = new FileInfo(Path + reseau + i + ext);
                Encog.Persist.EncogDirectoryPersistence.SaveObject(fichier, (BasicNetwork)indicatorEncogTest2[i].Reseau);

                fichier = new FileInfo(Path + reseauOptimalEntrainement + i + ext);
                Encog.Persist.EncogDirectoryPersistence.SaveObject(fichier, (BasicNetwork)indicatorEncogTest2[i].ReseauOptimalEntrainement);

                fichier = new FileInfo(Path + reseauOptimalValidation + i + ext);
                Encog.Persist.EncogDirectoryPersistence.SaveObject(fichier, (BasicNetwork)indicatorEncogTest2[i].ReseauOptimalValidation);
            }
        }

        public List<IndicatorEncog> Chargement()
        {
            String line;
            StreamReader sr = new StreamReader(Path + param);
            line = sr.ReadLine();
            count = int.Parse(line);
            line = sr.ReadLine();
            nbPasse = int.Parse(line);
            line = sr.ReadLine();
            nbHidden = int.Parse(line);
            sr.Close();

            List<IndicatorEncog> indicatorEncogTest2 = new List<IndicatorEncog>();

            FileInfo fichier;
            for (int i = 0; i < count; i++)
            {
                IndicatorEncog item = new IndicatorEncog(nbPasse, nbHidden);

                fichier = new FileInfo(Path + reseau + i + ext);
                if (!fichier.Exists)
                    throw new Exception();
                item.Reseau = (BasicNetwork)(Encog.Persist.EncogDirectoryPersistence.LoadObject(fichier));

                fichier = new FileInfo(Path + reseauOptimalEntrainement + i + ext);
                if (!fichier.Exists)
                    throw new Exception();
                item.ReseauOptimalEntrainement = (BasicNetwork)(Encog.Persist.EncogDirectoryPersistence.LoadObject(fichier));

                fichier = new FileInfo(Path + reseauOptimalValidation + i + ext);
                if (!fichier.Exists)
                    throw new Exception();
                item.ReseauOptimalValidation = (BasicNetwork)(Encog.Persist.EncogDirectoryPersistence.LoadObject(fichier));

                indicatorEncogTest2.Add(item);
            }

            return indicatorEncogTest2;
        }
    }
}
