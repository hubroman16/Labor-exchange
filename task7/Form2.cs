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
    //форма информации о вакансии
    public partial class Form2 : Form 
    {
        //конструктор формы заполняет текстбоксы
        public Form2(string name, string field, int exp, string education,int salary)
        {
            InitializeComponent();
            textBox1.Text = name;
            textBox2.Text = field;
            textBox3.Text = Convert.ToString(exp);
            textBox4.Text = education;
            textBox5.Text = Convert.ToString(salary);
        }
    }
}
