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
    public partial class Chat_work : Form
    {
        public Chat_work()
        {
            InitializeComponent();
            frends_dt.TableName = "Friends";
            for(int i=0;i<dataGridView1.ColumnCount;i++)
            {
                frends_dt.Columns.Add(new DataColumn(dataGridView1.Columns[i].Name));
                dataGridView1.Columns[i].DataPropertyName = dataGridView1.Columns[i].Name;
            }
            dataGridView1.DataSource = frends_dt;
            for (int i = 0; i < dataGridView3.ColumnCount; i++)
            {
               chats_user.Columns.Add(new DataColumn(dataGridView3.Columns[i].Name));
                dataGridView3.Columns[i].DataPropertyName = dataGridView3.Columns[i].Name;
            }
            dataGridView3.DataSource = chats_user;
            //foreach (DataGridViewColumn d in dataGridView1.Columns)
            //{
            //    frends_dt.Columns.Add(d.HeaderText);

            //}
        }
        public VkNet.VkApi vk { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {

                var gr_id = vk.Utils.ResolveScreenName(textBox1.Text.Substring(textBox1.Text.LastIndexOf(@"/") + 1));
                if (gr_id != null)
                {
                    textBox2.Text = gr_id.Id.Value.ToString();
                    label3.Text = gr_id.Type.ToString();
                }
                else
                {
                    textBox2.Text = "Ссылка не рабочая";
                }
            }
        }
        DataTable frends_dt = new DataTable();
       // BindingSource SBind = new BindingSource();
        private void button2_Click(object sender, EventArgs e)
            
        {
            if (dataGridView1.Rows.Count > 1)
                frends_dt.Rows.Clear();
            var all_fr = vk.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams() { Fields = VkNet.Enums.Filters.ProfileFields.All });
         
            if (checkBox1.Checked)
            {
               
                //var fr = vk.Friends.GetOnlineEx(new VkNet.Model.RequestParams.FriendsGetOnlineParams() { Order = VkNet.Enums.SafetyEnums.Frien//dsOrder.Hints });
                var result = from data in all_fr where (data.Online == true || data.OnlineMobile == true) select data;
                foreach (var res in result)
                {
                    frends_dt.Rows.Add(res.FirstName + " " + res.LastName, res.Id, false);
                 

                }
               
            }
            else
            {
                //var all_fr = vk.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams() { Fields = VkNet.Enums.Filters.ProfileFields.All });
                foreach (var res in all_fr)
                {
                    frends_dt.Rows.Add(res.FirstName + " " + res.LastName, res.Id, false);

                }
            }
            
           // dataGridView1.AutoGenerateColumns = false;
         
            //dataGridView1.DataMember = frends_dt.TableName;
            
            dataGridView1.Refresh();
            label4.Text = (dataGridView1.Rows.Count).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 1)
                dataGridView2.Rows.Clear();
            //  vk.Messages.GetHistory(new VkNet.Model.RequestParams.MessagesGetHistoryParams() { })
            var diag = vk.Messages.GetDialogs(new VkNet.Model.RequestParams.MessagesDialogsGetParams() {  });
            var dialogs = vk.Messages.GetDialogs(new VkNet.Model.RequestParams.MessagesDialogsGetParams() { Count=200 });
            foreach (var f in dialogs.Messages)
            {
               if( f.UsersCount>1)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = f.Title;
                    row.Cells[1].Value = f.ChatId;
                    row.Cells[2].Value = f.UsersCount;
                    dataGridView2.Rows.Add(row);
                }
            }
        }
        DataTable chats_user = new DataTable("Chats_User");
        private void get_users_of_chat_Click(object sender, EventArgs e)
        {
  //          dataGridView3.Rows.Clear();
            chats_user.Clear();
            if (!this.dataGridView2.Rows[this.rowIndex].IsNewRow)
            {
                List<long> _chatID =new List<long>();
                _chatID.Add(Convert.ToInt64(this.dataGridView2.Rows[this.rowIndex].Cells[1].Value));
               var chat_user = vk.Messages.GetChatUsers(_chatID,VkNet.Enums.Filters.UsersFields.All,VkNet.Enums.SafetyEnums.NameCase.Nom);

                foreach(var d in chat_user)
                {
                    chats_user.Rows.Add(d.Id, d.FirstName + " " + d.LastName);
                   
                }
                dataGridView3.Refresh();
            }
        }
        private int rowIndex = 0;
        private void dataGridView2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                
                this.dataGridView2.Rows[e.RowIndex].Selected = true;
                this.rowIndex = e.RowIndex;
                this.dataGridView2.CurrentCell = this.dataGridView2.Rows[e.RowIndex].Cells[1];
                this.contextMenuStrip1.Show(this.dataGridView2, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox3.Text!=string.Empty)
            {
                var users = from DataRow row in frends_dt.Rows where row.ItemArray[2].Equals("True") select Convert.ToUInt64(row.ItemArray[1]);

                // var users = from DataGridViewRow data in dataGridView1.Rows.Cast<DataGridViewRow>() where () select data;
                try
                {
                    if (users.Count() > 1)
                    {
                        var chat = vk.Messages.CreateChat(users, textBox3.Text);
                        if (chat > 0)
                            button3_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Для создания беседы(чата) необходимо в списке друзей отметить хотя бы 2 аккаунта");
                    }
                 }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка :" + ex.Message);
                }
                finally
                {
                    button3_Click(sender, e);

                }

            }
            else
            {
                MessageBox.Show("Необходимо задать название беседы");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var d = vk.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams() { ChatId = Convert.ToInt64(this.dataGridView2.Rows[this.rowIndex].Cells[1].Value), Message = richTextBox1.Text });
                if(d>0)
                {
                    button5.FlatAppearance.BorderColor = Color.LightGreen;
                    System.Threading.Thread.Sleep(200);
                    button5.FlatAppearance.BorderColor = Color.Black;
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка :" + ex.Message);
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                
                foreach(DataRow row in frends_dt.Rows)
                {
                    row.SetField<bool>(2,true);   
                }
                dataGridView1.Refresh();
            }
            else
            {
                foreach (DataRow row in frends_dt.Rows)
                {
                    row.SetField<bool>(2, false);
                }
                dataGridView1.Refresh();
            }
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            this.rowIndex = dataGridView2.CurrentRow.Index;
            get_users_of_chat_Click(sender, e);
            
            UriBuilder urbld = new UriBuilder(string.Format("https://vk.com/im?sel=c{0}", (Convert.ToInt64(this.dataGridView2.Rows[this.rowIndex].Cells[1].Value))));
           // webBrowser1.Navigate(urbld.Uri);
            //geckoWebBrowser1.Navigate( string.Format("https://vk.com/im?sel=c{0}", (Convert.ToInt64(this.dataGridView2.Rows[this.rowIndex].Cells[1].Value))));
            //chromiumWebBrowser1.LoadUrl(urbld.Uri.ToString());
            //webKitBrowser1.Url = urbld.Uri ;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BlackList bl = new BlackList();
            bl.vk = vk;
            bl.ShowDialog();
            if (File.Exists("blacklist.xml"))
            {
                bl_dt_.Rows.Clear();
                //bl_dt_.ReadXmlSchema("blacklist.xml");
                bl_dt_.ReadXml("blacklist.xml");
                // dataGridView1.Refresh();
                label8.Text = bl_dt_.Rows.Count.ToString();
            }
        }
        public DataTable bl_dt_ = new DataTable("BlackList");
        private void Chat_work_Load(object sender, EventArgs e)
        {
            bl_dt_.Columns.Add("Link");
            bl_dt_.Columns.Add("UserID");
            
            if (File.Exists("blacklist.xml"))
            {
                //bl_dt_.ReadXmlSchema("blacklist.xml");
                bl_dt_.ReadXml("blacklist.xml");
               // dataGridView1.Refresh();
                label8.Text = bl_dt_.Rows.Count.ToString();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(textBox2.Text!=string.Empty && !textBox2.Text.Equals("Ссылка не рабочая"))
            {
                bl_dt_.Rows.Add(textBox1.Text, textBox2.Text);
                label8.Text = bl_dt_.Rows.Count.ToString();
                bl_dt_.WriteXml("blacklist.xml");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            get_users_of_chat_Click(sender, e);
            var chat = from DataRow row in chats_user.Rows select Convert.ToInt64(row.ItemArray[0].ToString());
            var blacklist = from DataRow row in bl_dt_.Rows select Convert.ToInt64(row.ItemArray[1].ToString());
            var result = chat.Intersect<long>(blacklist);
            if(result.Count()>0)
            {
                if(MessageBox.Show(string.Format("Найдены участники беседы, входящие в ЧС - {0}. Исключить их из беседы?", result.Count()),"Внимание!",MessageBoxButtons.YesNo)==DialogResult.Yes)
                {
                    foreach (var res in result)
                        vk.Messages.RemoveChatUser(Convert.ToUInt64(this.dataGridView2.Rows[this.rowIndex].Cells[1].Value), res);

                    get_users_of_chat_Click(sender, e);
                    button3_Click(sender, e);
                }
                
            }
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var users = from DataRow row in frends_dt.Rows where row.ItemArray[2].Equals("True") select Convert.ToInt64(row.ItemArray[1]);
            if(users.Count()>0)
            {
                foreach (var user in users)
                {
                    try
                    {
                        vk.Messages.AddChatUser(Convert.ToInt64(this.dataGridView2.Rows[this.rowIndex].Cells[1].Value), user);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(string.Format("Пользователь {0}  уже является собеседником", this.dataGridView2.Rows[this.rowIndex].Cells[0].Value));
                    }
                }
                get_users_of_chat_Click(sender, e);
                button3_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Необходимо отметить те аккаунты в списке друзей, которые надо добавить в чат(беседу)");
            }
        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //label10.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
           
            var ban_list = vk.Account.GetBanned(0,200);
            List<VkNet.Model.User> total_list = new List<VkNet.Model.User>();
            if(ban_list.Count>200)
            {
                for(ulong i=0;i<(ban_list.Count/200)+1;i++)
                {
                    var temp = vk.Account.GetBanned(Convert.ToInt32( i) * 200, 200);
                    total_list.AddRange(temp.Items);
                }
            }
            else
            {
                total_list.AddRange(ban_list.Items);
            }
            var dd = from data in total_list select ("http://vk.com/id"+data.Id.ToString());
            File.WriteAllLines("ban_list.txt", dd);
            Process.Start("ban_list.txt");

        }
    }
}
