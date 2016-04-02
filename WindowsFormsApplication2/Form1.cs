using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        List<string> MOEX_tikers = new List<string>(); 
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            Security.securities = new List<Security>();
            Security.securities.Add(new Security());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            BackgroundWorker making_order = new BackgroundWorker();
            making_order.DoWork += make_order;
            making_order.RunWorkerAsync();
        }

        private void radioButton1_CheckedChanged(Object sender, EventArgs e)
        {
            textBox3.Visible = false;
            label3.Visible = false;
        }
        private void radioButton2_CheckedChanged(Object sender, EventArgs e)
        {
            textBox3.Visible = true;
            label3.Visible = true;
        }

        //метод заглушка
        private void make_order(object sender, DoWorkEventArgs e)
        {          
            bool flag = true;
            double stop_los = 0, take_pt = 0, price = 0;
            int amount = 0;

            try
            {
                if (radioButton1.Checked)
                {
                    if (!textBox3.Text.Equals(""))
                        price = double.Parse(textBox3.Text);
                    else
                        price = 0;
                }
                else
                {
                    price = double.Parse(textBox3.Text);
                }
            }
            catch
            {
                MessageBox.Show("Некорректное значение \"цена\"\nпопробуйте ввести еще раз.");
                return;
            }
            try
            {
                if (!textBox4.Text.Equals(""))
                    stop_los = double.Parse(textBox4.Text);
                else
                    stop_los = 0;
            }
            catch
            {
                MessageBox.Show("Некорректное значение \"stop-los\"\nпопробуйте ввести еще раз.");
                return;
            }
            try
            {
                if (!textBox5.Text.Equals(""))
                    take_pt = double.Parse(textBox5.Text);
                else
                    take_pt = 0;
            }
            catch
            {
                MessageBox.Show("Некорректное значение \"take-pt\"\nпопробуйте ввести еще раз.");
                return;
            }
            try
            {
                amount = int.Parse(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Некорректное значение \"количество\"\nпопробуйте ввести еще раз.");
                return;
            }

            foreach (var security in Security.securities)
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Вы не заполнили поле \"Инструмент\" заполните\nполе и попробуте еще раз.");
                    break;
                }
                else
                {
                    if (security.ticker.Equals(textBox1.Text))
                    {
                        flag = false;
                        if (radioButton1.Checked)
                        {
                            MessageBox.Show(security.AddOrder(amount, stop_los, take_pt));
                        }
                        else
                        {
                            MessageBox.Show(security.AddOrder(amount, price, stop_los, take_pt));
                        }
                        break;
                    }
                }   
            }

            if (flag)
            {
                Security new_order = new Security(textBox1.Text);
                if (radioButton1.Checked)
                {
                    MessageBox.Show(new_order.AddOrder(amount, stop_los, take_pt));
                }
                else
                {
                    MessageBox.Show(new_order.AddOrder(amount, price, stop_los, take_pt));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (var sr = new StreamReader(myStream))
                        {
                            while (!sr.EndOfStream)
                                MOEX_tikers.Add(sr.ReadLine().Trim());
                            MessageBox.Show("Тикеры успешно загружены!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Неудается прочитать файл: " + ex.Message);
                }
            }
        }

        //MICEX сравнение пар
        private void button3_Click(object sender, EventArgs e)
        {

        }

    }
}
