using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DefectorVK
{
    public partial class CaptchaMesseeg : Form
    {
        public CaptchaMesseeg()
        {
            InitializeComponent();
        }
        public string captcha_pict;
        //private Button button1;
        //private TextBox textBox1;
        //private PictureBox pictureBox1;
        public string captcha_res;
        private void CaptchaMesseeg_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = captcha_pict;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                captcha_res = textBox1.Text;
            }
            else
            {
                MessageBox.Show("Введите код с картинки");
            }
        }

       
    }
}
