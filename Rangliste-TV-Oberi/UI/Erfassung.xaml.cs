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


namespace Rangliste_TV_Oberi
{
    /// <summary>
    /// Interaktionslogik für Erfassung.xaml
    /// </summary>
    public partial class Erfassung : Window
    {
        MainWindow main = (MainWindow)App.Current.MainWindow;
        int biggestStartnr;
        public Erfassung()
        {
            InitializeComponent();
            biggestStartnr = Businessobjects.SQLFunctions.checkStartnumbers();
        }


       private void Window_Closed(object sender, EventArgs e)
        {
            main.erfassungIsOpen = false;
        }

       private void btnAdd_Click(object sender, RoutedEventArgs e)
       {
          
       }
    }
}
