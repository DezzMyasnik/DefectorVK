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
using System.Text.RegularExpressions;
using System.IO;
namespace DefectorVK
{
    public partial class IndividualCheck : Form
    {
        public IndividualCheck()
        {
            InitializeComponent();
           
        }
        public VkNet.VkApi vk { get; set; }
        public List<string> users_id = new List<string>();
        public List<string> groups_id = new List<string>();
        private void button3_Click(object sender, EventArgs e)
        {
            if (vk != null)
            {
                users_id.Clear();
                groups_id.Clear();
                if (vk.IsAuthorized)
                {
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        try
                        {
                            string header = row.HeaderCell.Value.ToString();

                            header = header.Substring(header.LastIndexOf('/'));
                            string pattern = @"^(public+\d)";
                            Regex reg = new Regex(pattern);
                            if (reg.IsMatch(header))
                            {
                                header = header.Replace("public", "club");
                            }
                            header = header.Substring(header.LastIndexOf('/') + 1);
                            var id = vk.Utils.ResolveScreenName(header).Id.Value;
                            groups_id.Add(id.ToString());
                            var checker = vk.Groups.IsMember(id.ToString(), vk.UserId.Value,new List<long>() { vk.UserId.Value },false);
                            if (checker[0].Member)
                            {
                                // DataGridViewButtonCell dgbtn = null;
                                //row.Cells[0].Value = "huyiji";
                                var BtnCell = (DataGridViewButtonCell)row.Cells[1];
                                BtnCell.ReadOnly = false;
                                BtnCell.UseColumnTextForButtonValue = false;
                                BtnCell.Value = "Выйти";
                                //BtnCell.Style.BackColor = Color.LightGreen;


                                row.Cells[1].Style.BackColor = Color.Green;

                                //row.HeaderCell.Style.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                var BtnCell = (DataGridViewButtonCell)row.Cells[1];
                                BtnCell.ReadOnly = false;
                                BtnCell.UseColumnTextForButtonValue = false;
                                BtnCell.Value = "Вступить";
                                BtnCell.Style.BackColor = Color.IndianRed;
                                // BtnCell.Style.BackColor = Color.LightPink;
                                row.Cells[1].Style.BackColor = Color.Red;
                                //row.HeaderCell.Style.BackColor = Color.LightPink;
                            }
                            //us_id.Add(id);
                            //users_id.Add(id.ToString());
                            //toolStripProgressBar1.Value = row.Index;
                            //StringBuilder strb = new StringBuilder();
                            //strb.AppendFormat("Получение ID участников: {0}/{1}", row.Index, dataGridView1.Rows.Count);
                            //toolStripStatusLabel1.Text = strb.ToString();
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("!!!Проверте ссылки участников!!! где-то неправильная ссылка " + ex.Message);
                            break;
                        }
                    }
                    this.Cursor = Cursors.Default;




                    // dataGridView1.Refresh();
                    if (!textBox1.Text.Equals("Ссылка на группу"))
                    {
                        this.Cursor = Cursors.WaitCursor;
                        // List<string> users_id = new List<string>();
                        List<long> us_id = new List<long>();
                        //toolStripProgressBar1.Maximum = dataGridView1.Rows.Count;
                        //toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                        //toolStripStatusLabel1.Text = "Получение ID участников";
                       
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            try
                            {
                                string header = row.Cells[0].Value.ToString();

                                header = header.Substring(header.LastIndexOf('/') + 1);
                                var id = vk.Utils.ResolveScreenName(header).Id.Value;
                                us_id.Add(id);
                                users_id.Add(id.ToString());
                                // toolStripProgressBar1.Value = row.Index;
                                StringBuilder strb = new StringBuilder();
                                strb.AppendFormat("Получение ID участников: {0}/{1}", row.Index, dataGridView2.Rows.Count);
                                //toolStripStatusLabel1.Text = strb.ToString();
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show("!!!Проверте ссылки участников!!! где-то неправильная ссылка " + ex.Message);
                                break;
                            }
                        }
                        string control = textBox1.Text;

                        control = control.Substring(control.LastIndexOf('/') + 1);
                        try
                        {
                            var id_control = vk.Utils.ResolveScreenName(control.Replace(" ","")).Id.Value;
                            //groups_id.Add(id_control.ToString());
                            var ch = vk.Groups.IsMember(id_control.ToString(),null,us_id,false ); 
                            //var checker = vk.Groups.IsMember(id.ToString(), vk.UserId.Value, new List<long>() { vk.UserId.Value }, false);
                            for (int i = 0; i < ch.Count; i++)
                            {
                                dataGridView2.Rows[i].Cells[2].Value = ch[i].Member;
                                if (ch[i].Member)
                                {

                                    dataGridView2.Rows[i].Cells[0].Style.BackColor = Color.LightGreen;
                                }
                                else
                                {
                                    dataGridView2.Rows[i].Cells[0].Style.ForeColor = Color.Gold;
                                    dataGridView2.Rows[i].Cells[0].Style.BackColor = Color.Red;
                                }
                            }
                        }
                        catch (VkNet.Exception.VkApiException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        //toolStripStatusLabel1.Text = "Определение вступивших";
                        this.Cursor = Cursors.Default;
                    }
                    checkBox1.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("Вы не авторизовались в ВК");
            }
            
          
            }

