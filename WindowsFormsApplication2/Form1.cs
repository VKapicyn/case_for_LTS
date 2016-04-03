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
            initEvents();
            Control.CheckForIllegalCrossThreadCalls = false;
            Security.securities = new List<Security>();
            Security.securities.Add(new Security());
        }
        private void initEvents()
        {
            listView1.Clear();
            listView1.Columns.Add("N", 30, HorizontalAlignment.Right);
            listView1.Columns.Add("Date", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("Time", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Source", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Event", 400, HorizontalAlignment.Left);
        }
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            DateTime close_time = DateTime.Now;
            String name = "log_" + close_time.ToShortDateString() + ".txt";
            StreamWriter sw = new StreamWriter(name, true, Encoding.UTF8);
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                sw.WriteLine(listView1.Items[i].SubItems[0].Text + "  |  " +
                    listView1.Items[i].SubItems[1].Text + "  |  " +
                    listView1.Items[i].SubItems[2].Text + "  |  " +
                    listView1.Items[i].SubItems[3].Text + "  |  " +
                    listView1.Items[i].SubItems[4].Text
                    );
            }
            sw.Close();
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
                            List<string> MOEX2_tikers = new List<string>(MOEX_tikers);
                            comboBox1.DataSource = MOEX_tikers;
                            comboBox2.DataSource = MOEX2_tikers;
                            MessageBox.Show("Тикеры успешно загружены!");
                            label13.Text="Загружено тикеров: " + MOEX_tikers.Count;
                            label14.Text = "Число сочетаний: " + ((Math.Pow(MOEX_tikers.Count, 2) - MOEX_tikers.Count) / 2).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Неудается прочитать файл: " + ex.Message);
                }
            }
        }
        public void addEvent(string source, string text)
        {
            string str = "";
            DateTime time = DateTime.Now;

            ListViewItem item = new ListViewItem((listView1.Items.Count + 1).ToString());
            for (int k = 1; k < listView1.Columns.Count; k++)
            {
                item.SubItems.Add("");
                string s = "";
                ColumnHeader col = listView1.Columns[k];
                if (col.Text == "Date")
                    s = time.ToShortDateString();
                else if (col.Text == "Time")
                    s = String.Format("{0}.{1:000}", time.ToLongTimeString(), time.Millisecond);
                else if (col.Text == "Source")
                    s = source;
                else if (col.Text == "Event")
                    s = text;
                item.SubItems[k].Text = s;
                str = str + s;
            }
            listView1.Items.Add(item);

            if (listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].Index == listView1.Items.Count - 2)
            {
                listView1.SelectedItems[0].Selected = false;
                item.Selected = true;
                item.Focused = true;
                item.EnsureVisible();
            }
            else if (listView1.SelectedItems.Count == 0)
                item.EnsureVisible();
        }

        //MICEX сравнение пар
        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog(this);
            User user = User.getInstance();
            user.connect();
            Security first = new Security(comboBox1.SelectedItem.ToString());
            Security second = new Security(comboBox2.SelectedItem.ToString());
            int time = 0, from = 0;
            try
            {
                time = int.Parse(textBox6.Text);
                if (time <= 0)
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Некорректное значения таймфрейма.");
                return;
            }
            try
            {
                from = int.Parse(textBox7.Text);
                if (from < 1 || from > 30)
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Некорректное значения периода.");
                return;
            }
            first.MICEX_history = user.getHistory("TQBR", first.ticker, DateTime.Now.AddDays((-1)*from), DateTime.Now, time);
            second.MICEX_history = user.getHistory("TQBR", second.ticker, DateTime.Now.AddDays((-1) * from), DateTime.Now, time);
            //подсчет общей корреляции и сохранение в файл
            //вывод графика
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Заглушка");
        }
    }
}
