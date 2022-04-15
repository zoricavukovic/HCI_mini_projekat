using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjekatHCI.model
{
    class Data
    {
        public string name { get; set; }
        public string interval { get; set; }
        public string unit { get; set; }
        public List<PerDataValue> data { get; set; }

        public Data() { }

        public Data(string name, string interval, string unit, List<PerDataValue> data)
        {
            this.name = name;
            this.interval = interval;
            this.unit = unit;
            this.data = data;
        }
    }

    public class PerDataValue
    {
        public DateTime date { get; set; }
        public double valueD { get; set; }
        public string value { get;
            set; }

        public PerDataValue() {
        }

        public PerDataValue(DateTime date, string value)
        {
            this.date = date;
            this.value = value;
        }
    }
}
