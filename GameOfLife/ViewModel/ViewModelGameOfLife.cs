using GameOfLife.Model;
using System;
using System.Collections.Generic;
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
        const int MIN_WIDH_WINDOW = 800;
        const int MIN_HEIGHT_WINDOW = 455;
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
        /// Représante la largeur de la fenètre
        /// </summary>
        private int windowsWidh;
        /// <summary>
        /// Représente la heuteur de la fenètre
        /// </summary>
        private int windowsHeight;
        /// <summary>
        /// Représante la largeur du canevas de jeux
        /// </summary>
        private int canvaWidh;
        /// <summary>
        /// Représente la heuteur du canevas de jeux
        /// </summary>
        private int canvaHeight;


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
        /// Représante la largeur de la fenètre
        /// </summary>
        public int WindowsWidh { get { return windowsWidh; }set { windowsWidh = value;ValeurChanger(); } }
        /// <summary>
        /// Représente la heuteur de la fenètre
        /// </summary>
        public int WindowsHeight { get { return windowsHeight; } set { windowsHeight = value; ValeurChanger(); } }
        /// <summary>
        /// Représante la largeur du canevas de jeux
        /// </summary>
        public int CanvaWidh { get { return canvaWidh; } set { canvaWidh = value; ValeurChanger(); } }
        /// <summary>
        /// Représente la heuteur du canevas de jeux
        /// </summary>
        public int CanvaHeight { get { return canvaHeight; } set { canvaHeight = value; ValeurChanger(); } }

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
            double cooeficiantMultiplicateurX;
            double cooeficiantMultiplicateurY;
            // Vérification si les cellule ont la taille minimum pour entrer dans l'écran;
            //Pour la largeur
            if (nbCelluleX*20 < MIN_WIDH_WINDOW-40)
            {
                cooeficiantMultiplicateurX = ((double)(MIN_WIDH_WINDOW - 40)) / nbCelluleX;
                canvaWidh = (int)cooeficiantMultiplicateurX * nbCelluleX;
                windowsWidh = canvaWidh + 40;
            }
            else
            {
                cooeficiantMultiplicateurX = 20.0;
                canvaWidh = MIN_WIDH_WINDOW - 40;
                windowsWidh = MIN_WIDH_WINDOW;
            }
            //Pour la hauteur
            if (nbCelluleY * 20 < MIN_HEIGHT_WINDOW - 300)
            {
                cooeficiantMultiplicateurY = ((double)(MIN_HEIGHT_WINDOW - 300)) / nbCelluleY;
                canvaHeight = (int)cooeficiantMultiplicateurY * nbCelluleY;
                windowsHeight = canvaHeight + 300;
            }
            else
            {
                cooeficiantMultiplicateurY = 20.0;
                canvaHeight = MIN_HEIGHT_WINDOW - 300;
                windowsHeight = MIN_HEIGHT_WINDOW;
            }

            //Création de la grille
            celluleHelper = new(cooeficiantMultiplicateurX, cooeficiantMultiplicateurY, nbCelluleX, nbCelluleY);
        }

        #endregion

    }
}
