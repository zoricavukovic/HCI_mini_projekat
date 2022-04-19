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
using System.Windows;

namespace MiniProjekatHCI.mock_data
{
    class MockData
    {
        private readonly string apiKey = "G3SDJTDZWHWPMGBV";

        public Data LoadGDP(String f, String i)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://" + $@"www.alphavantage.co/query?function={f}&interval={i}&apikey={this.apiKey}&datatype=json");
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                StreamReader sr = new StreamReader(res.GetResponseStream());
                string results = sr.ReadToEnd();
                Data json = JsonConvert.DeserializeObject<Data>(results);
                if (json.data == null)
                {
                    MessageBox.Show("Loading data failed.Try again!", "Load Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }
                foreach (PerDataValue v in json.data)
                {
                    double doubleVal;
                    if (Double.TryParse(v.value, out doubleVal)) { v.valueD = doubleVal; }
                    else { v.valueD = 0.0; }
                }

                sr.Close();
                return json;
            }
            catch (Exception)
            {
                MessageBox.Show("Loading data failed.Try again!", "Load Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

        }

        public Data LoadTr(String f, String i, String m)
        {
            try { 
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://" + $@"www.alphavantage.co/query?function={f}&interval={i}{m}&apikey={this.apiKey}&datatype=json");
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                StreamReader sr = new StreamReader(res.GetResponseStream());
                string results = sr.ReadToEnd();
                var json = JsonConvert.DeserializeObject<Data>(results);
                if (json.data == null)
                {
                    MessageBox.Show("Loading data failed.Try again!", "Load Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }
                foreach (PerDataValue v in json.data)
                {
                    double doubleVal;
                    if (Double.TryParse(v.value, out doubleVal)) { v.valueD = doubleVal; }
                    else { v.valueD = 0.0; }
                }
                sr.Close();
                return json;
            }
            catch (Exception)
            {
                MessageBox.Show("Loading data failed.Try again!", "Load Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

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
}
