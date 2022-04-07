using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Valeur qui détermine si la cellule est vivanteé
        /// </summary>
        public bool IsVivante { get; set; }

        /// <summary>
        /// Constructeur qui permet de créer une cellule à partir d'un ensemble de coordonner prédéfinie avec un état initialle morte.
        /// </summary>
        /// <param name="coordonneCellule">Représente les coordonnées de la cellule</param>
        public Cellule(Coordonne coordonneCellule)
        {
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
            CoordonneCellule = coordonneCellule;
            IsVivante = isVivante;
        }
    }
}
