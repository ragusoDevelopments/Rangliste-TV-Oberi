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
            #endregion

            listTable();


        }


        void listTable()
        {
            IEnumerable<RL_Datacontext.Categories> cat = Businessobjects.SQLFunctions.listTable();

            ListViewItem item = new ListViewItem();

            foreach(var v in cat)
            {
                item.Content = v.Category;
                ListView1.Items.Add(item);
            }
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
