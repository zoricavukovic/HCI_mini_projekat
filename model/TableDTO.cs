using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjekatHCI.model
{
    public class TableDTO
    {
        public bool GDPselected { get; set; }
        public String IntervalName { get; set; }
        public String SelectedMaturity { get; set; }

        public DateTime SelectedStartDate { get; set; }
        public DateTime SelectedEndDate { get; set; }
        public List<PerDataValue> DataToDisplay { get; set; }

        public TableDTO(bool gDPselected, string intervalName, string selectedMaturity, DateTime selectedStartDate, DateTime selectedEndDate, List<PerDataValue> dataToDisplay)
        {
            GDPselected = gDPselected;
            IntervalName = intervalName;
            SelectedMaturity = selectedMaturity;
            SelectedStartDate = selectedStartDate;
            SelectedEndDate = selectedEndDate;
            DataToDisplay = dataToDisplay;
        }

    }
}
