using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication2.reactableObjects;


namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
           // Oscillator osci = new Oscillator(canvas, 300, 300, 75);
           // Filter fil = new Filter(canvas, 500, 500, 75);
        }

        private SmartBoard smartBoard;

        private void ExerciseSDN_Loaded(object sender, RoutedEventArgs e)
        {
            smartBoard = new SmartBoard(canvas);
        }
    }
}
