using MiniProjekatHCI.mock_data;
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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Events;

namespace MiniProjekatHCI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool clickedGDPButton = false;
        private bool clickedTreasuryButton = false;
        private MockData md;
        public MainWindow()
        {
            InitializeComponent();
            md = new MockData();
        }

        private void GDPButtonClick(object sender, RoutedEventArgs e)
        {
            clickedGDPButton = true;
            clickedTreasuryButton = false;
            GDPButton.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FF2A4191"));
            GDPButton.BorderThickness = (Thickness)(new ThicknessConverter().ConvertFrom("0, 0, 0, 3"));
            TreasuryButton.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
            TreasuryButton.BorderThickness = (Thickness)(new ThicknessConverter().ConvertFrom("0, 0, 0, 0"));
            secondRow.Height = new GridLength(7, GridUnitType.Star);
            MaturityLabel.Visibility = Visibility.Hidden;
            MaturityComboTr.Visibility = Visibility.Hidden;
            additionalInfo.Visibility = Visibility.Visible;
            deleteComboBoxItem(IntervalCombo, "Daily");
            deleteComboBoxItem(IntervalCombo, "Monthly");
            deleteComboBoxItem(IntervalCombo, "Weekly");
            if (IntervalCombo.Items.Count > 1 || IntervalCombo.Items.Count > 2) return;
            else
            {
                IntervalCombo.Items.Add(createComboBoxItem("Annual"));
                IntervalCombo.Items.Add(createComboBoxItem("Quarterly"));
            }
            md.LoadGDP("REAL_GDP", "annual");
        }

        private void ButtonGDP_MouseEnter(object sender, MouseEventArgs e)
        {
            GDPButton.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FF2A4191"));
            GDPButton.BorderThickness = (Thickness)(new ThicknessConverter().ConvertFrom("0, 0, 0, 3"));
        }

        private void ButtonGDP_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!clickedGDPButton)
            {
                GDPButton.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                GDPButton.BorderThickness = (Thickness)(new ThicknessConverter().ConvertFrom("0, 0, 0, 0"));

            }
        }

        private void TreasuryButtonClick(object sender, RoutedEventArgs e)
        {
            clickedGDPButton = false;
            clickedTreasuryButton = true;
            TreasuryButton.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FF2A4191"));
            TreasuryButton.BorderThickness = (Thickness)(new ThicknessConverter().ConvertFrom("0, 0, 0, 3"));
            GDPButton.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
            GDPButton.BorderThickness = (Thickness)(new ThicknessConverter().ConvertFrom("0, 0, 0, 0"));
            secondRow.Height = new GridLength(7, GridUnitType.Star);
            MaturityLabel.Visibility = Visibility.Visible;
            MaturityComboTr.Visibility = Visibility.Visible;
            additionalInfo.Visibility = Visibility.Visible;
            deleteComboBoxItem(IntervalCombo, "Annual");
            deleteComboBoxItem(IntervalCombo, "Quarterly");
            if (IntervalCombo.Items.Count > 1 || IntervalCombo.Items.Count > 2) return;
            else
            {
                IntervalCombo.Items.Add(createComboBoxItem("Daily"));
                IntervalCombo.Items.Add(createComboBoxItem("Weekly"));
                IntervalCombo.Items.Add(createComboBoxItem("Monthly"));
            }
            md.LoadGDP("TREASURY_YIELD", "monthly");

        }

        private void ButtonTreasury_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!clickedTreasuryButton)
            {
                TreasuryButton.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                TreasuryButton.BorderThickness = (Thickness)(new ThicknessConverter().ConvertFrom("0, 0, 0, 0"));

            }
        }
        private void ButtonTreasury_MouseEnter(object sender, MouseEventArgs e)
        {
            TreasuryButton.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FF2A4191"));
            TreasuryButton.BorderThickness = (Thickness)(new ThicknessConverter().ConvertFrom("0, 0, 0, 3"));
        }


        private ComboBoxItem createComboBoxItem(string content)
        {
            ComboBoxItem c = new ComboBoxItem();
            c.Content = content;
            c.Visibility = Visibility.Visible;
            c.Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF2A4191"));
            return c;
        }

        private void deleteComboBoxItem(ComboBox cb, String cbiToDelete)
        {
            foreach (ComboBoxItem cbi in cb.Items)
            {
                if (cbiToDelete.Equals((cbi.Content).ToString())) { cb.Items.Remove(cbi); return; }
            }
        }

        private void IntervalCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cdi = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            if (cdi == null) return;

            string selected = cdi.Content.ToString();
            if (clickedGDPButton) md.LoadGDP("REAL_GDP", selected.ToLower());
            else
            {
                if ((ComboBoxItem)MaturityComboTr.SelectedItem == null){
                    md.LoadTr("TREASURY_YIELD", selected.ToLower(), "");
                }
                else
                {
                    string selectedM = ((ComboBoxItem)((ComboBoxItem)MaturityComboTr.SelectedItem)).Content.ToString().ToLower();
                    md.LoadTr("TREASURY_YIELD", selected.ToLower(), "&maturity=" + selectedM);
                }
            }
        }
    }
}
