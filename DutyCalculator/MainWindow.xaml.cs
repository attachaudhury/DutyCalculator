using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace DutyCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //lbl_DollarRate.Content = getDollarRate();
            } catch { }
        }

        private void btn_Calculate(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dt = (DateTime)dp_Date.SelectedDate;
                string sYear = Convert.ToString(dt.Year);
                string sMonth = Convert.ToString(dt.Month);
                string sDay = Convert.ToString(dt.Day);
                string Capacity = cb_Capacity.Text;
                if (Capacity == "660cc - 800cc")
                {
                    Capacity = "800";
                }
                else if (Capacity == "801cc - 1000cc")
                {
                    Capacity = "1000";
                }
                else if (Capacity == "1001cc - 1300cc")
                {
                    Capacity = "1300";
                }
                else if (Capacity == "1301cc - 1500cc")
                {
                    Capacity = "1500";
                }
                else if (Capacity == "1501cc - 1600cc")
                {
                    Capacity = "1600";
                }
                else if (Capacity == "1601cc - 1800cc")
                {
                    Capacity = "1800";
                }
                string mYear = cb_mYear.Text;
                string Type = cb_Type.Text;
                if (mYear == "2012 (Jeep / Hiace / Commercial)")
                {
                    mYear = "2012";
                }
                if (mYear == "2013 (Jeep / Hiace / Commercial)")
                {
                    mYear = "2013";
                }

                string dollarRate = Convert.ToString(tb_DollarRate.Text);
                lbl_Duty.Content = CalculateDuty(Capacity, Type.ToLower(), mYear, sYear, sMonth, sDay, dollarRate);
            } catch { }
            
            
        }
        double getDollarRate()
        {
            string sURL = "http://apilayer.net/api/live?access_key=846805c37cd1f95a11bfb6f858405181&currencies=PKR";
            
            var request = WebRequest.Create(sURL);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            var splashInfo = JsonConvert.DeserializeObject<RootObject>(text);
            double rate = splashInfo.quotes.USDPKR;

            return Math.Round(rate, 2); 
        }
        public class Quotes
        {
            public double USDPKR { get; set; }
        }
        public class RootObject
        {
            public bool success { get; set; }
            public string terms { get; set; }
            public string privacy { get; set; }
            public int timestamp { get; set; }
            public string source { get; set; }
            public Quotes quotes { get; set; }
            
        }
        double CalculateDuty(string capacity, string type, string mYear,string sYear, string sMonth, string sDay, string dollarRate)
        {
            //CalculateDuty("1000", "normal", "2012", "2017", "5", "20", "105.10");
            g_data.Visibility = Visibility.Visible;
            int calulatedMonths = 0;
            int mYear_int = Convert.ToInt32(mYear);
            int sYear_int = Convert.ToInt32(sYear);
            int sMonth_int = Convert.ToInt32(sMonth);
            int sDay_int = Convert.ToInt32(sDay);

            int tmpMonths = (sYear_int - mYear_int -1) * 12;
            calulatedMonths = tmpMonths + sMonth_int;

            if (sDay_int <= 15)
            {
                calulatedMonths = calulatedMonths - 1;
            }

            if (mYear_int == 2017)
            {
                calulatedMonths = 0;
            }
            if (calulatedMonths >= 50)
            {
                calulatedMonths = 50;
            }

            
            int itpForNew = 0;
            int cdPercentage = 0;
            
            switch (capacity)
            {
                case "800":
                    itpForNew = 4800;
                    cdPercentage = 50;
                    type = "normal";
                    break;
                case "1000":
                    itpForNew = 6000;
                    cdPercentage = 55;
                    type = "normal";
                    break;
                case "1300":
                    itpForNew = 13200;
                    cdPercentage = 60;
                    type = "normal";
                    break;
                case "1500":
                    itpForNew = 18590;
                    cdPercentage = 60;
                    break;
                case "1600":
                    itpForNew = 18590;
                    cdPercentage = 75;
                    break;
                case "1800":
                    itpForNew = 27940;
                    cdPercentage = 75;
                    break;   
            }

            int calculateditp = itpForNew - ((itpForNew * calulatedMonths) / 100);
            if (type == "hybrid")
            {
                calculateditp = calculateditp / 2;
            }
            

            double dollarRate_double = Convert.ToDouble(dollarRate);


            int assesdCustomValue = 0;
            double importValue = 0;
            int importValue_Int = 0;
            if (capacity == "800")
            {
                double tmpitp = (calculateditp * 100) / 86.03;
                assesdCustomValue = Convert.ToInt32(tmpitp);

                importValue = assesdCustomValue * dollarRate_double;
                importValue_Int = (Convert.ToInt32(importValue))+50;
                

            }
            if (capacity == "1000")
            {
                double tmpitp = ((calculateditp * 100) / 92.231)+1;
                assesdCustomValue = Convert.ToInt32(tmpitp);

                importValue = assesdCustomValue * dollarRate_double;
                importValue_Int = (Convert.ToInt32(importValue)) + 32;
            }
            if (capacity == "1300")
            {
                double tmpitp = ((calculateditp * 100) / 98.432) ;
                assesdCustomValue = Convert.ToInt32(tmpitp);

                importValue = assesdCustomValue * dollarRate_double;
                importValue_Int = (Convert.ToInt32(importValue));
            }
            if (capacity == "1500")
            {
                double tmpitp = ((calculateditp * 100) / 98.432);
                assesdCustomValue = Convert.ToInt32(tmpitp);

                importValue = assesdCustomValue * dollarRate_double;
                importValue_Int = (Convert.ToInt32(importValue)) - 6;
            }
            if (capacity == "1600" || capacity == "1800")
            {
                double tmpitp = ((calculateditp * 100) / 117.035);
                assesdCustomValue = Convert.ToInt32(tmpitp);

                importValue = assesdCustomValue * dollarRate_double;
                importValue_Int = (Convert.ToInt32(importValue)) + 45;
            }
            


            int capacity_int = Convert.ToInt32(capacity);

            int calculatedDuty = 0;
            int cd = (importValue_Int * cdPercentage)/100;
            int j = importValue_Int + cd;
            int st = (j * 17) / 100;
            int l = importValue_Int+ cd + st;
            int it = (l * 6) / 100;

            calculatedDuty = cd + st + it;

            calculatedDuty = calculatedDuty + ((l / 100) * 3);
            
            return calculatedDuty;
        }
        
    }
}
