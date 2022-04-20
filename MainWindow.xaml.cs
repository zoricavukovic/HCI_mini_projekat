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
using SciChart;
using LiveCharts.Configurations;
using MiniProjekatHCI.model;
using LiveCharts.Defaults;

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
            

            var chartData = md.LoadGDP("REAL_GDP", "quarterly");
            DateFrom.SelectedDate = (from data in chartData.data
                                     orderby data.date
                                     select data.date).First();
            DateTo.SelectedDate = (from data in chartData.data
                                   orderby data.date descending
                                   select data.date).First();
            loadLineChart(chartData);

            loadOhclChart(chartData);
            
        }

        private void loadOhclChart(Data chartData)
        {
            columnChart.Series.Clear();
            columnChart.AxisX.Clear();
            columnChart.AxisY.Clear();

            var dateFrom = DateFrom.SelectedDate.Value;
            var dateTo = DateTo.SelectedDate.Value;

            label1.Content = chartData.name+" Bar Chart";
            label1.HorizontalAlignment = HorizontalAlignment.Center;

            var years = (from timeVal in chartData.data
                         where timeVal.date >= dateFrom && timeVal.date <= dateTo
                         orderby timeVal.date
                         select timeVal.date
                         ).ToList();
            var chartValues = new ChartValues<double>();
            foreach (var year in years)
            {
                double value = (from timeVal in chartData.data
                                where timeVal.date == year
                                select timeVal.valueD).First();
                chartValues.Add(value);

                var point = new Point() { X = year.ToOADate(), Y = value };
            }


            var seriesCollection = new SeriesCollection
            {
               new ColumnSeries
                {
                    Title = "2015",
                    Values = chartValues,
                    Configuration = new CartesianMapper<double>()
                    .Y(point => point)
                    .Stroke(point => (point == chartValues.Max()) ? Brushes.Blue : (point == chartValues.Min()) ? Brushes.Red : Brushes.Green)
                    .Fill(point => (point == chartValues.Max()) ? Brushes.Blue : (point == chartValues.Min()) ? Brushes.Red : Brushes.Green),
                }
            };
            // var formatter = value => value.ToString("N");

            var xLabels = (from timeVal in chartData.data
                           where timeVal.date >= dateFrom && timeVal.date <= dateTo
                           orderby timeVal.date
                           select timeVal.date.ToString().Split(' ')[0]
                         ).ToList();

            columnChart.AxisX.Add(new Axis
            {
                Title = "Datum",
                Labels = xLabels

            }) ;
            columnChart.AxisY.Add(new Axis
            {
                Title = chartData.unit,
                LabelFormatter = value => value.ToString("C")

            });

            columnChart.Series = seriesCollection;

        }

        private void loadLineChart(Data chartData)
        {
            lineChart.Series.Clear();
            lineChart.AxisX.Clear();
            lineChart.AxisY.Clear();

            label.Content = chartData.name + " Line Chart";
            label.HorizontalAlignment = HorizontalAlignment.Center;

            var dateFrom = DateFrom.SelectedDate.Value;
            var dateTo = DateTo.SelectedDate.Value;
            
            var xLabels = (from timeVal in chartData.data
                           where timeVal.date >= dateFrom && timeVal.date <= dateTo
                           orderby timeVal.date
                           select timeVal.date.ToString().Split(' ')[0]
                          ).ToList();

            lineChart.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Date",
                Labels = xLabels

            });

            lineChart.AxisY.Add(new Axis
            {
                Title = chartData.unit,
                LabelFormatter = value => value.ToString("C")

            });

            SeriesCollection series = new SeriesCollection();
            var years = (from timeVal in chartData.data
                         where timeVal.date >= dateFrom && timeVal.date <= dateTo
                         orderby timeVal.date
                         select timeVal.date
                          ).ToList();
            var chartValues = new ChartValues<double>();
            foreach (var year in years)
            {
                double value = (from timeVal in chartData.data
                                where timeVal.date == year
                                select timeVal.valueD).First();
                chartValues.Add(value);

                var point = new Point() { X = year.ToOADate(), Y = value };
                //chartValues.Add(point);
            }

            series.Add(new LineSeries()
            {
                Title = "Real GDP",
                Values = chartValues,
                Configuration = new CartesianMapper<double>()
                    .Y(point => point)
                    .Stroke(point => (point == chartValues.Max()) ? Brushes.Blue : (point == chartValues.Min()) ? Brushes.Red : Brushes.Green)
                    .Fill(point => (point == chartValues.Max()) ? Brushes.Blue : (point == chartValues.Min()) ? Brushes.Red : Brushes.Green),
                PointGeometrySize = 3,
            });

            lineChart.Series = series;

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

            notificationLabel.Visibility = Visibility.Hidden;

            if (IntervalCombo.Items.Count > 1 || IntervalCombo.Items.Count > 2) return;
            else
            {
                IntervalCombo.Items.Add(createComboBoxItem("Annual"));
                IntervalCombo.Items.Add(createComboBoxItem("Quarterly"));
            }

            IntervalCombo.SelectedIndex = 0;
            Data chartData = null;
            try
            {
                chartData = md.LoadGDP("REAL_GDP", "annual");
                
                DateFrom.SelectedDate = (from data in chartData.data
                                         orderby data.date
                                         select data.date).First();
                DateFrom.BlackoutDates.Clear();
                DateFrom.BlackoutDates.Add(new CalendarDateRange(new DateTime(1, 1, 1), ((DateTime)DateFrom.SelectedDate).AddDays(-1)));

                DateTo.SelectedDate = (from data in chartData.data
                                       orderby data.date descending
                                       select data.date).First();

                loadLineChart(chartData);
                loadOhclChart(chartData);
            }
            catch (LoadDataException ex)
            {
                MessageBox.Show(ex.Message, "Load Data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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

            IntervalCombo.SelectedIndex = 2;

            var chartData = md.LoadGDP("TREASURY_YIELD", "monthly");
            loadLineChart(chartData);
            loadOhclChart(chartData);
            //DateFrom.SelectedDate = (from data in chartData.data
            //                         orderby data.date
            //                         select data.date).First();
            //DateFrom.BlackoutDates.Clear();
            //DateFrom.BlackoutDates.Add(new CalendarDateRange(new DateTime(1, 1, 1), ((DateTime)DateFrom.SelectedDate).AddDays(-1)));

            //DateTo.SelectedDate = (from data in chartData.data
            //                       orderby data.date descending
            //                       select data.date).First();
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

      


        private void LoadTable_Click(object sender, RoutedEventArgs e)
        {
            Data chartData = GetDataByCriteria();
            if (chartData == null)
            {
                MessageBox.Show("Error on loading data", "Error on loading data", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                TableDTO tableDTO = new TableDTO(clickedGDPButton, GetClickedIntervalName(), GetSelectedMaturity(), GetSelectedStartDate(), GetSelectedEndDate(), getFilteredDataByTimeCriteria(chartData));
                TableWindow tableWindow = new TableWindow(tableDTO);
                tableWindow.Show();
            }
        }

        private List<PerDataValue> getFilteredDataByTimeCriteria(Data chartData)
        {
            return (from c in chartData.data
                    where c.date >= GetSelectedStartDate() && c.date <= GetSelectedEndDate()
                    select c
                   ).ToList();
        }

        public String GetClickedIntervalName()
        {
            if (clickedGDPButton)
            {
                string selected = "annualy";
                if (IntervalCombo.SelectedItem != null)
                {
                    ComboBoxItem item = (ComboBoxItem)IntervalCombo.SelectedItem;
                    selected = item.Content.ToString().ToLower();
                }
                return selected;
            }
            else
            {
                string selected = "monthly";
                if (IntervalCombo.SelectedItem != null)
                {
                    ComboBoxItem item = (ComboBoxItem)IntervalCombo.SelectedItem;
                    selected = item.Content.ToString().ToLower();
                }
                return selected;
            }
        }
        public String GetSelectedMaturity()
        {
            if (MaturityComboTr.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)MaturityComboTr.SelectedItem;
                string selectedM = string.Join("", item.Content.ToString().ToLower().Split(' '));
                return selectedM;
            }
            return null;
        }

        public DateTime GetSelectedStartDate()
        {
            return DateFrom.SelectedDate.Value;
        }
        public DateTime GetSelectedEndDate()
        {
            return DateTo.SelectedDate.Value;
        }

        private void LoadChart_Click(object sender, RoutedEventArgs e)
        {
            Data chartData = GetDataByCriteria();
            if (chartData == null)
            {
                MessageBox.Show("Error on loading data from api", "Error on loading data from api", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                loadLineChart(chartData);
                loadOhclChart(chartData);
            }
        }

        public Data GetDataByCriteria()
        {
            Data retVal = null;
            try
            {
                if (clickedGDPButton)
                {
                    string selected = "annualy";
                    if (IntervalCombo.SelectedItem != null)
                    {
                        ComboBoxItem item = (ComboBoxItem)IntervalCombo.SelectedItem;
                        selected = item.Content.ToString().ToLower();
                    }
                    retVal = md.LoadGDP("REAL_GDP", selected);
                }
                else
                {
                    string selected = "monthly";
                    if (IntervalCombo.SelectedItem != null)
                    {
                        ComboBoxItem item = (ComboBoxItem)IntervalCombo.SelectedItem;
                        selected = item.Content.ToString().ToLower();
                    }
                    if (MaturityComboTr.SelectedItem == null)
                    {
                        retVal = md.LoadTr("TREASURY_YIELD", selected.ToLower(), "");
                    }
                    else
                    {
                        ComboBoxItem item = (ComboBoxItem)MaturityComboTr.SelectedItem;
                        string selectedM = string.Join("", item.Content.ToString().ToLower().Split(' '));
                        retVal = md.LoadTr("TREASURY_YIELD", selected.ToLower(), "&maturity=" + selectedM);
                    }
                }
                return retVal;

            }
            catch (LoadDataException ex)
            {
                return null;
            }
        } 


        private void DateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateTo.SelectedDate == null)
                return;

            DateTo.BlackoutDates.Clear();

            if (DateTo.SelectedDate < (DateTime)DateFrom.SelectedDate)
                DateTo.SelectedDate = ((DateTime)DateFrom.SelectedDate).AddDays(1);
            
            DateTo.BlackoutDates.Add(new CalendarDateRange(new DateTime(1, 1, 1), (DateTime)DateFrom.SelectedDate));
           

            if (clickedTreasuryButton)
            {
                if (IntervalCombo.SelectedItem == null)
                    return;
                
                ComboBoxItem item = (ComboBoxItem)IntervalCombo.SelectedItem;
                var selected = item.Content.ToString().ToLower();

                if (selected.Equals("daily"))
                {
                    DateTo.SelectedDate = ((DateTime)DateFrom.SelectedDate).AddMonths(3);

                    DateTo.BlackoutDates.Add(new CalendarDateRange(((DateTime)DateTo.SelectedDate).AddDays(1), new DateTime(2100, 1, 1)));

                }
                else if (selected.Equals("weekly"))
                {
                    DateTo.SelectedDate = ((DateTime)DateFrom.SelectedDate).AddYears(2);

                    DateTo.BlackoutDates.Add(new CalendarDateRange(((DateTime)DateTo.SelectedDate).AddDays(1), new DateTime(2100, 1, 1)));
                }
                else if (selected.Equals("monthly"))
                {
                    DateTo.SelectedDate = ((DateTime)DateFrom.SelectedDate).AddYears(6);

                    DateTo.BlackoutDates.Add(new CalendarDateRange(((DateTime)DateTo.SelectedDate).AddDays(1), new DateTime(2100, 1, 1)));

                }


            }


        }

        private void IntervalCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clickedTreasuryButton)
            {
                if (IntervalCombo.SelectedItem == null)
                    return;
                ComboBoxItem item = (ComboBoxItem)IntervalCombo.SelectedItem;
                var selected = item.Content.ToString().ToLower();
                if (selected.Equals("daily"))
                {
                    DateFrom.SelectedDate = DateTime.Now.AddMonths(-3);
                    notificationLabel.Visibility = Visibility.Visible;
                    notificationLabel.Content = "* Because you selected daily interval you can only select 3 month period.";
                }
                else if (selected.Equals("weekly"))
                {
                    DateFrom.SelectedDate = DateTime.Now.AddYears(-2);
                    notificationLabel.Visibility = Visibility.Visible;
                    notificationLabel.Content = "* Because you selected weekly interval you can only select 2 year period.";
                }
                else if (selected.Equals("monthly"))
                {
                    DateFrom.SelectedDate = DateTime.Now.AddYears(-6);
                    notificationLabel.Visibility = Visibility.Visible;
                    notificationLabel.Content = "* Because you selected monthly interval you can only select 6 year period.";
                }
                else
                {
                    notificationLabel.Visibility = Visibility.Hidden;

                }


            }

            else if (clickedGDPButton)
            {
                if (IntervalCombo.SelectedItem == null)
                    return;
                ComboBoxItem item = (ComboBoxItem)IntervalCombo.SelectedItem;
                var selected = item.Content.ToString().ToLower();

                if (selected.Equals("quarterly"))
                    DateFrom.SelectedDate = new DateTime(2002, 1, 1);
                else if(selected.Equals("annual"))
                    DateFrom.SelectedDate = new DateTime(1929, 1, 1);

            }


        }
    }
}
