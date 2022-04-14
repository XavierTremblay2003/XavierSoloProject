using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        /// Représante toute les cellule du CelluleHelper
        /// </summary>
        public ObservableCollection<Cellule> Cellules;
        /// <summary>
        /// Construct un CelluleHelper avec Cellules pleine d'une grille de cellules
        /// </summary>
        /// <param name="coefficientConversion">Le coefficient de conversion de la grille de cellule</param>
        /// <param name="tailGrilleX">Taille de la grille en X</param>
        /// <param name="tailGrilleY">Taille de la grille en Y</param>
        public CelluleHelper(double coefficientConversion, int tailGrilleX,int tailGrilleY)
        {
            Cellules = InisialiseGrille(coefficientConversion, tailGrilleX, tailGrilleY);
        }
        /// <summary>
        /// Initialiser une observableCollection de cellule a partir d'un coefficient de Conversion et la taille d'une grille en X et Y
        /// </summary>
        /// <param name="coefficientConversion">Le coefficient de conversion de la grille de cellule</param>
        /// <param name="tailGrilleX">Taille de la grille en X</param>
        /// <param name="tailGrilleY">Taille de la grille en Y</param>
        /// <returns>Une ObservableCollection de cellule avec les coordoner dans la grille</returns>
        private ObservableCollection<Cellule> InisialiseGrille(double coefficientConversion, int tailGrilleX,int tailGrilleY)
        {
            ObservableCollection<Cellule> lesCellules = new();

            for(int i=0;i <= tailGrilleX;i++)
            {
                for (int j = 0; j <= tailGrilleY; j++)
                {
                    Coordonne coordonneCellule = new(coefficientConversion, i, j);
                    Cellule cellule = new(coordonneCellule);
                    lesCellules.Add(cellule);
                }
            }
            return lesCellules;
        }

        #region Aplique les règle du jeux de la vie

        /// <summary>
        /// Méthode qui applique les règles du jeu de la vie aux cellules de la liste Cellules
        /// </summary>
        public void ApplyRule()
        {
            foreach (Cellule cellule in Cellules)
            {
                int nbVoisineVivante = GetNombreCelluleVivante(GetAllVoisine(cellule));

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
            Cellule[] lesCellules = Cellules.Where((celluleVoisine) => IsVoisinne(cellule, celluleVoisine)).ToArray();
            return lesCellules;
        }
        /// <summary>
        /// Détermine si la condition pour être aux alentours de la cellule est respecter
        /// </summary>
        /// <param name="cellule">Cellule dont on veut savoir si l'autre est voisines</param>
        /// <param name="celluleVoisine">Cellule dont on veut savoir si elle est la voisine</param>
        /// <returns>Retourne True si les cellule sont voisinne et retourne false si les cellule ne le sont pas</returns>
        private bool IsVoisinne(Cellule cellule,Cellule celluleVoisine)
        {
            // Prise des nombre pour les coordoner de chaque cellule.
            int coordoneXCellule = cellule.CoordonneCellule.CoordonneAbsolue.Item1;
            int coordoneYCellule = cellule.CoordonneCellule.CoordonneAbsolue.Item2;
            int coordoneXVoisine = celluleVoisine.CoordonneCellule.CoordonneAbsolue.Item1;
            int coordoneYVoisine = celluleVoisine.CoordonneCellule.CoordonneAbsolue.Item2;

            // Vérification si les cellule sont dans le bon rang avec leur coordonnée X et Y
            return InRange(coordoneXVoisine, coordoneXCellule - 1, coordoneXCellule + 1) && InRange(coordoneYVoisine, coordoneYCellule - 1, coordoneYCellule + 1);
        }
        /// <summary>
        /// Détermine si un nombre est compris entre deux autre nombre
        /// </summary>
        /// <param name="nombre">Nombre que l'on veut savoir si il est dans l'étendue</param>
        /// <param name="min">Nombre minimum de l'étendue, inclusivement</param>
        /// <param name="max">Nombre maximum de l'étendue, inclusivement</param>
        /// <returns>Retourne True si le nombre est dans l'étendue et False si le nombre n'est pas dasn l'étendue</returns>
        private bool InRange(int nombre, int min, int max)
        {
            return (nombre >= min) && (nombre <= max);
        }

        #endregion

    }
}
