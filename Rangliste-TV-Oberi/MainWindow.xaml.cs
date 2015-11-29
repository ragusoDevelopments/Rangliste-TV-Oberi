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
using System.IO;


namespace Rangliste_TV_Oberi
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Businessobjects.Participant _participant = new Businessobjects.Participant();
        Businessobjects.Discipline _discipline = new Businessobjects.Discipline();

        public bool erfassungIsOpen = false;
        public bool infoIsOpen = false;
        public bool einstellungenIsOpen = false;

        DateTime dt = DateTime.Now;

        int participantId = 0;

        public MainWindow()
        {
            #region initStuff
            InitializeComponent();

            Erfassung erfassung = new Erfassung();


            erfassung.Show();
            erfassungIsOpen = true;
            #endregion

            _participant.fillCategoriesTable();
            _participant.fillStatusTable();
       }


        private void tBStartnumber_KeyDown(object sender, KeyEventArgs e)
        {
            updateComboBoxes();

            #region checking stuff
            if (e.Key != Key.Enter)
                return;

            int startnumber;
            try
            {
                startnumber = Convert.ToInt32(tBStartnumber.Text);
            }
            catch (Exception)
            {
                return;
            }

            RL_Datacontext.Participants participant = _participant.returnParticipant(startnumber);
            if (participant == null)
                return;
            #endregion

            wPBasicInfo.Visibility = Visibility.Visible;
            wPDisciplineStuff.Visibility = Visibility.Visible;
            participantId = participant.Id;

            #region fillBasicInformation
            tBName.Text = participant.Name;
            tBYear.Text = participant.YearOfBirth.ToString();
            lblCategory.Content = participant.Category;
            cBStatus.SelectedIndex = participant.StatusIndex;

            if (participant.Gender == "male")
                rBMale.IsChecked = true;
            else
                rBFemale.IsChecked = true;
            #endregion

            #region fill categories
            IEnumerable<RL_Datacontext.Results> results = participant.Results;

            foreach (var v in results)
            {
                addDisciplineToUI(v.Discipline, (float)v.Result);
            }
            #endregion
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
            if (tBStartnumber.Text == "")
            {
                tBStartnumber.Text = "Startnummer";
                tBStartnumber.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
            }
        }



        private void updateComboBoxes()
        {
            cBAddDiscipline.Items.Clear();
            cBAddDisciplineSet.Items.Clear();

            foreach (var v in _discipline.returnDisciplines(null))
            {
                cBAddDiscipline.Items.Add(new ComboBoxItem().Content = v.DisciplineName);
            }

            foreach (var v in _discipline.returnDisciplineSets(null))
            {
                cBAddDisciplineSet.Items.Add(new ComboBoxItem().Content = v.Name);
            }

        }

        private void cBAddDiscipline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string disciplineToAdd = "";
            if (cBAddDiscipline.SelectedIndex != -1)
                disciplineToAdd = cBAddDiscipline.SelectedItem.ToString();
            else
                return;

            if (cBAddDiscipline.SelectedIndex != -1 && checkDisciplineExisting(disciplineToAdd))
                addDisciplineToUI(disciplineToAdd, 0);

        }


        private bool checkDisciplineExisting(string disciplineName)
        {
            IEnumerable<WrapPanel> panels = wPDisciplines.Children.OfType<WrapPanel>();

            foreach (WrapPanel wP in panels)
            {
                foreach (Label lbl in wP.Children.OfType<Label>())
                {
                    if (lbl.Content.ToString() == disciplineName)
                        return false;
                }
            }

            return true;
        }

        private void addDisciplineToUI(string diciplineName, float value)
        {
            /*
            <WrapPanel Height="28" Width="407" Background="#FFE2E2E2">
                        <Label Content="Label" Height="23" Width="63"/>
                        <Label Content="Ergebnis:" Height="26" Width="58"/>
                        <TextBox Height="20" TextWrapping="Wrap" Text="TextBox" Width="120" Margin="0,1,0,0"/>
                        <Button Height="28" Width="28" Margin="137,0,0,0" BorderBrush="#FFDDDDDD">
                            <Image Source="Res/delete.png"/>
                        </Button>
                    </WrapPanel>*/

            WrapPanel newWrapPanel = new WrapPanel();
            newWrapPanel.Width = 407;
            newWrapPanel.Height = 28;
            newWrapPanel.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE2E2E2");

            Label newLabel = new Label();
            newLabel.Height = 23;
            newLabel.Width = 63;
            newLabel.Content = diciplineName;
            newLabel.Name = "lblDiscName";
            newWrapPanel.Margin = new Thickness(3.0, 0.0, 0.0, 0.0);

            Label labelResult = new Label();
            labelResult.Height = 26;
            labelResult.Width = 58;
            labelResult.Content = "Resultat";
            labelResult.Margin = new Thickness(3.0, 0.0, 0.0, 0.0);

            TextBox newTextBox = new TextBox();
            newTextBox.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
            newTextBox.Width = 100;
            newTextBox.Height = 20;
            newTextBox.Text = value.ToString();

            //var brush = new ImageBrush();
            //brush.ImageSource = new BitmapImage(new Uri("C:\\Users\\Andrea\\Desktop\\Programme\\C#\\Rangliste TV-Oberi\\Rangliste-TV-Oberi\\Res\\delete.png", UriKind.Relative));

            Button btnDelete = new Button();
            btnDelete.Height = 28;
            btnDelete.Width = 28;
            btnDelete.Margin = new Thickness(137, 0, 0, 0);
            //btnDelete.Background = brush;
            btnDelete.BorderThickness = new Thickness(0);
            btnDelete.Content = "X";
            btnDelete.Click += btnDelete_Click;



            newWrapPanel.Children.Add(newLabel);
            newWrapPanel.Children.Add(labelResult);
            newWrapPanel.Children.Add(newTextBox);
            newWrapPanel.Children.Add(btnDelete);

            wPDisciplines.Children.Add(newWrapPanel);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btnDelete = sender as Button;

            wPDisciplines.Children.Remove((UIElement)btnDelete.Parent);
        }

        private void cBAddDisciplineSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string disciplineSetToAdd = "";
            if (cBAddDisciplineSet.SelectedIndex != -1)
                disciplineSetToAdd = cBAddDisciplineSet.SelectedItem.ToString();
            else
                return;

            RL_Datacontext.DisciplineSet set = _discipline.returnDisciplineSets(disciplineSetToAdd).First();

            IEnumerable<RL_Datacontext.DisciplinesFromSet> discs = set.DisciplinesFromSet;

            foreach (var v in discs)
            {
                if (checkDisciplineExisting(v.Discipline) && cBAddDisciplineSet.SelectedIndex != -1)
                    addDisciplineToUI(v.Discipline, 0);
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string gender;

            if (rBMale.IsChecked == true)
                gender = "male";
            else if (rBFemale.IsChecked == true)
                gender = "female";
            else
                return;

            if (convertValue(tBYear.Text) == -1)
                return;
            int year = (int)convertValue(tBYear.Text);

            if (_participant.updateParticipant(participantId, tBName.Text, year, cBStatus.SelectedIndex, gender, wPDisciplines))
            #region cleanup
            {
                wPBasicInfo.Visibility = Visibility.Hidden;
                wPDisciplines.Children.Clear();
                cBAddDiscipline.SelectedIndex = -1;
                cBAddDisciplineSet.SelectedIndex = -1;
                wPDisciplineStuff.Visibility = Visibility.Hidden;

                tBStartnumber.Text = "Startnummer";
                tBStartnumber.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
            }
            #endregion

        }


        private float convertValue(string value)
        {
            float convertedValue;

            try
            {
                convertedValue = (float)Convert.ToDouble(value);
                return convertedValue;
            }
            catch
            {
                return -1f;
            }
        }

        private void tBYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tBYear.Text.Length == 4 && convertValue(tBYear.Text) != -1 && _participant.getCategory((int)convertValue(tBYear.Text)) != "")
            {
                lblCategory.Content = _participant.getCategory((int)convertValue(tBYear.Text));
            }
        }

        private void menuItemExport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = "Teilnehmer " + dt.Day + "." + dt.Month + "." + dt.Year;
            sfd.DefaultExt = ".csv";
            sfd.Filter = "CSV Dokumente (.csv)|*.csv";

            Nullable<bool> result = sfd.ShowDialog();

            if (result == true)
            {
                string path = sfd.FileName;
                IEnumerable<RL_Datacontext.Participants> participants = _participant.returnParticipants();

                File.Create(path).Close();

                StreamWriter sw = new StreamWriter(path);
                foreach (var v in participants)
                {
                    string germanGender;
                    if (v.Gender == "male")
                        germanGender = "(Knaben)";
                    else
                        germanGender = "(Maedchen)";
                    sw.WriteLine(v.Name + ";" + v.Category + germanGender + ";" + v.YearOfBirth + ";" + v.Startnumber);
                }
                sw.Close();

            }
        }

        private void menuItemMakeGroups_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<RL_Datacontext.Participants> participantsOSMale = _participant.returnParticipants("OS", "male");
            IEnumerable<RL_Datacontext.Participants> participantsMSMale = _participant.returnParticipants("MS", "male");
            IEnumerable<RL_Datacontext.Participants> participantsUSMale = _participant.returnParticipants("US", "male");

            IEnumerable<RL_Datacontext.Participants> participantsOSFemale = _participant.returnParticipants("OS", "female");
            IEnumerable<RL_Datacontext.Participants> participantsMSFemale = _participant.returnParticipants("MS", "female");
            IEnumerable<RL_Datacontext.Participants> participantsUSFemale = _participant.returnParticipants("US", "female");

            List<IEnumerable<RL_Datacontext.Participants>> allParticipants = new List<IEnumerable<RL_Datacontext.Participants>>();
            allParticipants.Add(participantsOSMale);
            allParticipants.Add(participantsMSMale);
            allParticipants.Add(participantsUSMale);

            allParticipants.Add(participantsOSFemale);
            allParticipants.Add(participantsMSFemale);
            allParticipants.Add(participantsUSFemale);

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = "Gruppen " + dt.Year;
            sfd.DefaultExt = ".csv";
            sfd.Filter = "CSV Dokumente (.csv)|*.csv";

            Nullable<bool> result = sfd.ShowDialog();

            if (result == true)
            {
                string path = sfd.FileName;

                File.Create(path).Close();

                StreamWriter sw = new StreamWriter(path);
                foreach (var v in allParticipants)
                {
                    if (v.Count() != 0)
                    {
                        sw.WriteLine("Gruppe " + v.ElementAt(0).Category + " " + v.ElementAt(0).Gender);
                        foreach (var v2 in v)
                        {
                            sw.WriteLine(v2.Name);
                        }
                        sw.WriteLine();
                    }

                }
                sw.Close();
            }


        }

        private void menuItemMakeRating_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = "Rangliste " + dt.Day + "." + dt.Month + "." + dt.Year;
            sfd.DefaultExt = ".csv";
            sfd.Filter = "CSV Dokumente (.csv)|*.csv";

            Nullable<bool> result = sfd.ShowDialog();

            if (result == true)
            {
                string path = sfd.FileName;

                File.Create(path).Close();
                StreamWriter sw = new StreamWriter(path);

                List<IEnumerable<RL_Datacontext.Participants>> allPAarticipants = new List<IEnumerable<RL_Datacontext.Participants>>();
                IEnumerable<RL_Datacontext.Participants> OSMale = _participant.returnOrderedParticipantForRating("male", "OS");
                IEnumerable<RL_Datacontext.Participants> MSMale = _participant.returnOrderedParticipantForRating("male", "MS");
                IEnumerable<RL_Datacontext.Participants> USMale = _participant.returnOrderedParticipantForRating("male", "US");

                IEnumerable<RL_Datacontext.Participants> OSFemale = _participant.returnOrderedParticipantForRating("female", "OS");
                IEnumerable<RL_Datacontext.Participants> MSFemale = _participant.returnOrderedParticipantForRating("female", "MS");
                IEnumerable<RL_Datacontext.Participants> USFemale = _participant.returnOrderedParticipantForRating("female", "US");

                allPAarticipants.Add(OSMale);
                allPAarticipants.Add(MSMale);
                allPAarticipants.Add(USMale);

                allPAarticipants.Add(OSFemale);
                allPAarticipants.Add(MSFemale);
                allPAarticipants.Add(USFemale);

                foreach (var v in allPAarticipants)
                {
                    if (v.Count() != 0)
                    {
                        string germanGender = "";
                        if (v.ElementAt(0).Gender == "male")
                            germanGender = "Knaben";
                        else
                            germanGender = "Maedchen";

                        sw.WriteLine(germanGender + ", Kategorie " + v.ElementAt(0).Category);
                        sw.WriteLine("Rang;Name;Punkte");

                        foreach (var v2 in v)
                        {
                            sw.WriteLine(v2.Rank + ";" + v2.Name + ";" + v2.TotalPoints);
                        }
                        sw.WriteLine();
                    }

                }
                sw.Close();
                System.Diagnostics.Process.Start(path);
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            Info info = App.Current.Windows.OfType<Info>().First();
            Erfassung erfassung = App.Current.Windows.OfType<Erfassung>().First();
            Einstellungen einstellungen = App.Current.Windows.OfType<Einstellungen>().First();

            info.WindowState = this.WindowState;
            erfassung.WindowState = this.WindowState;
            einstellungen.WindowState = this.WindowState;
        }
    }
}
