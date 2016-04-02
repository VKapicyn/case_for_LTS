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


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
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

        private void make_order(object sender, DoWorkEventArgs e)
        {
            foreach (var security in Security.securities)
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Вы не заполнили поле \"Инструмент\" заполните\nполе и попробуте еще раз.");
                    break;
                }
                else
                {
                    double stop_los = 0, take_pt = 0,price=0;

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
                        break;
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
                        break;
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
                        break;
                    }
                    int amount = 0;
                    try
                    {
                        amount = int.Parse(textBox2.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Некорректное значение \"количество\"\nпопробуйте ввести еще раз.");
                        break;
                    }
                    if (security.ticker.Equals(textBox1.Text))
                    {
                        if (radioButton1.Checked)
                        {
                            MessageBox.Show(security.AddOrder(amount, stop_los, take_pt));
                        }
                        else
                        {
                            MessageBox.Show(security.AddOrder(amount, double.Parse(textBox3.Text), stop_los, take_pt));
                        }
                        break;
                    }
                    else
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
                        break;
                    }
                }   
            }
        }

    }
}
