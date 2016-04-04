using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Atentis.History;
using System.Globalization;
using System.IO;
using System.Net;

namespace WindowsFormsApplication2
{
    public class User
    {
        private HistoryProvider provider = new HistoryProvider();

        private static User instance;
        public static User getInstance()
        {
            if(instance == null)
                instance = new User();
            return instance;
        }

        private User()
        {
     
        }
        public List<RawCandle> getHistory(string SECBOARD, string SECCODE, DateTime from, DateTime till, int timeFrame)
        {
            List<RawCandle> res=new List<RawCandle>();  
            try
            {
                HistoryRequest req = new HistoryRequest(SECBOARD, SECCODE, timeFrame,from,till);
                res = provider.LoadHistory(req, true);
                (Application.OpenForms[0] as Form1).addEvent(SECCODE, "История загружена.");
                
            }
            catch
            {
                (Application.OpenForms[0] as Form1).addEvent(SECCODE, "Не удалось загрузить историю, обратитесь в поддержку.");
            }
            return res;
        }

        public List<RawCandle> getHistory(string ticker, int from, int time)
        {
            try
            {
                WebClient webClient = new WebClient();
                string month=(DateTime.Now.Month)>10?(DateTime.Now.Month-1).ToString():"0"+(DateTime.Now.Month-1).ToString();
                if(time==1)
                    webClient.DownloadFile("http://real-chart.finance.yahoo.com/table.csv?s=" + ticker + "&a=00&b=1&c=" + (DateTime.Now.Year - from) + "&d=" + month + "&e=" + DateTime.Now.Day + "&f=" + DateTime.Now.Year + "&g=d&ignore=.csv", "base" + @"\" + ticker + ".csv");
                if (time == 7)
                    webClient.DownloadFile("http://real-chart.finance.yahoo.com/table.csv?s=" + ticker + "&a=00&b=1&c=" + (DateTime.Now.Year - from) + "&d=" + month + "&e=" + DateTime.Now.Day + "&f=" + DateTime.Now.Year + "&w=d&ignore=.csv", "base" + @"\" + ticker + ".csv");
                if (time == 30)
                    webClient.DownloadFile("http://real-chart.finance.yahoo.com/table.csv?s=" + ticker + "&a=00&b=1&c=" + (DateTime.Now.Year - from) + "&d=" + month + "&e=" + DateTime.Now.Day + "&f=" + DateTime.Now.Year + "&m=d&ignore=.csv", "base" + @"\" + ticker + ".csv");
            }
            catch
            {
                (Application.OpenForms[0] as Form1).addEvent(ticker, "Ошибка тут.");
                return null; }
            (Application.OpenForms[0] as Form1).addEvent(ticker, "История загружена.");

            List<RawCandle> res = new List<RawCandle>();
            NumberFormatInfo nfi_e = new CultureInfo("en-US", false).NumberFormat;
            try
            {
                using (var sr = new StreamReader("base" + @"\" + ticker + ".csv"))
                {
                    try
                    {
                        var first = sr.ReadLine().Split(',');//пропускаем первую строку c заголовком
                    }
                    catch { return null; }

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(',');
                        RawCandle a = new RawCandle(DateTime.Parse(line[0]),double.Parse(line[1], nfi_e),
                            double.Parse(line[2], nfi_e),double.Parse(line[3], nfi_e),double.Parse(line[4], nfi_e),
                            double.Parse(line[5], nfi_e));
                        res.Add(a);
                    }
                }
                (Application.OpenForms[0] as Form1).addEvent(ticker, "История сформирована.");
            }
            catch { return null; }
            return res;
        }
    }
}
