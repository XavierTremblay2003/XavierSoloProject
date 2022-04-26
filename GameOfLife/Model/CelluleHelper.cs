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
        public List<Cellule> CellulesView;

        /// <summary>
        /// Représante un tableaux de cellule pour appliquer la logique du jeux de la vie sur lui
        /// </summary>
        private Cellule[,] cellulesLogique;

        /// <summary>
        /// Représente la liste de toute les cellule avec le status de toute leur voisine
        /// </summary>
        private Dictionary<Cellule, Delegate[]> kVCellulesAvecVoisine;

        /// <summary>
        /// Construct un CelluleHelper avec Cellules pleine d'une grille de cellules
        /// </summary>
        /// <param name="coefficientConversion">Le coefficient de conversion de la grille de cellule</param>
        /// <param name="tailGrilleX">Taille de la grille en X</param>
        /// <param name="tailGrilleY">Taille de la grille en Y</param>
        public CelluleHelper(double coefficientConversion, int tailGrilleX, int tailGrilleY)
        {
            InisialiseGrille(coefficientConversion, tailGrilleX, tailGrilleY);
            LinkCelluleWitchVoisine();
        }
        /// <summary>
        /// Créer une grille avec les information d'un fichier
        /// </summary>
        /// <param name="coefficientConversion">coéficient de convertion du fichier</param>
        /// <param name="tailGrilleX">Taile de la grille en X</param>
        /// <param name="tailGrilleY">Taille de la grille en Y</param>
        public void GrateGameFromFile(double coefficientConversion, int tailGrilleX, int tailGrilleY,bool[] celluleAlive)
        {
            for (int i = 0; i < tailGrilleX; i++)
            {
                for (int j = 0; j < tailGrilleY; j++)
                {
                    Coordonne coordonneCellule = new(coefficientConversion, i, j);
                    Cellule cellule = new(coordonneCellule,celluleAlive[i*j + j]);
                    CellulesView.Add(cellule);
                    cellulesLogique[i, j] = cellule;
                }
            }
            LinkCelluleWitchVoisine();
        }
        /// <summary>
        /// Génère une forme alléatoire de cellule avec une probabiliter de 1/6
        /// </summary>
        public void GenerateFormeAlleatoire()
        {
            Random ran = new();
            foreach (Cellule cellule in CellulesView)
            {
                int Result = ran.Next(0, 6);

                if(Result == 3)
                {
                    cellule.IsVivante = !cellule.IsVivante;
                }
            }
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
            CellulesView = new((tailGrilleX*tailGrilleY));

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

            foreach (KeyValuePair<Cellule,Delegate[]> CelluleAvecVoisine in kVCellulesAvecVoisine)
            {
                nbVoisineAll.Add(GetNombreCelluleVivante(CelluleAvecVoisine.Value));
            }

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
        /// Retourne le nombre de cellule vivante avec les methode IsVivante de chanque cellule voisine
        /// </summary>
        /// <param name="etatVoisines">Methode IsVinvante des cellule voisine</param>
        /// <returns>Le nombre de cellule vivante</returns>
        private int GetNombreCelluleVivante(Delegate[] etatVoisines)
        {
            int nbVivante = 0;

            foreach (Delegate etatVoisine  in etatVoisines)
            {
                if((bool)etatVoisine.DynamicInvoke(null))
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

        #region Assosiation des Is Vivante des voisine a la cellule
        /// <summary>
        /// Lie toute les cellule dans CelluleView avec l'état de ces voisine dasn le dictionnaire kVCelluleAvecVoisine
        /// </summary>
        private void LinkCelluleWitchVoisine()
        {
            kVCellulesAvecVoisine = new Dictionary<Cellule, Delegate[]>();
            // Recherche dasn toute les cellule de la grille
            foreach (Cellule cellule in CellulesView)
            {
                List<Delegate> delegatesIsVivante = new();
                //Recherche toute les voisine de la cellule
                foreach (Cellule Voisine in GetAllVoisine(cellule))
                {
                    delegatesIsVivante.Add(GetGetMethodeIsVivanteCellule(Voisine));
                }
                // Ajoute la cellule avec ces voisine dans le dictionnaire
                kVCellulesAvecVoisine.Add(cellule,delegatesIsVivante.ToArray());
            }
        }

        /// <summary>
        /// Renvoi la vonction Get de la propriéter IsVivante d'une cellule
        /// </summary>
        /// <param name="cellule">La cellule dont on veut la fonction Get</param>
        /// <returns>La référence a la fonction Get de la propriété IsVivante d'une cellule</returns>
        private Func<bool> GetGetMethodeIsVivanteCellule(Cellule cellule)
        {
            Func<bool> fonctionGetCellule = () => (bool)cellule.IsVivante;

            return fonctionGetCellule;
        }


        #endregion

    }
}
