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
            InitializeComponent();
            Erfassung erfassung = new Erfassung();
            erfassung.Show();
            erfassungIsOpen = true;

            Businessobjects.SQLFunctions.insertParticipant("test3", 2000, "female");
            Businessobjects.SQLFunctions.insertParticipant("test4", 2005, "male");
            showDatabaseTable();

            
        }

        void showDatabaseTable()
        {
            IEnumerable<RL_Datacontext.Participants> parts = Businessobjects.SQLFunctions.listTable();

            foreach(var p in parts)
            {
                ListViewItem item = new ListViewItem();
                item.Content = p.Name;
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
