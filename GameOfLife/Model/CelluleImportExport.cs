using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace GameOfLife.Model
{
    class CelluleImportExport
    {
        /// <summary>
        /// Permet d'exporter un paterne de cellule précis depuis le jeux
        /// </summary>
        /// <param name="cellules">Liste des cellule du jeux</param>
        /// <param name="filePath">Extroi ou Créer le fichier</param>
        public void ExportCellule(List<Cellule> cellules, string filePath)
        {
            string gameString = "<FileGameOfLifeXavier>\n";
            // Ajoute Le nombre de cellule en X
            gameString += (cellules.Max(cel => cel.CoordonneCellule.CoordonneAbsolue.Item1) + 1) +";";
            //Ajoute Le nombre de cellule en Y
            gameString += (cellules.Max(cel => cel.CoordonneCellule.CoordonneAbsolue.Item2) + 1) + ";";
            gameString += cellules[0].CoordonneCellule.CoefficientConversion;
            gameString += "\n";

            foreach (Cellule cellule in cellules)
            {
                gameString += cellule.IsVivante.ToString() + ";";
            }
            gameString = gameString.Remove(gameString.Length - 1);

            using (StreamWriter sw = new(filePath))
            {
                sw.WriteLine(gameString);
            }
        }

        /// <summary>
        /// Méthode pour importer une grille de jeux avec un fichier créer a cette effet
        /// </summary>
        /// <param name="filePath">Le fichier a utiliser pour faire la grille</param>
        /// <returns>Une grille de cellules rempli</returns>
        public void ImportCellule(string filePath, CelluleHelper celluleHelperGrille)
        {
            using (StreamReader sr = new(filePath))
            {
                string verif = sr.ReadLine().Trim();
                if (verif == "<FileGameOfLifeXavier>")
                {
                    // Prendre la première ligne qui contient les information sur les cellule
                    string[] infoGrille = sr.ReadLine().Split(";");
                    // Prendre la seconde ligne qui contient toute les information sur les cellule si il sont vivante ou morte
                    string[] infoCelluleVivante = sr.ReadLine().Split(";");

                    bool[] infoCelluleVivanteBool = new bool[infoCelluleVivante.Length];
                    for (int i = 0; i < infoCelluleVivante.Length; i++)
                    {
                        infoCelluleVivanteBool[i] = Convert.ToBoolean(infoCelluleVivante[i]);
                    }

                    int tailGrilleX = Convert.ToInt32(infoGrille[0]);
                    int tailGrilleY = Convert.ToInt32(infoGrille[1]);
                    double coeficiantConvertion = double.Parse("20.21515116813583135");
                    celluleHelperGrille.GrateGameFromFile(coeficiantConvertion,tailGrilleX,tailGrilleY, infoCelluleVivanteBool);
                }
                else
                {
                    MessageBox.Show("Type de fichier inconu");
                }
            }
        }
    }
}
