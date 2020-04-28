using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace DefectorVK
{
    public partial class ChekList : Form
    {
        public ChekList()
        {
            InitializeComponent();
        }
        public string text { get; set; }
        private void ChekList_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = text;
        }

        private void сохраниттьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Сохранить файл отчета";
            sfd.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
           
            if(sfd.ShowDialog()==DialogResult.OK)
            {
                richTextBox1.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText);
               
            }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start("chrome.exe", e.LinkText);
        }
    }
}
