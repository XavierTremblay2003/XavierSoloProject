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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;

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
        /// <summary>
        /// Représente les interraction avec les fichier pour les grille de cellule
        /// </summary>
        private CelluleImportExport celluleImportExport;


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
        public List<Cellule> ListeCellues { get { return celluleHelper.CellulesView; } }
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

        #region Start game
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
                    DateTime TimeAvant = DateTime.Now;
                    celluleHelper.ApplyRule();
                    nbIterration--;
                    ValeurChanger("NbIterration");
                    DateTime TimeApres = DateTime.Now;
                    long tickDiif = TimeApres.Ticks - TimeAvant.Ticks;
                    TimeSpan timeAttente = (TimeSpan.FromTicks((25 * TimeSpan.TicksPerMillisecond) - tickDiif));
                    if(timeAttente > TimeSpan.Zero)
                    {
                        await Task.Delay(timeAttente);
                    }
                    
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

        #region FormePredefini
        /// <summary>
        /// Représente la forme 1 a charger
        /// </summary>
        public ICommand Forme1 { get; set; }
        /// <summary>
        /// Méthode appeler pour chanrger la forme 1
        /// </summary>
        /// <param name="parameter"></param>
        private void From1Execute(object parameter)
        {
            createNewTable("Save/Forme/Forme1.gol");
        }
        /// <summary>
        /// Méthode appeler pour voir si l'on peut chanrger la forme 1
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool From1CanExecute(object parameter)
        {
            return !IsGameStart;
        }

        /// <summary>
        /// Représente la forme 2 a charger
        /// </summary>
        public ICommand Forme2 { get; set; }
        /// <summary>
        /// Méthode appeler pour chanrger la forme 2
        /// </summary>
        /// <param name="parameter"></param>
        private void From2Execute(object parameter)
        {
            createNewTable("Save/Forme/Forme2.gol");
        }
        /// <summary>
        /// Méthode appeler pour voir si l'on peut chanrger la forme 2
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool From2CanExecute(object parameter)
        {
            return !IsGameStart;
        }

        /// <summary>
        /// Représente la forme 3 a charger
        /// </summary>
        public ICommand Forme3 { get; set; }
        /// <summary>
        /// Méthode appeler pour chanrger la forme 3
        /// </summary>
        /// <param name="parameter"></param>
        private void From3Execute(object parameter)
        {
            createNewTable("Save/Forme/Forme3.gol");
        }
        /// <summary>
        /// Méthode appeler pour voir si l'on peut chanrger la forme 3
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool From3CanExecute(object parameter)
        {
            return !IsGameStart;
        }

        #endregion

        #region Forme aléatoire
        /// <summary>
        /// Proprieter pour générer une forme alléatoire
        /// </summary>
        public ICommand CreateFormeAlleatoire { get; set; }
        
        /// <summary>
        /// Methode pour créer la forme alléatoire
        /// </summary>
        /// <param name="parameter"></param>
        private void CreateFormeAlleatoireExecute(object parameter)
        {
            celluleHelper.GenerateFormeAlleatoire();
        }
        /// <summary>
        /// Méthode pour savoir si nous pouvont créer une forme alléatoire
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool CreateFormAlleatoireCanExecute(object parameter)
        {
            return !IsGameStart;
        }

        #endregion

        #region Export Grille
        /// <summary>
        /// commande pour exporter Une grille dasn un fichier .gol 
        /// </summary>
        public ICommand ExportGrille { get; set; }
        /// <summary>
        /// Méthode executer pour exporter un fichier .gol
        /// </summary>
        /// <param name="parameter"></param>
        private void ExportGrilleExecute(object parameter)
        {
            SaveFileDialog fileSave = new();
            CreateBaseFile(fileSave);
            string filePath = fileSave.FileName;
            if(filePath != string.Empty)
            {
                celluleImportExport.ExportCellule(celluleHelper.CellulesView, filePath);
            }
        }
        /// <summary>
        /// Méthode pour voir si on peut executer la méthode pour avoir un fichier .gol
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>true si on peut sinon false</returns>
        private bool ExportGrilleCanExecute(object parameter)
        {
            return !IsGameStart;
        }

        #endregion

        #region Importe Grille
        /// <summary>
        /// Méthode pour importer une grille avec un filePicker
        /// </summary>
        public ICommand ImporteGrille { get; set; }

        private void ImporteGrilleExecute(object parameter)
        {
            OpenFileDialog openFileDialog = new();
            CreateBaseFile(openFileDialog);
            string filePath = openFileDialog.FileName;
            if (filePath != string.Empty)
            {
                celluleImportExport.ImportCellule(filePath,celluleHelper);
                double tail = Math.Max(celluleHelper.NbCelluleX * celluleHelper.CoefficientConversionAcc, celluleHelper.NbCelluleY * celluleHelper.CoefficientConversionAcc);
                CanvaTailX = (int)tail;
                CanvaTailY = (int)tail;

                ValeurChanger(nameof(ListeCellues));

            }
        }

        private bool ImporteGrilleCanExecute(object parameter)
        {
            return !IsGameStart;
        }

        #endregion


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
            CreateFormeAlleatoire = new CommandeRelais(CreateFormeAlleatoireExecute, CreateFormAlleatoireCanExecute);
            ExportGrille = new CommandeRelais(ExportGrilleExecute, ExportGrilleCanExecute);
            ImporteGrille = new CommandeRelais(ImporteGrilleExecute, ImporteGrilleCanExecute);
            Forme1 = new CommandeRelais(From1Execute, From1CanExecute);
            Forme2 = new CommandeRelais (From2Execute, From2CanExecute);
            Forme3 = new CommandeRelais(From3Execute, From3CanExecute);

            // Initialliser les variable du Vm
            celluleImportExport = new();
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

        #region Méthode réutiliser
        /// <summary>
        /// génère une nouvelle grille depuis un fichier dasn filePatch
        /// </summary>
        /// <param name="filePath">Emplacement du fichier a entrer</param>
        public void createNewTable(string filePath)
        {
            celluleImportExport.ImportCellule(filePath, celluleHelper);
            double tail = Math.Max(celluleHelper.NbCelluleX * celluleHelper.CoefficientConversionAcc, celluleHelper.NbCelluleY * celluleHelper.CoefficientConversionAcc);
            CanvaTailX = (int)tail;
            CanvaTailY = (int)tail;

            ValeurChanger(nameof(ListeCellues));
        }

        /// <summary>
        /// Créer un file dialog avec les information sur les fichier .gol
        /// </summary>
        /// <param name="fileDialog">Le file a faire les modification</param>
        private void CreateBaseFile(FileDialog fileDialog)
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".gameoflife\\Save"));
            fileDialog.Filter = "File ame Of Live (*.gol)|*.gol";
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = ".gol";
            fileDialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".gameoflife\\Save");
            fileDialog.ShowDialog(App.Current.MainWindow);
        }

        #endregion

    }
}
