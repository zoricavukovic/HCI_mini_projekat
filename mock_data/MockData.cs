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
        private ErrorWindow errorWindow;
       
        public MockData()
        {
        }


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
                    errorWindow = new ErrorWindow("Loading data failed. Try again later.");
                    errorWindow.ShowDialog();
                    return null;
                }
                else
                {
                    foreach (PerDataValue v in json.data)
                    {
                        double doubleVal;
                        if (Double.TryParse(v.value, out doubleVal)) { v.valueD = doubleVal; }
                        else { v.valueD = 0.0; }
                    }

                    sr.Close();
                    return json;
                }
            }
            catch (Exception)
            {
                errorWindow = new ErrorWindow("Loading data failed. Try again later.");
                errorWindow.ShowDialog();
            }
            return null;

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
                    errorWindow = new ErrorWindow("Loading data failed. Try again later.");
                    errorWindow.ShowDialog();
                    return null;
                }
                else
                {
                    foreach (PerDataValue v in json.data)
                    {
                        double doubleVal;
                        if (Double.TryParse(v.value, out doubleVal)) { v.valueD = doubleVal; }
                        else { v.valueD = 0.0; }
                    }
                    sr.Close();
                    return json;
                }
            }
            catch (Exception)
            {
                errorWindow = new ErrorWindow("Loading data failed. Try again later.");
                errorWindow.ShowDialog();
            }

            return null;

        }


    }
}
