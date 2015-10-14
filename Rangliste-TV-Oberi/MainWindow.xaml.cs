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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rangliste_TV_Oberi
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            Einstellungen einstellungen = new Einstellungen();
            einstellungen.Show();
            einstellungenIsOpen = true;
            #endregion

            Businessobjects.SQLAddAndReturnFunctions.fillCategoriesTable();
            Businessobjects.SQLAddAndReturnFunctions.fillStatusTable();
            listTable();//delete when finished
        }



        //testpurpose
        public void listTable()
        {
            RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();

            IEnumerable<RL_Datacontext.Disciplines> parts = from p in dc.Disciplines
                                                                     select p;

            foreach(var v in parts)
            {
                ListViewItem item = new ListViewItem();
                item.Content = v.ResultIncrF;
                ListView1.Items.Add(item);
            }

            /*
            IEnumerable<RL_Datacontext.FemaleDisciplinePoints> partsF = from p in dc.FemaleDisciplinePoints
                                                                        select p;

            foreach (var v in partsF)
            {
                ListViewItem item = new ListViewItem();
                item.Content = v.Result.ToString() + " " + v.Points.ToString();
                ListView1_Copy.Items.Add(item);
            }
            */
           
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

        
    }
}
