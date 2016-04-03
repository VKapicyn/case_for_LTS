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
            User user = User.getInstance();
            user.login = textBox1.Text;
            user.password = textBox2.Text;
            user.server = textBox3.Text;
            int port = 0;
            try { port = int.Parse(textBox4.Text); }
            catch 
            {
                MessageBox.Show("Некорректное значение port.");
                return; 
            }
            user.port = port;
            this.Close();
        }
    }
}
