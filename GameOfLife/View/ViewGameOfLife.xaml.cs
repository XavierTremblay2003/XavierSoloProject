using GameOfLife.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameOfLife.View
{
    /// <summary>
    /// Logique d'interaction pour ViewGameOfLife.xaml
    /// </summary>
    public partial class ViewGameOfLife : Window
    {
        public ViewGameOfLife()
        {
            ViewModelGameOfLife vmGame = new(10, 10);
            this.DataContext = vmGame;
            InitializeComponent();
        }
    }
}
