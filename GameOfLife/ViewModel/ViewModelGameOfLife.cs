using CommandRelais;
using GameOfLife.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
        /// <summary>
        /// Réprésante si la partie est l'ancer ou non
        /// </summary>
        private bool isGameStart;
        /// <summary>
        /// Représente le nombre d'ithération du jeux
        /// </summary>
        private int nbIterration;


        #endregion

        #region Propriete
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
        /// <summary>
        /// Réprésante si la partie est l'ancer ou non
        /// </summary>
        public bool IsGameStart { get { return isGameStart; } set { isGameStart = value; ValeurChanger(); ValeurChanger("IsCahngenbIteration"); } }
        /// <summary>
        /// Réprésante la posibiliter de changer le nombre t'ittération
        /// </summary>
        public bool CanChangedNbIteration { get { return !isGameStart; } }
        /// <summary>
        /// Représente le nombre d'ithération du jeux
        /// </summary>
        public string NbIterration { get { return nbIterration.ToString(); } set { int.TryParse(value, out nbIterration); ValeurChanger(); } }
        #endregion

        #region CommandRelais
        /// <summary>
        /// Fonction lancer quand on veut lancer le jeux fait changer les cellule d'état
        /// </summary>
        public ICommand StartGame { get; set; }
        /// <summary>
        /// Fonction executer lor du démarage de la partie
        /// </summary>
        /// <param name="parameter">paramète de la fonction non utiliser</param>
        private async void StartGameAsyncExecute(object parameter)
        {
            IsGameStart = true;
            await Task.Run(async () =>
            {
                for (int i = nbIterration; i > 0; i--)
                {
                    celluleHelper.ApplyRule();
                    nbIterration--;
                    ValeurChanger("NbIterration");
                    await Task.Delay(250);
                }
            });
            IsGameStart = false;

        }
        /// <summary>
        /// Fonction pour vérifier si la partie peut ètre jouer
        /// </summary>
        /// <param name="parameter">paramète de la fonction non utiliser</param>
        /// <returns>True si la partie peut être lancé sinon False</returns>
        private bool StartGameCanExecute(object parameter)
        {
            return NbIterration != default(int).ToString() && !isGameStart;
        }



        #endregion
        /// <summary>
        /// Constructeur du view Model
        /// </summary>
        /// <param name="nbCelluleX">Nombre de cellule en x que vous voulezx pour le heux de la vie</param>
        /// <param name="nbCelluleY">Nombre de cellule en y que vous voulezx pour le heux de la vie</param>
        public ViewModelGameOfLife(int nbCelluleX,int nbCelluleY)
        {
            //Initiallisation des ICommand
            StartGame = new CommandeRelais(StartGameAsyncExecute, StartGameCanExecute);


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
