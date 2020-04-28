using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace DefectorVK
{
    public partial class BlackList : Form
    {
        public BlackList()
        {
            InitializeComponent();
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                bl_dt.Columns.Add(new DataColumn(dataGridView1.Columns[i].Name));
                dataGridView1.Columns[i].DataPropertyName = dataGridView1.Columns[i].Name;
            }
            dataGridView1.DataSource = bl_dt;


        }
        public VkNet.VkApi vk { get; set; }
        DataTable bl_dt = new DataTable("BlackList");
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";

            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
               
               
                var links = File.ReadAllLines(openFileDialog1.FileName);
                foreach (var link in links)
                {
                    if (link != string.Empty)
                        bl_dt.Rows.Add(link, string.Empty);
                }
                dataGridView1.Refresh();
            }
            toolStripStatusLabel2.Text = bl_dt.Rows.Count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            foreach( DataRow row in bl_dt.Rows)
            {
                
                string link = row.ItemArray[0].ToString().Substring(row.ItemArray[0].ToString().LastIndexOf(@"/") + 1);
                if (link != string.Empty)
                {
                    System.Threading.Thread.Sleep(500);
                    link = link.Trim();
                    var gr_id = vk.Utils.ResolveScreenName(link);
                    if(gr_id!=null)
                        row.SetField<object>(1, gr_id.Id);
                    else
                        row.SetField<object>(1, "Ссылка не работает");

                }
                dataGridView1.Refresh();
            }
            this.Cursor = Cursors.Default;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(bl_dt.Rows.Count>0)
            {
                try
                {
                    bl_dt.WriteXml("blacklist.xml");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
                finally
                {
                    MessageBox.Show("Черный список успешно сохранен");
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(File.Exists("blacklist.xml"))
            {
                bl_dt.ReadXml("blacklist.xml");
                dataGridView1.Refresh();
            }
            else
            {
                MessageBox.Show("Файл с черным списком еще не создан.");
            }
        }

        private void BlackList_Load(object sender, EventArgs e)
        {
            if (File.Exists("blacklist.xml"))
            {
               // bl_dt.ReadXmlSchema("blacklist.xml");
                bl_dt.ReadXml("blacklist.xml");
               // dataGridView1.DataSource = bl_dt;
                dataGridView1.Refresh();
                toolStripStatusLabel2.Text = bl_dt.Rows.Count.ToString();
            }

        }
        private DataGridViewCellEventArgs mouselocation;

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouselocation = e;
        }

        private void удалитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Удалить аккаунт {0}?", bl_dt.Rows[mouselocation.RowIndex].ItemArray[0].ToString()), "Удаление не обратимо!!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bl_dt.Rows.RemoveAt(mouselocation.RowIndex);
                dataGridView1.Refresh();
                toolStripStatusLabel2.Text = bl_dt.Rows.Count.ToString();
            }
        }
    }
}
