using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atentis.History;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication2
{
    public class User
    {
        public List<RawCandle> res
        { get; private set; }

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
            res = new List<RawCandle>();       
        }

        public void clear()
        {
            res = new List<RawCandle>();
        }
        public List<RawCandle> getHistory(string SECBOARD, string SECCODE, DateTime from, DateTime till, int timeFrame)
        {
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
    }
}
