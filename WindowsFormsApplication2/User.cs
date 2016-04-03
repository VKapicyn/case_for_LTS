using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atentis.Connection;
using Atentis.History;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication2
{
    public class User
    {
        private Slot slot;
        private List<RawCandle> res = new List<RawCandle>();
        private HistoryProvider provider = new HistoryProvider();

        private static readonly User instance = new User();
        public static User Instance
        {
            get { return instance; }
        }

        private User()
        {
            //чтение параметров с формы
            slot = new Slot();
        }

        public void connect()
        {
            slot.evhSlotStateChanged += s_evhSlotStateChanged;
            slot.rqs = new RequestSocket(slot);
            slot.rqs.Init();
            slot.Start();
        }
        public void getHistory(string SECBOARD, string SECCODE,DateTime from, DateTime till, int timeFrame)
        {
        restart:
            try
            {

                HistoryRequest req = new HistoryRequest(SECBOARD, SECCODE, timeFrame,from,till);
                res = provider.LoadHistory(req, true);
                (Application.OpenForms[0] as Form1).addEvent("ИСТОРИЯ", "История загружена");
            }
            catch
            {
                if (slot.State.Equals(SlotState.Connected))
                {
                    (Application.OpenForms[0] as Form1).addEvent("ИСТОРИЯ", "Не получается загрузить историю, ожидайте...");
                    Thread.Sleep(2000);
                    goto restart;
                }
                else
                { 
                    (Application.OpenForms[0] as Form1).addEvent(slot.SlotID.ToString(),"");
                }
            }
        }
        private void s_evhSlotStateChanged(object Sender, SlotEventArgs e)
        {
            (Application.OpenForms[0] as Form1).addEvent(e.Slot.ToString(), e.State.ToString());
        }
    }
}
