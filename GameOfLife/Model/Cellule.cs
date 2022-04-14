using CommandRelais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameOfLife.Model
{
    /// <summary>
    /// Représente une cellule composant du jeux de la vie avec ses coordonne et son état
    /// </summary>
    class Cellule
    {
        /// <summary>
        /// Coordonnée de la cellulle dans sont plan
        /// </summary>
        public Coordonne CoordonneCellule { get; private set; }
        /// <summary>
        /// Methode effectuer lor du clique sur une cellule
        /// </summary>
        public ICommand CelluleClickEvent { get; set; }



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
        }
    }
}
