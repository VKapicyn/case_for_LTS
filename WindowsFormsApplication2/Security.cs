using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Atentis.History;

namespace WindowsFormsApplication2
{
    public class Security
    {
        public static List<Security> securities;
        public List<RawCandle> history
        { get; set; }
        public Security()
        {
            history = new List<RawCandle>();
            transactions = new List<int>();
            this.amount = 0;
            this.price = 0;
            this.ticker = "";
        }

        public Security(string ticker)
        {
            history = new List<RawCandle>();
            transactions = new List<int>();
            this.ticker = ticker;
            this.price = 0;
            this.amount = 0;
        }

        public static int order=1; 
        public string ticker 
            { get; private set; }

        public int amount
            { get; private set; }

        public double price 
            { get; private set; }

        public List<int> transactions
        { get; private set; }

        public string check(int amount, double price, double stop_los, double take_pt)
        {
            string buysell = "";
            if ((Application.OpenForms[0] as Form1).radioButton4.Checked)
            {
                this.amount += amount;
                buysell = "B";
            }
            else
            {
                this.amount -= amount;
                buysell = "S";
            }
            return String.Format("Инструмент: {0,-5}\nЦена: {1,-5}\nКоличество: {2,-5}\nНаправление: {3,-5}\nТип заявки: {4,-5}\nstop-los: {5,-5}\ntake-pt: {6,-5}",
                this.ticker, this.price, amount, buysell, "Limit", stop_los, take_pt);
        }

        public void addOrder(int amount, double price, double stop_los,double take_pt)
        {
            string buysell = "";
            if ((Application.OpenForms[0] as Form1).radioButton4.Checked)
            {
                this.amount += amount;
                buysell = "B";
            }
            else
            {
                this.amount -= amount;
                buysell = "S";
            }
            this.price = price;
            (Application.OpenForms[0] as Form1).dataGridView1.Rows.Add(order,this.ticker,amount,this.price,buysell,"Limit", stop_los, take_pt);
            order++;
            this.transactions.Add(order);
        }

        public string check(int amount, double stop_los, double take_pt)
        {
            string buysell = "";
            if ((Application.OpenForms[0] as Form1).radioButton4.Checked)
            {
                this.amount += amount;
                buysell = "B";
            }
            else
            {
                this.amount -= amount;
                buysell = "S";
            }
            return String.Format("Инструмент: {0,-5}\nКоличество: {2,-5}\nНаправление: {3,-5}\nТип заявки: {4,-5}\nstop-los: {5,-5}\ntake-pt: {6,-5}",
                this.ticker, 0, amount, buysell, "Market", stop_los, take_pt);
        }
        public void addOrder(int amount, double stop_los, double take_pt)
        {
            string buysell = "";
            if ((Application.OpenForms[0] as Form1).radioButton4.Checked)
            {
                this.amount += amount;
                buysell = "B";
            }
            else
            {
                this.amount -= amount;
                buysell = "S";
            }
            (Application.OpenForms[0] as Form1).dataGridView1.Rows.Add(order, this.ticker, amount, this.price,buysell, "Market", stop_los, take_pt);
            order++;
            this.transactions.Add(order);
        }
    }
}
