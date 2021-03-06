using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Model
{
    /// <summary>
    /// Représente une ensemble de coordonne X et Y
    /// </summary>
    class Coordonne
    {
        #region propriété
        /// <summary>
        /// Nombre qui détermine le nombre de fois le plan est plus gros que 1
        /// </summary>
        public double CoefficientConversion { get; }

        /// <summary>
        /// Représente les coordonné absolu d'un objet sur n'importe quelle plan
        /// </summary>
        public Tuple<int, int> CoordonneAbsolue { get; private set; }

        /// <summary>
        /// Représente les coordonné Relative aux plan actuelle d'un objet
        /// </summary>
        public Tuple<double, double> CoordonneRelative { get { return GetCoordonneRelative(); } }

        #endregion

        /// <summary>
        /// Construt une coordonnée avec les coordonné absolut de l'objet
        /// </summary>
        /// <param name="coefficientConversion">Nombre qui détermine le nombre de fois le plan est plus gros que 1</param>
        /// <param name="coordonneX">Coordonée X en coordonnée absolut</param>
        /// <param name="coordonneY">Coordonnée Y en coordonnée absolut </param>
        public Coordonne(double coefficientConversion, int coordonneX,int coordonneY)
        {
            CoefficientConversion = coefficientConversion;
            CoordonneAbsolue = new(coordonneX, coordonneY);
        }

        #region Méthodes

        /// <summary>
        /// Calculle a partir des Coordonnées absolu d'un objet ses Coordonnées relative
        /// </summary>
        /// <returns>Les coordonnées relative d'un objet</returns>
        public Tuple<double, double> GetCoordonneRelative()
        {
            return new Tuple<double, double>(CoordonneAbsolue.Item1 * CoefficientConversion, CoordonneAbsolue.Item2 * CoefficientConversion);
        }

        #endregion
    }
}
