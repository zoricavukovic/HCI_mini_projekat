using MiniProjekatHCI.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjekatHCI.mock_data
{
    class MockData
    {
        private readonly string apiKey = "G3SDJTDZWHWPMGBV";
        private string function;
        public string Function { 
            get { return function; }
            set {
                if (function != value)
                {
                    function = value;
                    OnPropertyChanged("Function");
                }
            }
        }
        private string interval;
        public string Interval
        {
            get { return interval; }
            set
            {
                if (interval != value)
                {
                    interval = value;
                    OnPropertyChanged("Interval");
                }
            }
        }
        private string maturity;
        public string Maturity
        {
            get { return maturity; }
            set
            {
                if (maturity != value)
                {
                    maturity = value;
                    OnPropertyChanged("Maturity");
                }
            }
        }

        public MockData()
        {
        }

        public MockData(String f, String i, String m)
        {
            this.function = f;
            this.interval = i;
            this.maturity = m;
            if (m.Equals(""))
                LoadGDP(f, i);
            else
                LoadTr(f, i, m);
        }

        public void LoadGDP(String f, String i)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://" + $@"www.alphavantage.co/query?function={f}&interval={i}&apikey={this.apiKey}&datatype=json");
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(res.GetResponseStream());
            string results = sr.ReadToEnd();
            Data json = JsonConvert.DeserializeObject<Data>(results);
            
            sr.Close();

        }

        public void LoadTr(String f, String i, String m)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://" + $@"www.alphavantage.co/query?function={f}&interval={i}{m}&apikey={this.apiKey}&datatype=json");
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(res.GetResponseStream());
            string results = sr.ReadToEnd();
            var json = JsonConvert.DeserializeObject<Data>(results);
            foreach (PerDataValue v in json.data)
            {
                double doubleVal;
                if (Double.TryParse(v.value, out doubleVal)) { v.valueD = doubleVal; }
                else { v.valueD = 0.0; }
            }
            sr.Close();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
    public enum Interval
    {
        Annual, Quarterly, Daily, Weekly, Monthly
    }

    public enum Maturity
    {
        Month3, Year2, Year5, Year7, Year10, Year30
    }
    public enum Function
    {
        REAL_GDP, TREASURY_YIELD
    }
}
