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
        public String Value { get; set; }

        public TableRow(DateTime timeVal, String value)
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
                ? "Real Gross Domestic Product table data"
                : tableDTO.SelectedMaturity + " Treasury Constant Maturity Rate Table Data";

            IntervalLbl.Content = tableDTO.IntervalName;
            if (tableDTO.GDPselected)
            {
                MaturityTextBlock.Visibility = Visibility.Hidden;
                MaturityLbl.Visibility = Visibility.Hidden;
            }
            else
            {
                MaturityLbl.Content = tableDTO.SelectedMaturity;
            }

            DateRangeLbl.Content = tableDTO.SelectedStartDate.ToString().Split(' ')[0] + " - " + tableDTO.SelectedEndDate.ToString().Split(' ')[0];
        }
        private void fillTable()
        {
            var dateColumn = new DataGridTextColumn();
            dateColumn.Header = "Date";
            dateColumn.Binding = new Binding("Date");
            
            dt.Columns.Add(dateColumn);

            var valueColumn = new DataGridTextColumn();
            valueColumn.Binding = new Binding("Value");
            if (tableDTO.GDPselected)
            {
                valueColumn.Header = "Billions of dollars";
            }
            else
            {
                valueColumn.Header = "Percent";
            }
            dt.Columns.Add(valueColumn);
            
            tableData.ForEach(t => dt.Items.Add(t));

        }

        private List<TableRow> getAllItemsSource()
        {
            List<TableRow> rows = new List<TableRow>();
            var items = this.tableDTO.DataToDisplay;
            foreach (var i in items) {
                if (this.tableDTO.GDPselected)
                {
                    rows.Add(new TableRow(i.date, i.value + "$"));
                }
                else
                {
                    rows.Add(new TableRow(i.date, i.value + "%"));
                }
            }
            return rows;
        }


        
    }
}
