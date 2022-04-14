using GameOfLife.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameOfLife.ViewModel
{
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

        const int MIN_TAIL_CELLULE = 20;
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
        /// Représante la tail du canevas de jeux
        /// </summary>
        private int canvaTail;


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
        /// Représante la tail du canevas de jeux
        /// </summary>
        public int CanvaTail { get { return CanvaTail; } set { CanvaTail = value; ValeurChanger(); } }
        /// <summary>
        /// Retourne la liste de cellule du cellule Helper
        /// </summary>
        public ObservableCollection<Cellule> ListeCellues { get { return celluleHelper.Cellules; } }
        #endregion

        #region CommandRelais




        #endregion
        public ViewModelGameOfLife(int nbCelluleX,int nbCelluleY)
        {
            InisializeJeu(nbCelluleX, nbCelluleY);
        }

        #region Methode de démarage

        private void InisializeJeu(int nbCelluleX,int nbCelluleY)
        {
            int nbCelluleMax = Math.Max(nbCelluleX, nbCelluleY);

            double cooeficiantMultiplicateur = 0;

            // Vérification si les cellule ont la taille minimum pour entrer dans l'écran;
            if (nbCelluleMax*MIN_TAIL_CELLULE < MIN_TAIL_CANVAS)
            {
                cooeficiantMultiplicateur = (((double)MIN_TAIL_CANVAS) / nbCelluleMax);
                canvaTail = MIN_TAIL_CANVAS;
            }
            else
            {
                cooeficiantMultiplicateur = MIN_TAIL_CELLULE;
                CanvaTail = nbCelluleMax * MIN_TAIL_CELLULE;
            }

            //Création de la grille
            celluleHelper = new(cooeficiantMultiplicateur, nbCelluleX, nbCelluleY);
        }

        #endregion

    }
}
