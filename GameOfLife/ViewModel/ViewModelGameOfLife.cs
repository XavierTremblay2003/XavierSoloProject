using CommandRelais;
using GameOfLife.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace GameOfLife.ViewModel
{
    /// <summary>
    /// Représante le VieewModel pour relier la vue du jeux de la vie aux models
    /// </summary>
    class ViewModelGameOfLife : INotifyPropertyChanged
    {
        #region Proprieter d'événement
        /// <summary>
        /// Supprter de l'événement de changement de proprieter pour avertir la vue
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Méthode pour appeler le PropertyChanged Event avec le nom de la proprieter
        /// </summary>
        /// <param name="sender">Nom de la proprieté, Laisser vide si la méthode est appeler dasn la propriéter elle mème</param>
        private void ValeurChanger([CallerMemberName]string sender = null)
        {
            PropertyChanged?.Invoke(this, new(sender));
        }

        #endregion

        #region Constante
        /// <summary>
        /// Taille minimum qu'une cellule peut avoir dasn l'afichage
        /// </summary>
        const int MIN_TAIL_CELLULE = 20;
        /// <summary>
        /// Taille minimal du canevas pour l'affichage du jeux
        /// </summary>
        const int MIN_TAIL_CANVAS = 350;
        #endregion

        #region Atribut
        /// <summary>
        /// Reprécende la couleur qui remplis les cellules vivante sur la grille
        /// </summary>
        private Color celluleColor;
        /// <summary>
        /// Représente la couleur de la bordure entre les cellule
        /// </summary>
        private Color borderColor;
        /// <summary>
        /// Représente un celluleHelper qui contien toute les cellule dont le jeux a besoin pour fonctionner
        /// </summary>
        private CelluleHelper celluleHelper;
        /// <summary>
        /// Représante la tail en X du canevas de jeux
        /// </summary>
        private int canvaTailX;
        /// <summary>
        /// Représante la tail en Y du canevas de jeux
        /// </summary>
        private int canvaTailY;


        #endregion

        #region Propriete
        /// <summary>
        /// Reprécende la couleur qui remplis les cellules vivante sur la grille
        /// </summary>
        public Color CelluleColor { get { return celluleColor; } set { celluleColor = value;ValeurChanger(); } }
        /// <summary>
        /// Représente la couleur de la bordure entre les cellule
        /// </summary>
        public Color BorderColor { get { return borderColor; } set { borderColor = value; ValeurChanger(); } }
        /// <summary>
        /// Représante la tail en x du canevas en jeux
        /// </summary>
        public int CanvaTailX { get { return canvaTailX; } set { canvaTailX = value; ValeurChanger(); } }
        /// <summary>
        /// Représante la tail en x du canevas en jeux
        /// </summary>
        public int CanvaTailY { get { return canvaTailY; } set { canvaTailY = value; ValeurChanger(); } }
        /// <summary>
        /// Retourne la liste de cellule du cellule Helper
        /// </summary>
        public ObservableCollection<Cellule> ListeCellues { get { return celluleHelper.Cellules; } }
        #endregion

        #region CommandRelais





        #endregion
        /// <summary>
        /// Constructeur du view Model
        /// </summary>
        /// <param name="nbCelluleX">Nombre de cellule en x que vous voulezx pour le heux de la vie</param>
        /// <param name="nbCelluleY">Nombre de cellule en y que vous voulezx pour le heux de la vie</param>
        public ViewModelGameOfLife(int nbCelluleX,int nbCelluleY)
        {
            //Initiallisation des ICommand


            InisializeJeu(nbCelluleX, nbCelluleY);
        }

        #region Methode de démarage
        /// <summary>
        /// Permet de générer une grille de jeux de la vie et de mettre les cellule a la bonne tail
        /// </summary>
        /// <param name="nbCelluleX">nombre de cellules dans la coordonnée x</param>
        /// <param name="nbCelluleY">nombre de cellule dasn la coordonné y</param>
        private void InisializeJeu(int nbCelluleX,int nbCelluleY)
        {
            int nbCelluleMax = Math.Max(nbCelluleX, nbCelluleY);

            double cooeficiantMultiplicateur = 0;

            // Vérification si les cellule ont la taille minimum pour entrer dans l'écran;
            if (nbCelluleMax*MIN_TAIL_CELLULE < MIN_TAIL_CANVAS)
            {
                cooeficiantMultiplicateur = (((double)MIN_TAIL_CANVAS) / nbCelluleMax);
                canvaTailX = MIN_TAIL_CANVAS;
                canvaTailY = MIN_TAIL_CANVAS;
            }
            else
            {
                cooeficiantMultiplicateur = MIN_TAIL_CELLULE;
                CanvaTailX = nbCelluleX * MIN_TAIL_CELLULE;
                CanvaTailY = nbCelluleY * MIN_TAIL_CELLULE;
            }

            //Création de la grille
            celluleHelper = new(cooeficiantMultiplicateur, nbCelluleX, nbCelluleY);
        }

        #endregion

    }
}