        private void button13_Click(object sender, EventArgs e)
        {
           // dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();           
           
            
            DataGridViewTextBoxColumn text_col = new DataGridViewTextBoxColumn();
            text_col.HeaderText = "Участники*";
            dataGridView2.Columns.Add(text_col);

            //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // DataGridViewTextBoxColumn text_col_for_us = new DataGridViewTextBoxColumn();
            DataGridViewButtonColumn col = new DataGridViewButtonColumn();
            col.HeaderText = "Действие*";
            //col.Name = "ff";
            col.Text = "Вступить";
            col.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(col);
            DataGridViewCheckBoxColumn checkBoc_c = new DataGridViewCheckBoxColumn();
            checkBoc_c.FalseValue = false;
            checkBoc_c.TrueValue = true;

            checkBoc_c.HeaderText = "Состояние";
            checkBoc_c.Visible = false;

            dataGridView2.Columns.Add(checkBoc_c);
            foreach (var in_str in richTextBox1.Lines)
            {
                string[] temp = in_str.Split(' ');
                string[] uri_l = new string[2];
                int l = 0;
                DataGridViewRow row2 = new DataGridViewRow();
               // dataGridView2.Rows.Add();
                for (int i = 0; i < temp.Length; i++)
                {

                    if (Uri.IsWellFormedUriString(temp[i], UriKind.Absolute) && temp[i].Contains("vk.com"))
                    {
                        
                        if (l == 0)
                        {
                            DataGridViewRow row = new DataGridViewRow();

                            //row.HeaderCell.Size.Width = 100;
                            row.HeaderCell.Value = temp[i];
                            row2.HeaderCell.Value = temp[i];
                            //dataGridView2.CurrentRow.HeaderCell.Value = temp[i]; 
                           // row.SetValues("Вступить");
                           // dataGridView2.Rows.Add(row);
                        }
                        if (l == 1)
                        {
                            DataGridViewCell cell = new DataGridViewTextBoxCell();
                            cell.Value = temp[i];
                            row2.Cells.Add(cell);
                            //comboBox1.Items.Add(temp[i]);
                            DataGridViewButtonCell but = new DataGridViewButtonCell();
                            
                            row2.Cells.Add(but);
                            //row2.Cells[0].Value = temp[i];
                            //dataGridView2.CurrentRow.Cells[0].Value = temp[i];
                            //row.HeaderCell.Size.Width = 100;


                            dataGridView2.Rows.Add(row2);

                        }
                        l++;
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.ColumnIndex > -1)
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0)
                {
                    //TODO - Button Clicked - Execute Code Here
                    if (senderGrid.CurrentCell.Value.Equals("Вступить"))
                    {
                        if (Join_Groups(vk, dataGridView2.Rows[e.RowIndex].HeaderCell.Value.ToString()))
                        {
                            senderGrid.CurrentCell.Value = "Выйти";
                            dataGridView2.CurrentCell.Style.BackColor = Color.Green;
                        }
                    }
                    else
                    {
                        if (Leave_Groups(vk, dataGridView2.Rows[e.RowIndex].HeaderCell.Value.ToString()))
                        {
                            senderGrid.CurrentCell.Value = "Вступить";
                            dataGridView2.CurrentCell.Style.BackColor = Color.Red;
                        }
                    }


                }
            }
        }
        public bool Join_Groups(VkNet.VkApi vk, string href_)
        {
            long guest_gruopID=0;
            try
            {
                 guest_gruopID = vk.Utils.ResolveScreenName(href_.Substring(href_.LastIndexOf('/') + 1)).Id.Value;
                return vk.Groups.Join(guest_gruopID);
            }
            catch(VkNet.Exception.CaptchaNeededException ex)
            {

                MessageBox.Show("ВК требует капчу! К сожалению, Вам придется воити в ВК через браузер и " +
                    "попробовать вступить в какое-нибудь сообщество, что бы ВК предложил ввести капчу, там ее и введите. " +
                    "Затем можно вернуться в программу");
                
                return false;
            }
            catch(VkNet.Exception.VkApiException ex)
            {
                MessageBox.Show(string.Format("Ошибка: {0}" + Environment.NewLine + "Для продолжения нажмити ОК", ex.Message));
                return false;
            }
           
           // return false;
        }
        public bool Leave_Groups(VkNet.VkApi vk, string href_)
        {
            try
            {
                var guest_gruopID = vk.Utils.ResolveScreenName(href_.Substring(href_.LastIndexOf('/') + 1)).Id.Value;
                return vk.Groups.Leave(guest_gruopID);
            }
            catch (VkNet.Exception.CaptchaNeededException ex)
            {

                MessageBox.Show("ВК требует капчу! К сожалению, Вам придется воити в ВК через браузер и " +
                    "попробовать вступить в какое-нибудь сообщество, что бы ВК предложил ввести капчу, там ее и введите. " +
                    "Затем можно вернуться в программу");
                return false;
            }
            catch (VkNet.Exception.VkApiException ex)
            {
                MessageBox.Show(string.Format("Ошибка: {0}" + Environment.NewLine + "Для продолжения нажмити ОК", ex.Message));
                return false;
            }

            // return false;
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name_out = string.Empty;
            long group = 0;
            if(!textBox1.Text.Equals("Ссылка на группу"))
            {
                var intOsnova =group= vk.Utils.ResolveScreenName(textBox1.Text.Substring(textBox1.Text.LastIndexOf('/') + 1)).Id.Value;
                string osnova = intOsnova + ".xml";
                name_out = osnova;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Сохранить или добавить в базу данных";
            sfd.FileName = name_out;
            
            if(sfd.ShowDialog()==DialogResult.OK)
            {
                try
                {
                    if (!File.Exists(sfd.FileName))
                    {
                        DataSet ds = new DataSet(); // создаем пока что пустой кэш данных
                        DataTable dt = new DataTable(); // создаем пока что пустую таблицу данных
                        dt.TableName = group.ToString(); // название таблицы
                        dt.Columns.Add("Group"); // название колонок
                        dt.Columns.Add("User");
                        dt.Columns.Add("State");
                        //dt.Columns.Add("Programmer");
                        ds.Tables.Add(dt); //в ds создается таблица, с названием и колонками, созданными выше

                        for (int i = 0; i < groups_id.Count; i++) // пока в dataGridView1 есть строки
                        {
                            DataRow row = ds.Tables[0].NewRow(); // создаем новую строку в таблице, занесенной в ds
                            row["Group"] = groups_id[i];  //в столбец этой строки заносим данные из первого столбца dataGridView1
                            row["User"] = users_id[i]; // то же самое со вторыми столбцами
                            row["State"] = dataGridView2.Rows[i].Cells[2].Value;                           //row["Programmer"] = r.Cells[2].Value; //то же самое с третьими столбцами
                            ds.Tables[group.ToString()].Rows.Add(row); //добавление всей этой строки в таблицу ds.
                        }
                        ds.WriteXml(sfd.FileName);
                    }
                    else
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(sfd.FileName);
                        for (int i = 0; i < groups_id.Count; i++) // пока в dataGridView1 есть строки
                        {
                            DataRow row = ds.Tables[0].NewRow(); // создаем новую строку в таблице, занесенной в ds
                            row["Group"] = groups_id[i];  //в столбец этой строки заносим данные из первого столбца dataGridView1
                            row["User"] = users_id[i]; // то же самое со вторыми столбцами
                            row["State"] = dataGridView2.Rows[i].Cells[2].Value;                   //row["Programmer"] = r.Cells[2].Value; //то же самое с третьими столбцами
                            ds.Tables[0].Rows.Add(row); //добавление всей этой строки в таблицу ds.

                        }
                      
                        ds.WriteXml(sfd.FileName);
                    }
                    MessageBox.Show("XML файл успешно сохранен.", "Выполнено.");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Невозможно сохранить XML файл. "+ex.Message, "Ошибка.");
                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();


            DataGridViewTextBoxColumn text_col = new DataGridViewTextBoxColumn();
            text_col.HeaderText = "Участники*";
            dataGridView2.Columns.Add(text_col);

            //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // DataGridViewTextBoxColumn text_col_for_us = new DataGridViewTextBoxColumn();
            DataGridViewButtonColumn col = new DataGridViewButtonColumn();
            col.HeaderText = "Действие**";
            //col.Name = "ff";
            col.Text = "Вступить";
            col.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(col);
            DataGridViewCheckBoxColumn checkBoc_c = new DataGridViewCheckBoxColumn();
            checkBoc_c.FalseValue = false;
            checkBoc_c.TrueValue = true;

            checkBoc_c.HeaderText = "Состояние";
            checkBoc_c.Visible = false;

            dataGridView2.Columns.Add(checkBoc_c);
            OpenFileDialog ofd = new OpenFileDialog();
            DataSet ds = new DataSet();
            if(ofd.ShowDialog()==DialogResult.OK)
            {
                ds.ReadXml(ofd.FileName);
                textBox1.Text = "http://vk.com/club" + ds.Tables[0].TableName;
                var result = (from DataRow dRow in ds.Tables[0].Rows
                                    select new { col1 = dRow["Group"], col2 = dRow["User"], col3 =dRow["State"] }).Distinct();

                foreach (var row in result)
                {
                    DataGridViewRow dd = new DataGridViewRow();
                    dd.HeaderCell.Value = string.Format("http://vk.com/club{0}", row.col1.ToString());
                   
                   // dd.Cells[0].Value = 


                    DataGridViewCell cell = new DataGridViewTextBoxCell();
                    cell.Value = string.Format("http://vk.com/id{0}", row.col2.ToString()); ;
                    dd.Cells.Add(cell);
                    //comboBox1.Items.Add(temp[i]);
                    DataGridViewButtonCell but = new DataGridViewButtonCell();

                    dd.Cells.Add(but);

                    DataGridViewCheckBoxCell cc = new DataGridViewCheckBoxCell();
                    cc.Value = Convert.ToBoolean(row.col3);
                    dd.Cells.Add(cc);
                    //row2.Cells[0].Value = temp[i];
                    //dataGridView2.CurrentRow.Cells[0].Value = temp[i];
                    //row.HeaderCell.Size.Width = 100;


                    //dataGridView2.Rows.Add(row2);
                    if (!Convert.ToBoolean(cc.Value))
                    {
                        dd.Cells[0].Style.ForeColor = Color.Gold;
                        dd.Cells[0].Style.BackColor = Color.Red;
                    }
                    else
                        dd.Cells[0].Style.BackColor = Color.LightGreen;

                    dataGridView2.Rows.Add(dd);
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           if(checkBox1.Checked)
            {
               foreach(DataGridViewRow row in dataGridView2.Rows)
                {
                    row.Visible = !Convert.ToBoolean(row.Cells[2].Value);
                }
            }
           else
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    row.Visible = true;
                }
            }
            dataGridView2.Refresh();
        }
    }
    }

