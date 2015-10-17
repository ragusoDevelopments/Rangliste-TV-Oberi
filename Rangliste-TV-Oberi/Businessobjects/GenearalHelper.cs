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

        public void prepareTextBoxes(WrapPanel wPAddDisc)
        {
            WrapPanel[] panels = new WrapPanel[2];
            int count = 0;

            foreach(TextBox tB in wPAddDisc.Children.OfType<TextBox>())
            {
                tB.Foreground = Brushes.Black;
            }

            foreach(WrapPanel wP in wPAddDisc.Children.OfType<WrapPanel>())
            {
                panels[count] = wP;
                count++;
            }

            foreach(WrapPanel wP in panels)
            {
                foreach (TextBox tB in wP.Children.OfType<TextBox>())
                {
                    tB.Foreground = Brushes.Black;
                }
            }
        }
    }
}
