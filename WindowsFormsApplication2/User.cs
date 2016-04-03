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

        private static User instance;
        public static User getInstance()
        {
            if(instance == null)
                instance = new User();
            return instance;
        }

        public string login
        { get; set; }
        public string password
        { get; set; }
        public string server
        { get; set; }
        public int port
        { get; set; }

        private User()
        {
            slot = new Slot();         
        }

        public void connect()
        {
            slot.SlotID = this.login;
            slot.Login = this.login;
            slot.Password = this.password;
            slot.Server = this.server;
            slot.Port = this.port; 
            slot.evhSlotStateChanged += s_evhSlotStateChanged;
            slot.rqs = new RequestSocket(slot);
            slot.rqs.Init();
            slot.Start();
        }
        public List<RawCandle> getHistory(string SECBOARD, string SECCODE, DateTime from, DateTime till, int timeFrame)
        {
            try
            {
                HistoryRequest req = new HistoryRequest(SECBOARD, SECCODE, timeFrame,from,till);
                res = provider.LoadHistory(req, true);
                (Application.OpenForms[0] as Form1).addEvent("ИСТОРИЯ", "История загружена.");
            }
            catch
            {
                if (slot.State.Equals(SlotState.Connected))
                    (Application.OpenForms[0] as Form1).addEvent("ИСТОРИЯ", "Не удалось загрузить историю, обратитесь в поддержку.");
                else
                    (Application.OpenForms[0] as Form1).addEvent(slot.SlotID.ToString(),"Не удалось подключиться к серверу истории.");
            }
            return res;
        }
        private void s_evhSlotStateChanged(object Sender, SlotEventArgs e)
        {
            (Application.OpenForms[0] as Form1).addEvent(e.Slot.ToString(), e.State.ToString());
            if (e.State == SlotState.Failed || e.State == SlotState.Denied)
            {
                slot.Disconnect();
                slot.evhSlotStateChanged -= s_evhSlotStateChanged;
                MessageBox.Show("Не удается соединиится с сервером!\nПроверьте введенные данные.");
            }
        }
    }
}
