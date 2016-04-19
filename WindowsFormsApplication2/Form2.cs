using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Decimal min=-1,max=1;
            try
            {
                if (textBox1.Text != "")
                {
                    min = Decimal.Parse(textBox1.Text);
                    if (min > 1 || min < -1)
                        throw new Exception();
                }
            }
            catch { MessageBox.Show("Некорректное значение минимума"); return; }
            try
            {
                if (textBox2.Text != "")
                {
                    max = Decimal.Parse(textBox2.Text);
                    if (max > 1 || max < -1)
                        throw new Exception();
                }
            }
            catch { MessageBox.Show("Некорректное значение максимума"); return; }
            Form1.min = min;
            Form1.max = max;
            this.Close();
        }
    }
}
