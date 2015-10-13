using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Rangliste_TV_Oberi.Businessobjects
{
    class GenearalHelper
    {
        public void filllBDiscipline(ListBox lBDisciplines)
        {
            RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();
            lBDisciplines.Items.Clear();

            IEnumerable<RL_Datacontext.Disciplines> discs = Businessobjects.SQLAddAndReturnFunctions.returnDisciplines(null);

            foreach (var v in discs)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = v.DisciplineName;

                lBDisciplines.Items.Add(newItem);
            }
        }

        public void cleanupTextBoxes(TextBox[] tBs, string[] texts)
        {
            int count = 0;
            foreach(TextBox tB in tBs)
            {
                tB.Text = texts[count];
                tB.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
                count++;
            }
        }
    }
}
