using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace task7
{
    public partial class Form4 : Form
    {
        public bool flag = false;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            pictureBox3.Visible = true;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox3.Left += 5;
            if (pictureBox3.Location.X > 810)
            {
                timer1.Enabled = false;
                this.Close();
            }
        }

    }
}
