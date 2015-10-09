﻿using System;
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
        private MainWindow main = (MainWindow)App.Current.MainWindow;

        RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();
        int biggestStartnr;

        public Erfassung()
        {
            InitializeComponent();
            biggestStartnr = Businessobjects.SQLFunctions.checkStartnumbers();
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.lblWarning.Visibility = Visibility.Hidden;

            string gender;
            int yearOfBirth;

            try
            {
                yearOfBirth = Convert.ToInt32(tBYear.Text);
            }
            catch (Exception)
            {
                this.lblWarning.Visibility = Visibility.Visible;
                return;
            }


            if (rBMale.IsChecked == true)
                gender = "male";
            else if (rBFemale.IsChecked == true)
                gender = "female";
            else
                return;

            Businessobjects.SQLFunctions.addParticipant(tBName.Text, gender, yearOfBirth);


            tBName.Text = "";
            tBYear.Text = "";
            rBMale.IsChecked = false;
            rBFemale.IsChecked = false;
            cBStatus.SelectedItem = -1;
            
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            main.erfassungIsOpen = false;
        }

    }
}
