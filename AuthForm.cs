using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using VkNet.Utils;
using MySql.Data;
namespace DefectorVK
{
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            label4.Visible = true;
            linkLabel1.Visible = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length<32 || textBox1.Text.Length > 32)
            {
                textBox1.BackColor = Color.Pink;
            }
            else
                if(textBox1.Text.Length==32)
                {
                textBox1.BackColor = Color.LightGreen;
                int num = 12000;
                if(CheckCode(textBox1.Text, ref num))
                {
                    label2.Text = "КОД ПРИНЯТ";
                    const string userRoot = "HKEY_CURRENT_USER";
                    const string subkey = "DefectorVK";
                    const string keyName = userRoot + "\\" + subkey;
                    Microsoft.Win32.Registry.SetValue(keyName, "key", textBox1.Text);
                    numeric_of_code = num;
                    check_code = true;
                }
                else
                {
                    textBox1.BackColor = Color.Pink;
                    label2.Text = "КОД НЕ ВЕРНЫЙ";
                    check_code = false;
                }
                }
        }
        public bool CheckCode(string code,ref int num)
        {
            bool check_auth = false;
            MD5 md5Hash = MD5.Create();
           // string hash = GetMd5Hash(md5Hash, code);
            Codes cod = new Codes();
            for(int i=0;i<cod.code.Length;i++)
            
            {
                

                //   // string hash = GetMd5Hash(md5Hash, source);
                if (VerifyMd5Hash(md5Hash, cod.code[i], code))
                {
                    check_auth = true;
                    num = i;    
                    
                    //MessageBox.Show("Авторизация успешна", "ВНИМАНИЕ", MessageBoxButtons.OK);
                    //return true;
                    break;
                }
                else
                {
                    check_auth = false;
                    //MessageBox.Show("Авторизация не прошла", "ВНИМАНИЕ", MessageBoxButtons.OK);
                    //return false;
                    
                }
            }
            //}
            if (check_auth)
            {
                //MessageBox.Show("Авторизация успешна", "ВНИМАНИЕ", MessageBoxButtons.OK);
                return true;
            }
            else
            {
                //MessageBox.Show("Авторизация не прошла", "ВНИМАНИЕ", MessageBoxButtons.OK);
                return false;
               
            }
           
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool check_code = false;
        public bool trial_code = false;
        public int numeric_of_code { get; set; }
        public int num_code { get; set; }
        private void AuthForm_Load(object sender, EventArgs e)
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "DefectorVK";
            const string keyName = userRoot + "\\" + subkey;
            textBox1.Text=(string)Microsoft.Win32.Registry.GetValue(keyName, "key", "Введите ключ доступа");
            int num_code_ = 12000;
            if (CheckCode(textBox1.Text,ref num_code_))
            {
                numeric_of_code = num_code_;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                if (LoadSettingsFromReg() == 0)
                {
                    VkNet.VkApi vk = new VkNet.VkApi();
                    var time = vk.Utils.GetServerTime();
                    SaveSettingsToReg(DateTimeToDenTime(time));
                }
                else
                {
                    VkNet.VkApi vk = new VkNet.VkApi();
                    var nowtime = vk.Utils.GetServerTime();
                    var start_time = ConvertFromDenTimestamp(LoadSettingsFromReg());
                    start_time = start_time.AddDays(3);
                    if ((start_time - nowtime).TotalDays > 0 && (start_time - nowtime).TotalDays <= 3)
                    {
                        TimeSpan s = (start_time - nowtime);

                        string mess = string.Format("{0} д. {1} ч. {2} мин.", (int)s.TotalDays, (int)s.Hours, (int)s.Minutes);
                        label2.Text = mess;
                        trial_code = true;
                        numeric_of_code = 12001;
                        //this.DialogResult = DialogResult.OK;
                    }
                    else
                    {

                        label2.Text = "Период бесплатного использования закончен";
                        textBox1.Visible = true;
                        label4.Visible = true;
                        linkLabel1.Visible = true;
                        int num = 12000;
                        if (CheckCode(textBox1.Text, ref num))
                        {
                            numeric_of_code = num;
                            check_code = true; // this.DialogResult = DialogResult.OK;
                        }
                        //this.DialogResult = DialogResult.Cancel;
                    }
                }
            }
        }
        void SaveSettingsToReg(double dt)
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "System";
            const string keyName = userRoot + "\\" + subkey;
            Microsoft.Win32.Registry.SetValue(keyName, "dt_start", dt);
            label2.Text = "Перезапустите программу";
            //Microsoft.Win32.Registry.SetValue(keyName, "Pass", textBox2.Text);

        }
        public double LoadSettingsFromReg()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "System";
            const string keyName = userRoot + "\\" + subkey;
           
            object dd = Microsoft.Win32.Registry.GetValue(keyName, "dt_start", 0);
            return Convert.ToDouble(dd);
            //textBox2.Text = (string)Microsoft.Win32.Registry.GetValue(keyName, "Pass", "");
        }
        static DateTime ConvertFromDenTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1985, 10, 31, 10, 45, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        public double DateTimeToDenTime(DateTime dt)
        {
            TimeSpan s = new TimeSpan();
            DateTime origin = new DateTime(1985, 10, 31, 10, 45, 0, 0);
            s = dt.Subtract(origin);
           // s.TotalSeconds

            return s.TotalSeconds;
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            if (check_code || trial_code)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("https://vk.com/id396308382");
        }
    }
}
