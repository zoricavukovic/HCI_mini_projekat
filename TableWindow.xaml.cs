using MiniProjekatHCI.model;
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
using System.Windows.Shapes;

namespace MiniProjekatHCI
{
    /// <summary>
    /// Interaction logic for TableWindow.xaml
    /// </summary>
    /// 

    public class TableRow
    {
        public String Date { get; set; }
        public double Value { get; set; }

        public TableRow(DateTime timeVal, double value)
        {
            Date = timeVal.ToString().Split(' ')[0];
            Value = value;
        }
    }


    public partial class TableWindow : Window
    {
        private TableDTO tableDTO;
        public List<TableRow> tableData;


        public TableWindow(TableDTO tableDTO)
        {
            InitializeComponent();
            this.tableDTO = tableDTO;
            tableData = getAllItemsSource();
            fillFields();
            fillTable();
        }

        private void fillFields()
        {
            Headerlbl.Content = tableDTO.GDPselected
                ? "Real Gross Domestic Product Table Data"
                : tableDTO.SelectedMaturity + " Treasury Constant Maturity Rate Table Data";

            IntervalLbl.Content = tableDTO.IntervalName;
            if (tableDTO.GDPselected)
            {
                MaturityTextBlock.Visibility = Visibility.Hidden;
                MaturityLbl.Visibility = Visibility.Hidden;
            }
            else
            {
                var a = tableDTO.SelectedMaturity;
                if (tableDTO.SelectedMaturity != null)
                {
                    MaturityTextBlock.Visibility = Visibility.Visible;
                    MaturityLbl.Content = tableDTO.SelectedMaturity;
                    
                }
                else
                {
                    MaturityTextBlock.Visibility = Visibility.Hidden;
                    MaturityLbl.Visibility = Visibility.Hidden;
                }
            }

            DateRangeLbl.Content = tableDTO.SelectedStartDate.ToString().Split(' ')[0] + " - " + tableDTO.SelectedEndDate.ToString().Split(' ')[0];
        }
        private void fillTable()
        {
            if (tableDTO.GDPselected)
            {
                ValueColumn.Header = "Billions Of Dollars ($)";
            }
            else
            {
                ValueColumn.Header = "Percent (%)";
            }
            
            tableData.ForEach(t => dt.Items.Add(t));

        }

        private List<TableRow> getAllItemsSource()
        {
            List<TableRow> rows = new List<TableRow>();
            var items = this.tableDTO.DataToDisplay;
            foreach (var i in items) {
                if (this.tableDTO.GDPselected)
                {
                    rows.Add(new TableRow(i.date, Math.Round(i.valueD, 2)));
                }
                else
                {
                    rows.Add(new TableRow(i.date, Math.Round(i.valueD, 2)));
                }
            }
            return rows;
        }


        
    }
}
