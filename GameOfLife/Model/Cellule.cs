using CommandRelais;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace GameOfLife.Model
{
    /// <summary>
    /// Représente une cellule composant du jeux de la vie avec ses coordonne et son état
    /// </summary>
    class Cellule : INotifyPropertyChanged
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


        /// <summary>
        /// Coordonnée de la cellulle dans sont plan
        /// </summary>
        public Coordonne CoordonneCellule { get; private set; }
        /// <summary>
        /// Methode effectuer lor du clique sur une cellule
        /// </summary>
        public ICommand CelluleClickEvent { get; set; }
        /// <summary>
        /// Représente la couleur de la cellule quand elle est morte ou vivante Vivante:Noir Morte:Transparent
        /// </summary>
        public Brush CouleurCellule 
        {
            get
            {
                if(IsVivante)
                {
                    return Brushes.Black;
                }
                else
                {
                    return Brushes.Transparent;
                }
            }
        }



        /// <summary>
        /// Valeur qui détermine si la cellule est vivanteé
        /// </summary>
        public bool IsVivante { get; set; }

        /// <summary>
        /// Constructeur qui permet de créer une cellule à partir d'un ensemble de coordonner prédéfinie avec un état initialle morte.
        /// </summary>
        /// <param name="coordonneCellule">Représente les coordonnées de la cellule</param>
        public Cellule(Coordonne coordonneCellule)
        {
            // Déclaration ICommand
            CelluleClickEvent = new CommandeRelais(CelluleClickEventExecute, parameter => true);



            CoordonneCellule = coordonneCellule;
            IsVivante = false;
        }

        /// <summary>
        /// Constructeur qui permet de crée une cellule avec des coordonné prédéfinie et de définir sont état initiale
        /// </summary>
        /// <param name="coordonneCellule">Représente les coordonnées de la cellule</param>
        /// <param name="isVivante">Représente l'état initialle de la cellule</param>
        public Cellule(Coordonne coordonneCellule, bool isVivante)
        {
            // Déclaration ICommand
            CelluleClickEvent = new CommandeRelais(CelluleClickEventExecute, parameter => true);


            CoordonneCellule = coordonneCellule;
            IsVivante = isVivante;
        }
        /// <summary>
        /// Action que fais le click sur la cellule. Rend vivante la cellule cible
        /// </summary>
        /// <param name="parameter">Evenement de click sur la cellule</param>
        private void CelluleClickEventExecute(object parameter)
        {
            if(IsVivante)
            {
                IsVivante = false;
            }
            else
            {
                IsVivante=true;
            }
            ValeurChanger();
            ValeurChanger("CouleurCellule");
        }
    }
}
