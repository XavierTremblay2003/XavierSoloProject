using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Model
{
    class CelluleImportExport
    {
        public void ExportCellule(List<Cellule> cellules, string filePath)
        {
            string gameString = "";
            // Ajoute Le nombre de cellule en X
            gameString += (cellules.Max(cel => cel.CoordonneCellule.CoordonneAbsolue.Item1) + 1) +";";
            //Ajoute Le nombre de cellule en Y
            gameString += (cellules.Max(cel => cel.CoordonneCellule.CoordonneAbsolue.Item2) + 1);
            gameString += "\n";

            foreach (Cellule cellule in cellules)
            {
                gameString += Convert.ToInt32(cellule.IsVivante) + ";";
            }
            gameString.Remove(gameString.Length - 1);

            using (StreamWriter sw = new(filePath))
            {
                sw.WriteLine(gameString);
            }
        }
    }
}
