using CommandRelais;
using GameOfLife.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GameOfLife.ViewModel
{
    class ViewModelGrilleTail : INotifyPropertyChanged
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
        private void ValeurChanger([CallerMemberName] string sender = null)
        {
            PropertyChanged?.Invoke(this, new(sender));
        }

        #endregion

        #region Attribut

        /// <summary>
        /// Repreésante la taille en X du jeux
        /// </summary>
        int tailX;
        /// <summary>
        /// Représante la taille en y du jeux
        /// </summary>
        int tailY;

        #endregion

        #region Propriete

        /// <summary>
        /// Repreésante la taille en X du jeux
        /// </summary>
        public string TailX { get { return tailX.ToString(); } set { Int32.TryParse(value,out tailX); ValeurChanger(); } }
        /// <summary>
        /// Représante la taille en y du jeux
        /// </summary>
        public string TailY { get { return tailY.ToString(); } set { Int32.TryParse(value, out tailY); ValeurChanger(); } }

        #endregion

        #region Binding Command

        public ICommand StartGrille { get; set; }
        /// <summary>
        /// Méthode qui crée la grille de cellule et la vue avec celle-ci
        /// </summary>
        /// <param name="parameter">NULL</param>
        private void StartGrilleExecute(object parameter)
        {
            ViewGameOfLife jeux = new ViewGameOfLife(new ViewModelGameOfLife(tailX,tailY));
            jeux.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = jeux;
        }
        /// <summary>
        /// Méthode pour vérifier si le Execute peut ce lancé 
        /// </summary>
        /// <param name="parameter">NULL</param>
        /// <returns></returns>
        private bool StartGrilleCanExecute(object parameter)
        {
            return tailX != default && tailY != default;
        }

        #endregion


        public ViewModelGrilleTail()
        {
            StartGrille = new CommandeRelais(StartGrilleExecute, StartGrilleCanExecute);
        }
    }
}
