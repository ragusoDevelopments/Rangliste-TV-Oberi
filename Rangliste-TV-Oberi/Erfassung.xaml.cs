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
        public Erfassung()
        {
            InitializeComponent();
            
        }

        private void tBYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            SolidColorBrush brushGrey = new SolidColorBrush(Color.FromRgb(179, 171, 171));
            SolidColorBrush brushRed = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            bool isNumber = true;

            tBYear.BorderBrush = brushGrey;

            try
            {
                Convert.ToInt32(tBYear.Text);
            }
            catch (Exception)
            {
                isNumber = false;
                tBYear.BorderBrush = brushRed;
            }
            
            if (tBYear.Text.Length == 4 && isNumber)
            {
                cBCategory.SelectedIndex = Businessobjects.Helper.getCategory(Convert.ToInt16(tBYear.Text));
            }
            else
            {
                if (tBYear.Text.Length < 4)
                    cBCategory.SelectedIndex = -1;
            }
        }
    }
}
