using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Model
{
    /// <summary>
    /// Représante l'interraction entre cellule dans le jeux de la vie
    /// </summary>
    class CelluleHelper
    {
        /// <summary>
        /// Représente l'ensemble des cellule ordonner pour pouvoir ètre afficher dans une vue
        /// </summary>
        public ObservableCollection<Cellule> CellulesView;
        /// <summary>
        /// Représante un tableaux de cellule pour appliquer la logique du jeux de la vie sur lui
        /// </summary>
        private Cellule[,] cellulesLogique;
        /// <summary>
        /// Construct un CelluleHelper avec Cellules pleine d'une grille de cellules
        /// </summary>
        /// <param name="coefficientConversion">Le coefficient de conversion de la grille de cellule</param>
        /// <param name="tailGrilleX">Taille de la grille en X</param>
        /// <param name="tailGrilleY">Taille de la grille en Y</param>
        public CelluleHelper(double coefficientConversion, int tailGrilleX,int tailGrilleY)
        {
            InisialiseGrille(coefficientConversion, tailGrilleX, tailGrilleY);
        }
        /// <summary>
        /// Initialiser une observableCollection de cellule a partir d'un coefficient de Conversion et la taille d'une grille en X et Y
        /// </summary>
        /// <param name="coefficientConversion">Le coefficient de conversion de la grille de cellule</param>
        /// <param name="tailGrilleX">Taille de la grille en X</param>
        /// <param name="tailGrilleY">Taille de la grille en Y</param>
        /// <returns>Une ObservableCollection de cellule avec les coordoner dans la grille</returns>
        private void InisialiseGrille(double coefficientConversion, int tailGrilleX,int tailGrilleY)
        {
            CellulesView = new();
            cellulesLogique = new Cellule[tailGrilleX,tailGrilleY];


            for (int i=0;i < tailGrilleX;i++)
            {
                for (int j = 0; j < tailGrilleY; j++)
                {
                    Coordonne coordonneCellule = new(coefficientConversion, i, j);
                    Cellule cellule = new(coordonneCellule);
                    CellulesView.Add(cellule);
                    cellulesLogique[i, j] = cellule;
                }
            }
        }

        #region Aplique les règle du jeux de la vie

        /// <summary>
        /// Méthode qui applique les règles du jeu de la vie aux cellules de la liste Cellules
        /// </summary>
        public void ApplyRule()
        {
            List<int> nbVoisineAll = new();

            //Prend le nombre de voisine de chanque cellule
            CellulesView.ToList().ForEach(cellule => nbVoisineAll.Add(GetNombreCelluleVivante(GetAllVoisine(cellule))));

            //Applique les règle a chaque cellule avec le nombre de voisine calculer
            foreach ( Cellule cellule in CellulesView)
            {
                int nbVoisineVivante = nbVoisineAll.FirstOrDefault();
                nbVoisineAll.RemoveAt(0);

                if (cellule.IsVivante && nbVoisineVivante < 2)
                {
                    cellule.IsVivante = false;
                }

                if(cellule.IsVivante && nbVoisineVivante > 3)
                {
                    cellule.IsVivante = false;
                }

                if(!cellule.IsVivante && nbVoisineVivante == 3)
                {
                    cellule.IsVivante = true;
                }
            }
        }
        /// <summary>
        /// Calcule le nombre de cellules vivante dans le tableaux
        /// </summary>
        /// <param name="lesCellules">Tableaux de cellules dont on veut savoir le nombre de cellues vivante</param>
        /// <returns>Le nombre de cellule vivante dans le tableaux</returns>
        private int GetNombreCelluleVivante(Cellule[] lesCellules)
        {
            int nbVivante = 0;

            foreach (Cellule cellule in lesCellules)
            {
                if(cellule.IsVivante)
                {
                    nbVivante++;
                }
            }

            return nbVivante;
        }

        #endregion



        #region Verifier les voisinne

        /// <summary>
        /// Retourne tous les cellule aux alentours de la cellule choisi
        /// </summary>
        /// <param name="cellule">Cellule dont on veut les voisines</param>
        /// <returns>Un tableaux de toute les cellules voisinne</returns>
        private Cellule[] GetAllVoisine(Cellule cellule)
        {
            List<Cellule> celluleVoisine = new();
            int coordX = cellule.CoordonneCellule.CoordonneAbsolue.Item1;
            int coordY = cellule.CoordonneCellule.CoordonneAbsolue.Item2;

            int minX;
            int maxX;

            // Regarder si la cellule est sur le bord gauche de la grille et si il depase fixer le bord a 0
            if (coordX - 1 <= 0)
                minX = 0;
            else
                minX = coordX - 1;

            // Regarder si la cellule est sur le bord droit de la grille et si il depase fixer le bord a la valeur maximum du bord
            if (coordX + 1 >= cellulesLogique.GetLength(0))
                maxX = cellulesLogique.GetLength(0)-1; // -1 parce que les tableaux commance a 0
            else
                maxX = coordX + 1;

            // Vérifie les cellule au y-1 de l'originale si il n'existe pas ne les calculle pas
            if(coordY-1 >= 0)
            {
                for (int i = minX; i <= maxX; i++)
                {
                    celluleVoisine.Add(cellulesLogique[i, coordY - 1]);
                }
            }
            // Calcule les cellule aux mème X que l'originale
            for(int i = minX; i <= maxX;i++)
            {
                if (i == coordX) continue;
                celluleVoisine.Add(cellulesLogique[i, coordY]);
            }

            // Vérifie les cellule au y+1 de l'originale si il n'existe pas ne les calculle pas
            if (coordY+1 < cellulesLogique.GetLength(1))
            {
                for (int i = minX; i <= maxX; i++)
                {
                    celluleVoisine.Add(cellulesLogique[i, coordY + 1]);
                }
            }
            return celluleVoisine.ToArray();
        }
        #endregion

    }
}
