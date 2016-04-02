using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    class Security
    {
        public static List<Security> securities;
        public Security()
        {
            _history = new List<History>();
            _transactions = new List<int>();
            this.amount = 0;
            this.price = 0;
            this.ticker = "";
            this.stop_los = 0;
            this.take_pt = 0;
        }

        public Security(string ticker, double stop_los,double take_pt)
        {
            _history = new List<History>();
            _transactions = new List<int>();
            this.ticker = ticker;
            this.stop_los = stop_los;
            this.take_pt = take_pt;
            this.price = 0;
            this.amount = 0;
        }

        public static int order=0; 
        public string ticker 
            { get; set; }

        public int amount
            { get; set; }

        public double price 
            { get; set; }

        public double stop_los
            { get; set; }

        public double take_pt
            { get; set; }

        private List<History> _history; 

        public List<History> history
        {
            get
            {
                return _history;
            }
            set 
            {
                _history = history;
            }
        }

        private List<int> _transactions;

        public List<int> transactions
        {
            get 
            {
                return _transactions;
            }
            set
            {
                _transactions = transactions;
            }
        }

        public string AddOrder(int amount, double price)
        {
            if ((Application.OpenForms[0] as Form1).radioButton4.Checked)
                this.amount += amount;
            else
                this.amount -= amount;

            this.price = price;
            (Application.OpenForms[0] as Form1).dataGridView1.Rows.Add(order,this.ticker,amount,this.price,"Limit",this.stop_los,this.take_pt);
            order++;

            return String.Format("Инструмент: {0,-5}\nЦена: {1,-5}\nКоличество: {2,-5}\nТип заявки: {3,-5}\nstop-los: {4,-5}\ntake-pt: {5,-5}",
                this.ticker, this.price, amount, "Limit", this.stop_los, this.take_pt);
        }

        public string AddOrder(int amount)
        {
            if ((Application.OpenForms[0] as Form1).radioButton4.Checked)
                this.amount += amount;
            else
                this.amount -= amount;

            (Application.OpenForms[0] as Form1).dataGridView1.Rows.Add(order, this.ticker, amount, this.price, "Limit", this.stop_los, this.take_pt);
            order++;
            return String.Format("Инструмент: {0,-5}\nКоличество: {2,-5}\nТип заявки: {3,-5}\nstop-los: {4,-5}\ntake-pt: {5,-5}",
                this.ticker, this.price, amount, "Market", this.stop_los, this.take_pt);
        }

        public class History
        {
            public History(double open, double hight, double low, double close, long volume, DateTime date)
            {
                this.open = open;
                this.hight = hight;
                this.low = low;
                this.close = close;
                this.volume = volume;
                this.date = date;
            }
            public double open
                { get; set; }
            public double hight
                { get; set; }
            public double low
                { get; set; }
            public double close
                { get; set; }
            public long volume
                { get; set; }
            public DateTime date
                { get; set; }
        }
    }
}
