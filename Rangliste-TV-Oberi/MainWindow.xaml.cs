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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rangliste_TV_Oberi
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Businessobjects.Participant participant = new Businessobjects.Participant();

        public bool erfassungIsOpen = false;
        public bool infoIsOpen = false;
        public bool einstellungenIsOpen = false;

        public MainWindow()
        {
            #region initStuff
            InitializeComponent();

            Erfassung erfassung = new Erfassung();
            erfassung.Show();
            erfassungIsOpen = true;
            #endregion

            participant.fillCategoriesTable();
            participant.fillStatusTable();
        }



        private void menuItemInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!infoIsOpen)
            {
                Info info = new Info();
                info.Show();
                infoIsOpen = true;
            }
        }

        private void menuItemErfassng_Click(object sender, RoutedEventArgs e)
        {
            if (!erfassungIsOpen)
            {
                Erfassung erfassung = new Erfassung();
                erfassung.Show();
                erfassungIsOpen = true;
            }
        }

        private void menuItemEinstellungen_Click(object sender, RoutedEventArgs e)
        {
            if (!einstellungenIsOpen)
            {
                Einstellungen einstellungen = new Einstellungen();
                einstellungen.Show();
                einstellungenIsOpen = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void menuItemClose_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }






        private void tBStartnumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tBStartnumber.Foreground.ToString() == "#FF7E7E7E")
            {
                tBStartnumber.Text = "";
                tBStartnumber.Foreground = Brushes.Black;
            }
        }

        private void tBStartnumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if(tBStartnumber.Text == "")
            {
                tBStartnumber.Text = "Startnummer";
                tBStartnumber.Foreground = (SolidColorBrush) new BrushConverter().ConvertFrom("#FF7E7E7E");
            }
        }

        private void tBStartnumber_KeyDown(object sender, KeyEventArgs e)
        {
            int startnumber;
            try
            {
                startnumber = Convert.ToInt32(tBStartnumber.Text);
            }
            catch(Exception)
            {
                return;
            }
            
            RL_Datacontext.Participants part = participant.returnParticipant(startnumber);

            if(e.Key == Key.Enter)
            {
                if (part == null)
                    return;
            }
        }

        
    }
}
