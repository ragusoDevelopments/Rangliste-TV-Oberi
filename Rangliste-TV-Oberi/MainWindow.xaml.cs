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
        public MainWindow()
        {
            InitializeComponent();

            Erfassung erfassung = new Erfassung();
            erfassung.Show();
            erfassungIsOpen = true;
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
            Einstellungen einstellungen = new Einstellungen();
            einstellungen.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
        
    }
}
