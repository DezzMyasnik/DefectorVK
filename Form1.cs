// бесплатная версия
//#define FREE
//#undef FULL
//------------------------
//полная версия
#define FULL
#undef FREE
//---Milana
//#define MILANA
//#undef FREE
//#undef FULL
//
//------------------------
//версия для взаимодействия
//#define FULL_VS
//#undef FULL
//#undef FREE
//------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using Microsoft.Office.Interop.Excel;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
 using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
namespace DefectorVK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
#if (FREE)
            label5.Visible = true;
            label6.Visible = true;
             pictureBox3.Visible = false;
            выбратьФонToolStripMenuItem.Visible = false;
#endif
#if (MILANA)
            label5.Visible = false;
            label6.Visible = false;
            label5.ForeColor = Color.Crimson;
            label5.Text = "Специально для проектов Миланы Премудрой";
#endif
#if (FULL)

            //label5.Visible = false;
            //label6.Visible = false;
            //pictureBox3.Visible = false;

#endif


#if (FULL_VS)
            label5.Visible = true;
            label6.Visible = true;
             pictureBox3.Visible = false;
            label6.Text = "Для взаимовыгодного сотрудничества"; 
#endif


        }

        VkApi vk = new VkApi();
#if (FREE)
        public ulong appID = 5937208; // 	, для бесплатной версии 5937208
        Bitmap bmp1 = Properties.Resources.подложка;
        Bitmap bmp2 = Properties.Resources.Dark_Hd_Wallpapers_HD_Collection;
        Bitmap bmp3 = DefectorVK.Properties.Resources.Bankoboev_Ru_temnye_oboi_podsvechennye_po_centru;
        Bitmap bmp4 = DefectorVK.Properties.Resources.bliki_krugi_fon_temnyy_1920x1080;
        Bitmap bmp5 = DefectorVK.Properties.Resources.xIomOwQlnL4;
        Bitmap bmp6 = DefectorVK.Properties.Resources.wH9_z8WM5RI;

#endif
#if (MILANA)
        public ulong appID = 5962586; // defector for premudraya +3 человека по репосту def_new2 5962586
         Bitmap bmp1 = Properties.Resources.gdSLbof1czw;
         Bitmap bmp2 = DefectorVK.Properties.Resources.IU0cmPHEZK8;
        Bitmap bmp3 = DefectorVK.Properties.Resources.kpMzQIlMm7o;
        Bitmap bmp4 = DefectorVK.Properties.Resources.m5fbipojK6Q;
        Bitmap bmp5 = DefectorVK.Properties.Resources.xIomOwQlnL4;
        Bitmap bmp6 = DefectorVK.Properties.Resources.wH9_z8WM5RI;
#endif
#if (FULL)
        public ulong appID = 6173759;// ID приложения 	5937207, Def_new1 5962584, триальное полное приложение 5962592  6011642
        Bitmap bmp1 = Properties.Resources.подложка;
        Bitmap bmp2 = Properties.Resources.Dark_Hd_Wallpapers_HD_Collection;
        Bitmap bmp3 = DefectorVK.Properties.Resources.Bankoboev_Ru_temnye_oboi_podsvechennye_po_centru;
        Bitmap bmp4 = DefectorVK.Properties.Resources.bliki_krugi_fon_temnyy_1920x1080;
        Bitmap bmp5 = DefectorVK.Properties.Resources.xIomOwQlnL4;
        Bitmap bmp6 = DefectorVK.Properties.Resources.wH9_z8WM5RI;
        //
#endif
#if (FULL_VS)
        public ulong appID = 5937210; //версия для взаимного сотрудничества на данный момент 5937210 - версия для рижского должно быть 4 участника


        Bitmap bmp1 = Properties.Resources.подложка;
        Bitmap bmp2 = Properties.Resources.Dark_Hd_Wallpapers_HD_Collection;
        Bitmap bmp3 = DefectorVK.Properties.Resources.Bankoboev_Ru_temnye_oboi_podsvechennye_po_centru;
        Bitmap bmp4 = DefectorVK.Properties.Resources.bliki_krugi_fon_temnyy_1920x1080;
        Bitmap bmp5 = DefectorVK.Properties.Resources.xIomOwQlnL4;
        Bitmap bmp6 = DefectorVK.Properties.Resources.wH9_z8WM5RI;
#endif
        public string GetAccess_tocen(ref string token, ref long UserID)
        {
            // WebRequest request = WebRequest.Create("https://oauth.vk.com/authorize?client_id=6173759&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=friends&response_type=token&v=5.92&state=123456");
            string url = @"https://oauth.vk.com/authorize?client_id=6173759&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=friends&response_type=token&v=5.92&state=123456";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.AllowAutoRedirect = false;  // IMPORTANT

            webRequest.Timeout = 10000;           // timeout 10s
            webRequest.Method = "HEAD";
            // Get the response ...
            HttpWebResponse webResponse;
            using (webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                // Now look to see if it a redirect
                var s = webResponse.ResponseUri.AbsoluteUri;
                if ((int)webResponse.StatusCode >= 300 && (int)webResponse.StatusCode <= 399)
                {
                    string uriString = webResponse.Headers["Location"];

                    Console.WriteLine("Redirect to " + uriString ?? "NULL");
                    webResponse.Close(); // don't forget to close it - or bad things happen!
                }

            }
            return url;
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            // ulong appID = 5763120;                      
            string email = textBox1.Text;         // email или телефон
            string pass = textBox2.Text;               // пароль для авторизации
           // Settings scope = Settings.All;    // Приложение имеет доступ к друзьям
            Settings scope = Settings.All;
            try
            {
                if (!checkBox2.Checked)
                {
                    string acc_tock = String.Empty;
                    long userID_ = 0;
                   // GetAccess_tocen(ref acc_tock, ref userID_);
                    //vk.Authorize(new IApiAuthParams { ApiAuthParams})
                    vk.Authorize(new ApiAuthParams
                    {
                        ApplicationId = appID,
                       
                        //AccessToken = "2a0e2f4716f188515801ecde3581141c95f366b78eb4f17e2f4ee810101b2bbc98fdb851797159b51bf1c",
                        Login = email,
                        Password = pass,
                        Settings = scope

                    });
                }
                else
                {
                    vk.Authorize(new ApiAuthParams
                    {
                        ApplicationId = appID,
                        Login = email,
                        Password = pass,
                        Settings = scope,
                        Host = textBox3.Text,
                        Port = Convert.ToInt32(textBox4.Text)
                    });
                }
                // vk.Authorize(appID, email, pass, scope);

            }
            catch (VkNet.Exception.CaptchaNeededException ex)
            {
                CaptchaMesseeg capt = new CaptchaMesseeg();
                capt.captcha_pict = ex.Img.AbsoluteUri;
                if (capt.ShowDialog() == DialogResult.OK)
                {
                    if (!checkBox2.Checked)
                    {
                        vk.Authorize(new ApiAuthParams
                        {
                            ApplicationId = appID,
                            
                            Login = email,
                            Password = pass,
                            Settings = scope,
                            CaptchaSid = ex.Sid,
                            CaptchaKey = capt.captcha_res
                        });
                    }
                    else
                    {
                        vk.Authorize(new ApiAuthParams
                        {
                            ApplicationId = appID,
                            Login = email,
                            Password = pass,
                            Settings = scope,
                            Host = textBox3.Text,
                            Port = Convert.ToInt32(textBox4.Text),
                            CaptchaSid = ex.Sid,
                            CaptchaKey = capt.captcha_res
                        });
                    }
                    // CaptchaSid = ex.Sid, CaptchaKey = capt.captcha_res });
                }
            }
            catch (VkNet.Exception.VkApiException ex)
            {
                if (ex.Message.Contains("(401)"))
                    MessageBox.Show("\t\t              Не актуальная версия программы!\n \t\t\t!!!!Обратесь к разработчику!!!!\n \t\tКонтактная информация в разделе О программе");
                else
                    MessageBox.Show(ex.Message);
            }
            if (vk.IsAuthorized)
            {
                try
                {
                    var ui = vk.Account.GetInfo();
                    var gpi = vk.Account.GetProfileInfo();
                    label1.Text = gpi.LastName;
                    label2.Text = gpi.FirstName;
                    
                    var photo = vk.Users.Get(new List<long>() { vk.UserId.Value }, ProfileFields.Photo50);
                    pictureBox1.ImageLocation = photo[0].Photo50.AbsoluteUri;
                }
                catch (VkNet.Exception.VkApiException ex)
                {
                    MessageBox.Show(string.Format("Ошибка: {0}" + Environment.NewLine + "Для продолжения нажмити ОК", ex.Message));
                }
                finally
                {
                    SaveSettingsToReg();
                    //if (Auth(vk, num_code))
                    {
                        frm.vk = vk;
                        frm2.vk = vk;
                        foreach (Control con in this.Controls)
                        {
                            con.Enabled = true;
                        }
                        richTextBox1.Enabled = true;
                        richTextBox2.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        button6.Enabled = true;
                        button7.Enabled = true;
                        button8.Enabled = true;
                        button10.Enabled = true;
                        button11.Enabled = true;
                        button13.Enabled = true;

                    }
                }
            }
        }
        public int num_code { get; set; }
#region Save_Load_Settings
        void SaveSettingsToReg()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "DefectorVK";
            const string keyName = userRoot + "\\" + subkey;
            Microsoft.Win32.Registry.SetValue(keyName, "Login", textBox1.Text);
            Microsoft.Win32.Registry.SetValue(keyName, "Pass", textBox2.Text);
            Microsoft.Win32.Registry.SetValue(keyName, "ProxIp", textBox3.Text);
            Microsoft.Win32.Registry.SetValue(keyName, "ProxPort", textBox4.Text);

        }
        void LoadSettingsFromReg()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "DefectorVK";
            const string keyName = userRoot + "\\" + subkey;
            textBox1.Text = (string)Microsoft.Win32.Registry.GetValue(keyName, "Login", "Введите логин");
            textBox2.Text = (string)Microsoft.Win32.Registry.GetValue(keyName, "Pass", "");
            textBox3.Text = (string)Microsoft.Win32.Registry.GetValue(keyName, "ProxIp", "IP прокси сервера");
            textBox4.Text = (string)Microsoft.Win32.Registry.GetValue(keyName, "ProxPort", "Порт");
        }
#endregion

        public string EncodeLink(long userID,int keyID)
        {
            string url_link = string.Empty;
            url_link = userID.ToString() + "_#^" + keyID.ToString();
            string[] arrya1 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string[] arrya2 = new string[] { "q", "f", "e", "d", "r", "sg", "y", "io", "kl", "j" };
            string res_code = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                url_link = url_link.Replace(arrya1[i], arrya2[i]);
            }
            Encoding enc = Encoding.Default;
            byte[] pass_b = enc.GetBytes(url_link);
            return "http://kitona.ru/defector?usercheck="+Convert.ToBase64String(pass_b);
          
        }
      public class Response
        {
            public string success { get; set; }
            public string error { get; set; }
        }
        public bool Auth2(VkApi vk,int key_id)
        {
            var enc_url = EncodeLink(vk.UserId.Value, key_id);
            WebRequest web_req = WebRequest.Create(enc_url);
            WebResponse response = web_req.GetResponse();
            Response joResponse = new Response();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                    // var str = reader.ReadToEnd();
                    //  resp = new Response();
                     joResponse = JObject.Parse(reader.ReadToEnd()).ToObject<Response>();
                    //JObject ojObject = (JObject)joResponse["success"];

                }
            }
            response.Close();
            if(joResponse.success!=null)
            {
                if(joResponse.success.Contains("user_count_"))
                {
                    var count = Convert.ToInt32(joResponse.success.Substring(11));

                    toolStripStatusLabel3.Text = (3-count).ToString();
                    return true;
                }

               
            }
            else
                if(joResponse.error!=null)
                {
                if(joResponse.error== "error_limit_users")
                {
                    MessageBox.Show("Лимит Аккаунтов кончился. Используйте аккаунт, с которого входили раньше!!!");//выводим сообщение а затем блокируем все конторлыв программе... нажать ни на что НЕЛЬЗЯ  
                    richTextBox1.Enabled = false;
                    button3.Enabled = false;
                    button2.Enabled = false;
                    this.textBox1.Enabled = true;
                    this.textBox2.Enabled = true;
                    this.textBox4.Enabled = true;
                    this.textBox3.Enabled = true;
                    this.button1.Enabled = true;
                    return false;

                }
                if(joResponse.error== "key_id_ban")
                {
                    MessageBox.Show("Ваш ключ не действителен. Обратитесь за покупкой к разработчику.");//выдаем сообщение
                    const string userRoot = "HKEY_CURRENT_USER";
                    const string subkey = "DefectorVK";
                    const string keyName = userRoot + "\\" + subkey;
                    Microsoft.Win32.Registry.SetValue(keyName, "key", "Введите код доступа");// СБРАСЫВАЕМ КЛЮЧ В СИСТЕМЕ,переводим программу на компе в состояние триала
                    MessageBox.Show("Перезапустите программу");
                    richTextBox1.Enabled = false;
                    button3.Enabled = false;
                    button2.Enabled = false;
                    this.textBox1.Enabled = true;
                    this.textBox2.Enabled = true;
                    this.textBox4.Enabled = true;
                    this.textBox3.Enabled = true;
                    this.button1.Enabled = true;
                    return false;
                }
                  
                }

            return false;
       }
        public bool Auth(VkApi vk, int key_id)
        {
            if (key_id < 10000) /// еслик ключ из действительного диапазона
            {
                MySqlConnection connection;
                MySqlDataAdapter adapter;
                //  string[] re_key = key.Split(':');
                DataSet DS = new DataSet();
                connection = new MySqlConnection("server=46.22.211.17;user id=defector;port=3306;persistsecurityinfo=True;database=defector;password=Mj0rxYyF; allowzerodatetime=True");//объявляе сервер бд

                string query = string.Format("SELECT  id, user_id, key_id, info FROM defector where key_id = \'{0}\'", key_id);//строка запроса. есть ли номер ключа  уже в базе. номер ключа программа определяет сама
               

                //осуществляем запрос в базу данных
                adapter = new MySqlDataAdapter(query, connection);//непосредственно выполенене запроса к базе
                try
                {
                    adapter.Fill(DS);//транслируем результат в таплицу DS 
                }
                catch (System.SystemException ex)
                {
                    MessageBox.Show("Нет соединения с сервером: " + ex.Message);
                    return false;
                    //return 0;
                }
                var ban = from DataRow data in DS.Tables[0].Rows where data.Field<string>("info").Equals("BAN") select data; //linq запрос к полученной таблице на наличие забаненных ключей, которы были скопрометированы. (люди которые ключ получили но не заплатили, либо хощяева ключа его дали кому-то но те свалили с ключом)
                if (ban.Count() > 0)//если запрос вернул какой -то результат. количество строк в ответе больше нуля
                {
                    MessageBox.Show("Ваш ключ не действителен. Обратитесь за покупкой к разработчику.");//выдаем сообщение
                    const string userRoot = "HKEY_CURRENT_USER";
                    const string subkey = "DefectorVK";
                    const string keyName = userRoot + "\\" + subkey;
                    Microsoft.Win32.Registry.SetValue(keyName, "key", "Введите код доступа");// СБРАСЫВАЕМ КЛЮЧ В СИСТЕМЕ,переводим программу на компе в состояние триала
                    MessageBox.Show("Перезапустите программу");
                    return false;
                }
                if (DS.Tables[0].Rows.Count==0)//ключ легитимен, но его еще нет в базе на сервере
                {
                    try
                    {
                        string insert = string.Format("INSERT INTO defector ( user_id, key_id) VALUE (\'{0}\',\'{1}\')", vk.UserId, key_id);//занаосим юзера и номер его ключа в базу
                        MySqlCommand command = connection.CreateCommand();
                        command.CommandText = insert;
                        connection.Open();
                        command.ExecuteNonQuery();//непосредственно выполнение команды insert, без подтверждения
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show( ex.Message);
                    }
                    finally
                    {
                        connection.Close();// закрываем подключение к базе
                        
                    }
                   
                }
                else
                    if (DS.Tables[0].Rows.Count < 3)//если ключ в базе есть, но пользователя такого еще в базе нет и количество аккаунтов не превышает трех
                {
                    var chek = from DataRow data in DS.Tables[0].Rows where data.Field<Int32>("user_id").ToString().Equals(vk.UserId.Value.ToString()) select data; //linq запрос к полученной таблице которым проверяем был ли пользователь в базе 
                    if (chek.Count() == 0)//если не было и ключ позволяет еще использовать аккаунты
                    {
                        try
                        {
                            string insert = string.Format("INSERT INTO defector ( user_id, key_id) VALUE (\'{0}\',\'{1}\')", vk.UserId, key_id);//заноси нового пользователя в базу и его ключ
                            MySqlCommand command = connection.CreateCommand();
                            command.CommandText = insert;
                            connection.Open();
                            command.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();

                        }

                    }
                    query = string.Format("SELECT  id, user_id, key_id, info FROM defector where key_id = \'{0}\'", key_id);//делаем новый запрос к базе по обновленным данным


                    //осуществляем запрос в базу данных
                    adapter = new MySqlDataAdapter(query, connection);
                    try
                    {
                        DS.Clear();
                        adapter.Fill(DS);
                    }
                    catch (System.SystemException ex)
                    {
                        MessageBox.Show("Нет соединения с сервером: " + ex.Message);
                        return false;
                        //return 0;
                    }
                    int dd = 3 - DS.Tables[0].Rows.Count;//считаем количество оставшихся для использования аккаунтов
                    toolStripStatusLabel3.Text = dd.ToString();
                    return true;
                }
                else
                    if (DS.Tables[0].Rows.Count == 3)//если количестов аккаунтов к одним ключом достикло 3-х
                {

                    var chek = from DataRow data in DS.Tables[0].Rows where data.Field<Int32>("user_id").ToString().Equals(vk.UserId.Value.ToString()) select data;//проверяем есть ли юзер в базе при 3 аккаунтах с одним ключом
                    //запрос вернул три строки, авторизация юзера в программе прошла у нас есть его id. мы смотрим есть ли этот id в списке уже когда то использовавших программу с этим ключом
                    if (chek.Count() == 0)//если результат 0, то юзер хочет со своим ключом использовать 4-ый аккаунт, а нельзя
                    {
                        MessageBox.Show("Лимит Аккаунтов кончился. Используйте аккаунт, с которого входили раньше!!!");//выводим сообщение а затем блокируем все конторлыв программе... нажать ни на что НЕЛЬЗЯ  
                        richTextBox1.Enabled = false;
                        button3.Enabled = false;
                        button2.Enabled = false;
                        this.textBox1.Enabled = true;
                        this.textBox2.Enabled = true;
                        this.textBox4.Enabled = true;
                        this.textBox3.Enabled = true;
                        this.button1.Enabled = true;
                        return false;
                    }
                    else
                    {
                        int dd = 3 - DS.Tables[0].Rows.Count;//если есть юзер в полученом списке, то пусть пользует прогу
                        toolStripStatusLabel3.Text = dd.ToString();
                        return true;
                    }

                }

            }

            else
            {//момент работы программы в период триального использования
                if (num_code == 12001)//номер ключа для триала 12001
                {
                    MySqlConnection connection;
                    MySqlDataAdapter adapter;

                    //  string[] re_key = key.Split(':');
                    DataSet DS = new DataSet();
                    DataSet DS1 = new DataSet();
                    AuthForm auth = new AuthForm();
                    connection = new MySqlConnection("server=46.22.211.17;user id=defector;port=3306;persistsecurityinfo=True;database=defector;password=Mj0rxYyF; allowzerodatetime=True");
                    string query = string.Format("SELECT  id, user_id, key_id, info FROM defector where info = \'{0}\'", auth.LoadSettingsFromReg());//на каждой машине во время первого запуска формируется уникальное число и записывает в аналы реестра уникальный коротки код... мы его спрашиваем в базе
                    string query1 = string.Format("SELECT  id, user_id, key_id, info FROM defector where user_id = \'{0}\'", vk.UserId);// СПРАШИВАЕМ в базе ид юзера котоый авторизировался в программе
                    adapter = new MySqlDataAdapter(query, connection);
                    try
                    {
                        adapter.Fill(DS);// формируем таблицу с в которой будут все (один) результаы по уникальному числу из реестра
                    }
                    catch (System.SystemException ex)
                    {
                        MessageBox.Show("Нет соединения с сервером: " + ex.Message);
                        return false;
                        //return 0;
                    }

                    adapter = new MySqlDataAdapter(query1, connection);
                    try
                    {
                        adapter.Fill(DS1);//формируем таблицу с в которой будут все (один) результаы по юзеру
                    }
                    catch (System.SystemException ex)
                    {
                        MessageBox.Show("Нет соединения с сервером: " + ex.Message);
                        return false;
                        //return 0;
                    }
                    if (DS.Tables[0].Rows.Count == 0)
                    {
                        try
                        {
                            if (DS1.Tables[0].Rows.Count == 0)//если в перовой выборке (по уникальному числу) и во вторй выборке(по юзеру)  результатов не было, то занаосим в базу юзера и это уникальное число
                            {
                                string insert = string.Format("INSERT INTO defector ( user_id, key_id,  info) VALUE (\'{0}\',\'{1}\',\'{2}\')", vk.UserId, num_code, auth.LoadSettingsFromReg());
                                MySqlCommand command = connection.CreateCommand();
                                command.CommandText = insert;
                                connection.Open();
                                command.ExecuteNonQuery();
                            }
                            else//если результат был найден... (пользователь почистил реестр) удали все упоминания о defectore с копма. но уникальное число не нашел, то мы ему блокируем дальнейшее использование программы даже если триальный период еще не истек и требуем купить ключ
                            {
                                MessageBox.Show("Зафиксирована попытка взлома триальной версии. Приобретите ключ для дальнейшего использования программы.");
                                return false;
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();// закырваем подключение

                        }
                        return true;
                    }
                    else//если уникальное число есть, то проверяем авторизовавшегося юзера с уже внесенным в базу
                    {
                        if (vk.UserId == Convert.ToInt64(DS.Tables[0].Rows[0].ItemArray[1].ToString()))//если это тот же юзер, то пусть пользует триальную версию до окнчания триального периода
                        {
                            return true;
                        }
                        else// если юзер новый а уникальное число старое, то говорим что так делать неправильно
                        {
                            MessageBox.Show("При использовании триальной версии программы доступно применение только одного аккаунта!");
                            return false;
                        }


                    }

                }
                else
                {
                    MessageBox.Show("Ваш ключ не валиден");
                    return false;
                }
            }
            return false;
        }
        IndividualCheck frm = new IndividualCheck();
        Chat_work frm2 = new Chat_work();
        private void Form1_Load(object sender, EventArgs e)
        {
            // vk.Utils.GetServerTime();
            //IndividualCheck frm = new IndividualCheck();
            frm.TopLevel = false;
            frm.Visible = true;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            tabControl1.TabPages[2].Controls.Add(frm);

            frm2.TopLevel = false;
            frm2.Visible = true;
            frm2.FormBorderStyle = FormBorderStyle.None;
            frm2.Dock = DockStyle.Fill;
            LoadSettingsFromReg();
            tabControl1.TabPages[3].Controls.Add(frm2);
            appID = 6159160;
            foreach (Control con in this.Controls)
                {
                   con.Enabled = true;
                }
            //AuthForm auth = new AuthForm();
            //if (auth.ShowDialog() == DialogResult.OK)
            //{
            //    if (auth.trial_code)
            //    {
            //        appID = 6159160;        //5962592 ;// для триального режима
            //    }
            //    foreach (Control con in this.Controls)
            //    {
            //        con.Enabled = true;
            //    }
            //    LoadSettingsFromReg();
            //    num_code = auth.numeric_of_code;
            //    фон1ToolStripMenuItem_Click(sender, e);
            //}
            //else
            //{
            //    foreach (Control con in this.Controls)
            //    {
            //        con.Enabled = false;
            //        frm.Visible = false;
            //        frm2.Visible = false;
            //    }
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
#region DataGrid_formate
        private void button2_Click(object sender, EventArgs e)
        {
            if(dataSet1.Tables.Count==0)
                dataSet1.Tables.Add("Main");

            foreach (var in_str in richTextBox1.Lines)
                {
                    string[] temp = in_str.Split(' ');
                    string[] uri_l = new string[2];
                    int l = 0;
                // System.Data.DataTable dt = new System.Data.DataTable();
                
                    for (int i = 0; i < temp.Length; i++)
                    {

                        if (Uri.IsWellFormedUriString(temp[i], UriKind.Absolute) && temp[i].Contains("vk.com"))
                        {
                            if (l == 0)
                            {
                                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                                //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                col.HeaderText = temp[0] + "\n " + temp[i];
                                col.Width = 25;
                                dataGridView1.Columns.Add(col);
                            }
                            if (l == 1)
                            {
                                DataGridViewRow row = new DataGridViewRow();
                            

                                row.HeaderCell.Value = temp[0] + " " + temp[i];
                                dataGridView1.Rows.Add(row);
                            }
                            l++;
                        }
                    }
                }

           // dataGridView1.DataSource ;

            if (dataGridView1.ColumnCount == 0)
                button5.Visible = true;







        
    } 

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string[] url_c = dataGridView1.Columns[e.ColumnIndex].HeaderText.Split('\n');
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkGoldenrod;
            
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
           // dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkGoldenrod;
          int t =  richTextBox1.Find(url_c[1]);
            richTextBox1.SelectionBackColor = Color.RoyalBlue;
            richTextBox1.Select();
            richTextBox1.Select(t, url_c[1].Length);
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string[] url_r = dataGridView1.Rows[e.RowIndex].HeaderCell.Value.ToString().Split(' ');
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkGoldenrod;
            
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            richTextBox1.SelectionBackColor = Color.RoyalBlue;
            int t = richTextBox1.Find(url_r[1]);
            richTextBox1.Select();
            richTextBox1.Select(t, url_r[1].Length);
        }
       

        private void button4_Click(object sender, EventArgs e)
        {
            users_id.Clear();
            groups_id.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
          
        }
#endregion
        public List<string> users_id = new List<string>();
        public List<string> groups_id =new List<string>();
        // проверка на вступления
       
        private void button3_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
            button2_Click(sender, e);
            //pictureBox2.BackColor = Color.Transparent;
            //pictureBox2.Image = DefectorVK.Properties.Resources.finder_icon__2_;
            this.Cursor = Cursors.WaitCursor;
            if (radioButton1.Checked)
            {
              

                try
                {
                    List<long> us_id = new List<long>();
                    toolStripProgressBar1.Maximum = dataGridView1.Rows.Count;
                    toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                    toolStripStatusLabel1.Text = "Получение ID участников";
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        try
                        {
                            string[] header = row.HeaderCell.Value.ToString().Split(' ');

                            header[1] = header[1].Substring(header[1].LastIndexOf('/') + 1);
                            var id = vk.Utils.ResolveScreenName(header[1]).Id.Value;
                            us_id.Add(id);
                            users_id.Add(id.ToString());
                            toolStripProgressBar1.Value = row.Index+1;
                            
                            toolStripStatusLabel1.Text = string.Format("Получение ID участников: {0}/{1}", row.Index+1, dataGridView1.Rows.Count);
                            statusStrip1.Refresh();
                            

                           
                        }
                        catch(System.Exception ex)
                        {
                            MessageBox.Show("!!!Проверте ссылки участников!!! где-то неправильная ссылка " + ex.Message);
                            break;
                        }
                    }
                    //tolStripStatusLabel1.Text = "Определение вступивших";
                    //us_id.Sort();
                   

                    // List<String> gr_str = new List<string>();
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {
                        string[] col_header = col.HeaderText.Split('\n');
                        col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/'));
                        VkObject gr_id = new VkObject();
                        try
                        {
                            string pattern = @"^(/public+\d)";
                            Regex reg = new Regex(pattern);
                            if (reg.IsMatch(col_header[1]))
                            {
                                col_header[1] = col_header[1].Replace("public", "club");
                            }
                            
                            gr_id = vk.Utils.ResolveScreenName(col_header[1].Substring(1));
                            if (gr_id.Type != VkNet.Enums.VkObjectType.Group && gr_id.Type == VkNet.Enums.VkObjectType.User)
                            {
                                string temp = vk.Users.Get(new List<long>() { gr_id.Id.Value }, ProfileFields.ScreenName)[0].ScreenName;
                               // string mess = string.Format("Повторяется ссылка на http://vk.com/id{0}. ", gr_id.Id.Value);
                                MessageBox.Show(("Среди ссылок на группы найдена ссылка на пользователя http://vk.com/" + temp),"ВНИМАНИЕ!!!", MessageBoxButtons.OK);
                                int t = richTextBox1.Find(temp);
                                if (t != -1)
                                {
                                    richTextBox1.SelectionBackColor = Color.Red;
                                    richTextBox1.Select();
                                    richTextBox1.Select(t, temp.Length);
                                    
                                    break;
                                }
                                
                            }
                            else
                            {
                                groups_id.Add(gr_id.Id.Value.ToString());
                                var defector = vk.Groups.IsMember(gr_id.Id.Value.ToString(), null, us_id, true, true);
                                if (defector.Count != us_id.Count)
                                {
                                    if (MessageBox.Show("Выявлены повторы участников в списке. Произвести автоматическое удаление повторов (ДА). Удалять в ручную (Нет)", "ВНИМАНИЕ!!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        var result = from item in us_id group item by item into pair where pair.Count() != 1 select pair.Key;
                                        var rich_lines = richTextBox1.Lines;
                                        foreach (var res in result)
                                        {
                                            string mess1 = res.ToString();// vk.Users.Get(res, ProfileFields.ScreenName).ScreenName;
                                            string mess = string.Format("Повторяется ссылка на http://vk.com/id{0}. ", mess1);
                                            MessageBox.Show(mess, "Внимание!!!", MessageBoxButtons.OK);
                                            // string mess1 = "http://vk.com/" + vk.Users.Get(res,ProfileFields.ScreenName).ScreenName;

                                            int t = richTextBox1.Find(mess1);
                                            if (t > 0)
                                            {
                                                // Clipboard.SetText(mess1);
                                                //MessageBox.Show("ID участника скопированно в буфер обмена");
                                                richTextBox1.SelectionBackColor = Color.Red;
                                                richTextBox1.Select();
                                                richTextBox1.Select(t, mess1.Length);

                                                //break;
                                            }
                                            //else
                                            if (t == -1)
                                            {
                                                mess1 = vk.Users.Get(new List<long>() { res }, ProfileFields.ScreenName)[0].ScreenName;
                                                //   // Clipboard.SetText(mess1);
                                                //   // MessageBox.Show("ID участника скопированно в буфер обмена");
                                                t = richTextBox1.Find(mess1);
                                                richTextBox1.SelectionBackColor = Color.Red;
                                                richTextBox1.Select();
                                                richTextBox1.Select(t, mess1.Length);
                                                //break;
                                                //MessageBox.Show("ID участника скопированно в буфер обмена");
                                            }
                                            var result_ser = from sel in richTextBox1.Lines where sel.Contains(mess1) select sel;
                                            //var result_ser1 = from sel in richTextBox1.Lines where !sel.Contains(mess1) select sel;
                                            List<string> temp = richTextBox1.Lines.ToList();
                                            temp.Remove(result_ser.ElementAt(0));
                                            richTextBox1.Lines = temp.ToArray();
                                            //richTextBox1.Lines = result_ser.ToArray();
                                            button4_Click(sender, e);
                                            button2_Click(sender, e);
                                        }
                                    }
                                    else
                                    {
                                        var result = from item in us_id group item by item into pair where pair.Count() != 1 select pair.Key;
                                        var rich_lines = richTextBox1.Lines;
                                        foreach (var res in result)
                                        {
                                            string mess1 = res.ToString();// vk.Users.Get(res, ProfileFields.ScreenName).ScreenName;
                                            string mess = string.Format("Повторяется ссылка на http://vk.com/id{0}. ", mess1);
                                            MessageBox.Show(mess, "Внимание!!!", MessageBoxButtons.OK);
                                            // string mess1 = "http://vk.com/" + vk.Users.Get(res,ProfileFields.ScreenName).ScreenName;

                                            int t = richTextBox1.Find(mess1);
                                            if (t > 0)
                                            {
                                                // Clipboard.SetText(mess1);
                                                //MessageBox.Show("ID участника скопированно в буфер обмена");
                                                richTextBox1.Select();
                                                richTextBox1.Select(t, mess1.Length);

                                                break;
                                            }
                                            //else
                                            if (t == -1)
                                            {
                                                mess1 = vk.Users.Get(new List<long>() { res }, ProfileFields.ScreenName)[0].ScreenName; 
                                                //   // Clipboard.SetText(mess1);
                                                //   // MessageBox.Show("ID участника скопированно в буфер обмена");
                                                t = richTextBox1.Find(mess1);
                                                richTextBox1.Select();
                                                richTextBox1.Select(t, mess1.Length);
                                                break;
                                                //MessageBox.Show("ID участника скопированно в буфер обмена");
                                            }
                                        }
                                    }
                                    break;
                                }
                                for (int i = 0; i < defector.Count; i++)
                                {
                                    if (!defector[i].Member)
                                    {
                                        dataGridView1.Rows[i].Cells[col.Index].Value = defector[i].Member;
                                        dataGridView1.Rows[i].Cells[col.Index].Style.BackColor = Color.Red;

                                    }
                                    else
                                    {
                                        dataGridView1.Rows[i].Cells[col.Index].Value = defector[i].Member;
                                        dataGridView1.Rows[i].Cells[col.Index].Style.BackColor = Color.MediumSlateBlue;
                                    }
                                }
                               
                            }
                            dataGridView1.Refresh();
                        }
                        catch (SystemException exer)
                        {
                            MessageBox.Show("Прверте правильность ссылки на группу: " + col.HeaderText + " " + exer.Message);
                        }
                        catch (VkNet.Exception.VkApiException ex)
                        {
                            MessageBox.Show("ошибка" + ex.Message);
                        }




                    }
                    pictureBox2.Image = DefectorVK.Properties.Resources.finder_icon__1_;
                    toolStripStatusLabel1.Text = "Определение завершено";
                }
                catch (VkNet.Exception.AccessTokenInvalidException ex)
                {
                    MessageBox.Show("Вы забыли авторизоваться. Ввойдите в ВК! " + ex.Message);
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    MessageBox.Show("Проверте входные данные1: " + ex.Message);
                }
               
                //catch (SystemException ex)
                //{
                //    MessageBox.Show("Проверте входные данные2: " + ex.Message);
                //}
            }
            else
            if(radioButton2.Checked)
            {
                pictureBox2.Image = DefectorVK.Properties.Resources.finder_icon__2_;

                try
                {
                    List<long> us_id = new List<long>();
                    toolStripProgressBar1.Maximum = dataGridView1.Rows.Count;
                    toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                    toolStripStatusLabel1.Text = "Получение ID участников";
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {
                        string[] header = col.HeaderCell.Value.ToString().Split('\n');

                        header[1] = header[1].Substring(header[1].LastIndexOf('/') + 1);
                        var id = vk.Utils.ResolveScreenName(header[1]).Id.Value;
                        us_id.Add(id);
                        toolStripProgressBar1.Value = col.Index;
                        StringBuilder strb = new StringBuilder();
                        strb.AppendFormat("Получение ID участников: {0}/{1}", col.Index, dataGridView1.Rows.Count);
                        toolStripStatusLabel1.Text = strb.ToString();
                    }

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string[] col_header = row.HeaderCell.Value.ToString().Split(' ');
                        col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string gr_id = "";
                        try
                        {
                            gr_id = vk.Utils.ResolveScreenName(col_header[1]).Id.Value.ToString();
                            var defector = vk.Groups.IsMember(gr_id, null, us_id, true, false);
                            if (defector.Count != us_id.Count)
                            {
                                var result = from item in us_id group item by item into pair where pair.Count() != 1 select pair.Key;
                                foreach (var res in result)
                                {
                                    string mess1 = "http://vk.com/" + vk.Users.Get(new List<long>() { res }, ProfileFields.ScreenName)[0].ScreenName; ;
                                    string mess = string.Format("Выявлено расхождение количества групп и участников, {0} повтояряется в списке участников", mess1);

                                    MessageBox.Show(mess);
                                   

                                    int t = richTextBox1.Find(mess1);
                                    if (t > 0)
                                    {
                                        Clipboard.SetText(mess1);
                                        MessageBox.Show("ID участника скопированно в буфер обмена");
                                        richTextBox1.Select();
                                        richTextBox1.Select(t, mess1.Length);
                                    }
                                    else
                                    if (t == -1)
                                    {
                                        mess1 = "https://vk.com/" + vk.Users.Get(new List<long>() { res }, ProfileFields.ScreenName)[0].ScreenName; 
                                        Clipboard.SetText(mess1);
                                        MessageBox.Show("ID участника скопированно в буфер обмена");
                                        t = richTextBox1.Find(mess1);
                                        richTextBox1.Select();
                                        richTextBox1.Select(t, mess1.Length);
                                        //MessageBox.Show("ID участника скопированно в буфер обмена");
                                    }
                                }
                                break;
                            }

                                for (int i = 0; i < defector.Count; i++)
                                {
                                dataGridView1.Rows[row.Index].Cells[i].Value = defector[i].Member;
                                if (!defector[i].Member)
                                {
                                   // dataGridView1.Rows[row.Index].Cells[i].Value = 
                                  
                                   dataGridView1.Rows[row.Index].Cells[i].Style.BackColor = Color.Red;

                                }
                                else
                                {
                                    //dataGridView1.Rows[i].Cells[row.Index].Value = defector[i].Member;
                                    dataGridView1.Rows[row.Index].Cells[i].Style.BackColor = Color.LimeGreen;
                                }
                            }
                            dataGridView1.Refresh();
                        }
                        catch (SystemException exer)
                        {
                            MessageBox.Show("Прверте правильность ссылки на группу: " + row.HeaderCell.Value.ToString() + " " + exer);
                        }

                        
                    }
                    toolStripStatusLabel1.Text = "Определение вступивших";
                    // List<String> gr_str = new List<string>();
                   
                    pictureBox2.Image = DefectorVK.Properties.Resources.finder_icon__1_;
                    toolStripStatusLabel1.Text = "Определение завершено";
                }
                catch (VkNet.Exception.AccessTokenInvalidException ex)
                {
                    MessageBox.Show("Вы забыли авторизоваться. Ввойдите в ВК! " + ex.Message);
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    MessageBox.Show("Проверте входные данные1: " + ex.Message);
                }
                //catch (SystemException ex)
                //{
                //    MessageBox.Show("Проверте входные данные2: " + ex.Message);
                //}
            }
            this.Cursor = Cursors.Default;
            button8.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            char[] paramss = new char[] { '>', '<', '?' };
            
           richTextBox1.Text = richTextBox1.Text.Replace(paramss[0],' ');
            richTextBox1.Text = richTextBox1.Text.Replace(paramss[1], ' ');
            richTextBox1.Text = richTextBox1.Text.Replace(paramss[2], '*');
            button5.Visible = false;
            //    SaveFileDialog sfd = new SaveFileDialog();
            //    sfd.Filter = "Электронная таблица Excel| *.xlsx";
            //    if (sfd.ShowDialog() != DialogResult.OK)
            //        return;
            //    DataTable dt1 = cExcel.ToDataTable(dataGridView1, "Table1");

            //    if (cExcel.DataTableToExcelFile(dt1, sfd.FileName, true))
            //        MessageBox.Show("Данные выгружены успешно!");
            //    else
            //    {
            //        MessageBox.Show("Ошибка выгрузки данных");
            //    }
        }

        private void ExportToExcel()
        {
            // Creating a Excel object.
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "ExportedFromDatGrid";

                int cellRowIndex = 1;
                int cellColumnIndex = 1;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    worksheet.Cells[cellRowIndex + 1, cellColumnIndex] = dataGridView1.Rows[i].HeaderCell.Value.ToString();
                    cellRowIndex++;
                }
                cellRowIndex = 1;
                cellColumnIndex = 1;
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex + 1] = dataGridView1.Columns[j].HeaderText;
                    cellColumnIndex++;
                }
                cellRowIndex = 1;
                cellColumnIndex = 1;
                //Loop through each row and read value from each column.
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        var val = dataGridView1.Rows[i].Cells[j].Value;
                        if (val != null)
                        {
                            if ((bool)val == true)
                                worksheet.Cells[cellRowIndex + 1, cellColumnIndex + 1] = "+";
                            else
                                worksheet.Cells[cellRowIndex + 1, cellColumnIndex + 1] = "-";
                        }
                        cellColumnIndex++;
                        
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                //Getting the location and file name of the excel to save from user.
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.FilterIndex = 2;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export Successful");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }

        }
        private void button6_Click(object sender, EventArgs e)
        {
            ExportToExcel();
            //Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            //ExcelApp.Application.Workbooks.Add(Type.Missing);
            //ExcelApp.Columns.ColumnWidth = 15;

            //ExcelApp.Cells[1, 1] = "№п/п";
            //ExcelApp.Cells[1, 2] = "Число";
            //ExcelApp.Cells[1, 3] = "Название";
            //ExcelApp.Cells[1, 4] = "Количество";
            //ExcelApp.Cells[1, 5] = "Цена ОПТ";
            //ExcelApp.Cells[1, 6] = "Цена Розница";
            //ExcelApp.Cells[1, 7] = "Сумма";

            //for (int i = 0; i < dataGridView1.ColumnCount; i++)
            //{
            //    for (int j = 0; j < dataGridView1.RowCount; j++)
            //    {
            //        ExcelApp.Cells[j + 2, i + 1] = (dataGridView1[i, j].Value).ToString();
            //    }
            //}
            //ExcelApp.Visible = true;
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //About dlg = new About();
            //dlg.appID = appID;
            //dlg.Show();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Contains('>') || richTextBox1.Text.Contains('<'))
                button5.Visible = true;
#if (FREE)
            if (richTextBox1.Lines.Length > 10)
            {
                MessageBox.Show("Количтесво групп и участников исследования ограничено 10 позициями!!! \nЗагляните в раздел О программе по вопросам приобретения версии без ограничений", "!!!ВНИМАНИЕ!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                string[] buf = richTextBox1.Lines;
                richTextBox1.Clear();
                for (int i = 0; i < 10; i++)
                    richTextBox1.Text += buf[i] + "\n";
            }
#endif

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                label4.Text = "Участники";
                label7.Text = "Сообщества";
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label4.Text = "Сообщества";
                label7.Text = "Участники";
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (radioButton3.Checked)
                {
                    string all_spisok = string.Empty;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string[] col_header = row.HeaderCell.Value.ToString().Split(' ');
                        //col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string ne_vstup = string.Empty;
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (row.Cells[i].Value != null)
                            {
                                if ((bool)row.Cells[i].Value != true)
                                {
                                    string[] col_name = dataGridView1.Columns[i].HeaderText.Split('\n');
                                    // col_name[1] = col_name[1].Substring(col_name[1].LastIndexOf('/') + 1);
                                    ne_vstup += " " + col_name[1] + Environment.NewLine;
                                }
                            }
                        }

                        if (ne_vstup != string.Empty)
                        {
                            string finish_str = string.Format("Пользователь {0} {1} не вступил в :", col_header[0], col_header[1])+Environment.NewLine;
                            ne_vstup += Environment.NewLine;
                            all_spisok += finish_str + ne_vstup;
                        }



                        //Uri ne = new Uri(row.HeaderCell.Value.ToString(), UriKind.RelativeOrAbsolute);
                        //string find_gr = ne.AbsolutePath;
                    }
                    ChekList cl = new ChekList();
                    cl.text = all_spisok;
                    cl.ShowDialog();
                }
                else
                    if(radioButton8.Checked)
                {
                    string all_spisok = string.Empty;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string[] col_header = row.HeaderCell.Value.ToString().Split(' ');
                        //col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string ne_vstup = string.Empty;
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (row.Cells[i].Value != null)
                            {
                                if ((bool)row.Cells[i].Value != true)
                                {
                                    string[] col_name = dataGridView1.Columns[i].HeaderText.Split('\n');
                                    // col_name[1] = col_name[1].Substring(col_name[1].LastIndexOf('/') + 1);
                                    ne_vstup += " " + col_name[0] + ", ";
                                }
                            }
                        }

                        if (ne_vstup != string.Empty)
                        {
                            string finish_str = string.Format("Пользователь {0} {1} не вступил в паблики под этими пунктами :", col_header[0], col_header[1]);
                            ne_vstup += Environment.NewLine;
                            all_spisok += finish_str + ne_vstup;
                        }



                        //Uri ne = new Uri(row.HeaderCell.Value.ToString(), UriKind.RelativeOrAbsolute);
                        //string find_gr = ne.AbsolutePath;
                    }
                    ChekList cl = new ChekList();
                    cl.text = all_spisok;
                    cl.ShowDialog();
                }
                else
                if (radioButton4.Checked)
                {
                    string all_spisok = string.Empty;
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {

                        string[] col_header = col.HeaderCell.Value.ToString().Split('\n');
                        //col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string ne_vstup = string.Empty;
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[col.Index].Value != null)
                            {
                                if ((bool)dataGridView1.Rows[i].Cells[col.Index].Value != true)
                                {
                                    string[] col_name = dataGridView1.Rows[i].HeaderCell.Value.ToString().Split(' ');
                                    // col_name[1] = col_name[1].Substring(col_name[1].LastIndexOf('/') + 1);
                                    ne_vstup += " " + col_name[1] + Environment.NewLine;
                                }
                            }
                        }

                        if (ne_vstup != string.Empty)
                        {
                            string finish_str = string.Format("В сообщество {0} {1} не вступили следующие персонажи :", col_header[0], col_header[1])+Environment.NewLine;
                            ne_vstup += Environment.NewLine;
                            all_spisok += finish_str + ne_vstup;
                        }



                        //Uri ne = new Uri(row.HeaderCell.Value.ToString(), UriKind.RelativeOrAbsolute);
                        //string find_gr = ne.AbsolutePath;
                    }
                    ChekList cl = new ChekList();
                    cl.text = all_spisok;
                    cl.ShowDialog();
                }
                else
                    if(radioButton9.Checked)
                {
                    string all_spisok = string.Empty;
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {

                        string[] col_header = col.HeaderCell.Value.ToString().Split('\n');
                        //col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string ne_vstup = string.Empty;
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[col.Index].Value != null)
                            {
                                if ((bool)dataGridView1.Rows[i].Cells[col.Index].Value != true)
                                {
                                    string[] col_name = dataGridView1.Rows[i].HeaderCell.Value.ToString().Split(' ');
                                    // col_name[1] = col_name[1].Substring(col_name[1].LastIndexOf('/') + 1);
                                    ne_vstup += " " + col_name[0] + ",";
                                }
                            }
                        }

                        if (ne_vstup != string.Empty)
                        {
                            string finish_str = string.Format("В сообщество {0} {1} не вступили пользователи из этих пунктов списка :", col_header[0], col_header[1]);
                            ne_vstup += Environment.NewLine;
                            all_spisok += finish_str + ne_vstup;
                        }



                        //Uri ne = new Uri(row.HeaderCell.Value.ToString(), UriKind.RelativeOrAbsolute);
                        //string find_gr = ne.AbsolutePath;
                    }
                    ChekList cl = new ChekList();
                    cl.text = all_spisok;
                    cl.ShowDialog();
                }
                else
                    if (radioButton5.Checked)
                {

                    string all_spisok = string.Empty;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string[] col_header = row.HeaderCell.Value.ToString().Split(' ');
                        //col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string ne_vstup = string.Empty;
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (row.Cells[i].Value != null)
                            {
                                if ((bool)row.Cells[i].Value != true)
                                {
                                    string[] col_name = dataGridView1.Columns[i].HeaderText.Split('\n');
                                    // col_name[1] = col_name[1].Substring(col_name[1].LastIndexOf('/') + 1);
                                    ne_vstup += " " + col_name[1] + Environment.NewLine;
                                }
                            }
                        }

                        if (ne_vstup != string.Empty)
                        {
                            string[] col_name = dataGridView1.Columns[row.Index].HeaderText.Split('\n');
                            string finish_str = string.Format("Пользователь {0} {1} ( {2} ) не вступил в :", col_header[0], col_header[1],col_name[1] )+Environment.NewLine;
                            ne_vstup += Environment.NewLine;
                            all_spisok += finish_str + ne_vstup;
                        }



                        //Uri ne = new Uri(row.HeaderCell.Value.ToString(), UriKind.RelativeOrAbsolute);
                        //string find_gr = ne.AbsolutePath;
                    }
                    ChekList cl = new ChekList();
                    cl.text = all_spisok;
                    cl.ShowDialog();
                }
                else
                if (radioButton6.Checked)
                {

                    string all_spisok = string.Empty;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string[] col_header = row.HeaderCell.Value.ToString().Split(' ');
                        //col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string ne_vstup = string.Empty;
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (row.Cells[i].Value != null)
                            {
                                if ((bool)row.Cells[i].Value != true)
                                {
                                    string[] col_name = dataGridView1.Columns[i].HeaderText.Split('\n');
                                    // col_name[1] = col_name[1].Substring(col_name[1].LastIndexOf('/') + 1);
                                    ne_vstup += " " + col_name[1] + Environment.NewLine;
                                }
                            }
                        }

                        if (ne_vstup != string.Empty)
                        {
                            int tr_c = 0;

                            for (int i = 0; i < row.Cells.Count; i++)
                                if (row.Cells[i].Value != null)
                                {
                                    if ((bool)row.Cells[i].Value == true)
                                        tr_c++;
                                }
                            //int tr_c = (from items in r where items == true select items).Count(); 
                            string finish_str = string.Format("Пользователь {0} {1} вступил в {2} из {3} ,но не вступил в :", col_header[0], col_header[1], tr_c, row.Cells.Count) + Environment.NewLine;
                            ne_vstup += Environment.NewLine;
                            all_spisok += finish_str + ne_vstup;
                        }



                        //Uri ne = new Uri(row.HeaderCell.Value.ToString(), UriKind.RelativeOrAbsolute);
                        //string find_gr = ne.AbsolutePath;
                    }
                    ChekList cl = new ChekList();
                    cl.text = all_spisok;
                    cl.ShowDialog();

                }
                else
                if(radioButton7.Checked)
                {
                    string all_spisok = string.Empty;
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {

                        string[] col_header = col.HeaderCell.Value.ToString().Split('\n');
                        //col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string ne_vstup = string.Empty;
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[col.Index].Value != null)
                            {
                                if ((bool)dataGridView1.Rows[i].Cells[col.Index].Value != true)
                                {

                                    string[] col_name = dataGridView1.Columns[i].HeaderCell.Value.ToString().Split('\n'); //dataGridView1.Rows[i].HeaderCell.Value.ToString().Split(' ');
                                    // col_name[1] = col_name[1].Substring(col_name[1].LastIndexOf('/') + 1);
                                    ne_vstup += " " + col_name[1] + Environment.NewLine;
                                }
                            }
                        }
                     //  var n_S = from DataGridViewRow ns in dataGridView1.Rows where (bool)ns.Cells[col.Index].Value != true select ns.HeaderCell.Value 
                        if (ne_vstup != string.Empty)
                        {
                            string finish_str = string.Format("В сообщество {0} {1} не вступили люди от этих пабликов :", col_header[0], col_header[1])+Environment.NewLine;
                            ne_vstup += Environment.NewLine;
                            all_spisok += finish_str + ne_vstup;
                        }



                        //Uri ne = new Uri(row.HeaderCell.Value.ToString(), UriKind.RelativeOrAbsolute);
                        //string find_gr = ne.AbsolutePath;
                    }
                    ChekList cl = new ChekList();
                    cl.text = all_spisok;
                    cl.ShowDialog();
                }
            }
            if(radioButton2.Checked)
            {
                if (radioButton3.Checked)
                {
                    string all_spisok = string.Empty;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string[] col_header = row.HeaderCell.Value.ToString().Split(' ');
                        //col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string ne_vstup = string.Empty;
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (row.Cells[i].Value != null)
                            {
                                if ((bool)row.Cells[i].Value != true)
                                {
                                    string[] col_name = dataGridView1.Columns[i].HeaderText.Split('\n');
                                    // col_name[1] = col_name[1].Substring(col_name[1].LastIndexOf('/') + 1);
                                    ne_vstup += " " + col_name[1] + Environment.NewLine;
                                }
                            }
                        }

                        if (ne_vstup != string.Empty)
                        {
                            string finish_str = string.Format("Пользоваетль {0} {1} не вступил в :", col_header[0], col_header[1])+Environment.NewLine;
                            ne_vstup += Environment.NewLine;
                            all_spisok += finish_str + ne_vstup;
                        }



                        //Uri ne = new Uri(row.HeaderCell.Value.ToString(), UriKind.RelativeOrAbsolute);
                        //string find_gr = ne.AbsolutePath;
                    }
                    ChekList cl = new ChekList();
                    cl.text = all_spisok;
                    cl.ShowDialog();
                }
                else
                    if(radioButton4.Checked)
                {
                    string all_spisok = string.Empty;
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {

                        string[] col_header = col.HeaderCell.Value.ToString().Split('\n');
                        //col_header[1] = col_header[1].Substring(col_header[1].LastIndexOf('/') + 1);
                        string ne_vstup = string.Empty;
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[col.Index].Value != null)
                            {
                                if ((bool)dataGridView1.Rows[i].Cells[col.Index].Value != true)
                                {
                                    string[] col_name = dataGridView1.Rows[i].HeaderCell.Value.ToString().Split(' ');
                                    // col_name[1] = col_name[1].Substring(col_name[1].LastIndexOf('/') + 1);
                                    ne_vstup += " " + col_name[1] + Environment.NewLine;
                                }
                            }
                        }

                        if (ne_vstup != string.Empty)
                        {
                            string finish_str = string.Format("В сообщество {0} {1} не вступили следующие персонажи :", col_header[0], col_header[1])+Environment.NewLine;
                            ne_vstup += Environment.NewLine;
                            all_spisok += finish_str + ne_vstup;
                        }



                        //Uri ne = new Uri(row.HeaderCell.Value.ToString(), UriKind.RelativeOrAbsolute);
                        //string find_gr = ne.AbsolutePath;
                    }
                    ChekList cl = new ChekList();
                    cl.text = all_spisok;
                    cl.ShowDialog();
                }
            }

        }

        private void фон1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = bmp1;
            //groupBox1.ForeColor = Color.WhiteSmoke;
            //label3.ForeColor = Color.WhiteSmoke;
            //groupBox3.ForeColor = Color.WhiteSmoke;
            //groupBox4.ForeColor = Color.WhiteSmoke;
            //button1.ForeColor = Color.Black;
            //label4.ForeColor = Color.WhiteSmoke;
            //label1.ForeColor = Color.WhiteSmoke;
            //label7.ForeColor = Color.WhiteSmoke;
            toolStripStatusLabel1.ForeColor = Color.WhiteSmoke;
        }

        private void фон2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // DefectorVK.Properties.Resources.IU0cmPHEZK8
            this.BackgroundImage = bmp2;
            groupBox1.ForeColor = Color.WhiteSmoke;
            label3.ForeColor = Color.WhiteSmoke;
            //groupBox3.ForeColor = Color.WhiteSmoke;
            //groupBox4.ForeColor = Color.WhiteSmoke;
            //button1.ForeColor = Color.Black;
            //label4.ForeColor = Color.WhiteSmoke;
            //label1.ForeColor = Color.WhiteSmoke;
            //label7.ForeColor = Color.WhiteSmoke;
            toolStripStatusLabel1.ForeColor = Color.WhiteSmoke;
        }

        private void фон3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DefectorVK.Properties.Resources.kpMzQIlMm7o
            this.BackgroundImage = bmp3;
            groupBox1.ForeColor = Color.WhiteSmoke;
            label3.ForeColor = Color.WhiteSmoke;
            //groupBox3.ForeColor = Color.WhiteSmoke;
            //groupBox4.ForeColor = Color.WhiteSmoke;
            //button1.ForeColor = Color.Black;
            //label4.ForeColor = Color.WhiteSmoke;
            //label1.ForeColor = Color.WhiteSmoke;
            //label7.ForeColor = Color.WhiteSmoke;
            toolStripStatusLabel1.ForeColor = Color.SlateGray;
        }

        private void фон4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = bmp4;
            groupBox1.ForeColor = Color.WhiteSmoke;
            label3.ForeColor = Color.WhiteSmoke;
            //groupBox3.ForeColor = Color.WhiteSmoke;
            //groupBox4.ForeColor = Color.WhiteSmoke;
            //button1.ForeColor = Color.Black;
            //label4.ForeColor = Color.WhiteSmoke;
            //label1.ForeColor = Color.WhiteSmoke;
            //label7.ForeColor = Color.WhiteSmoke;
            toolStripStatusLabel1.ForeColor = Color.WhiteSmoke;

        }

        private void фон5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = bmp5;
            groupBox1.ForeColor = Color.WhiteSmoke;
            label3.ForeColor = Color.WhiteSmoke;
            //groupBox3.ForeColor = Color.WhiteSmoke;
            //groupBox4.ForeColor = Color.WhiteSmoke;
            //button1.ForeColor = Color.Black;
            //label4.ForeColor = Color.WhiteSmoke;
            //label1.ForeColor = Color.WhiteSmoke;
            //label7.ForeColor = Color.WhiteSmoke;
            toolStripStatusLabel1.ForeColor = Color.WhiteSmoke;
        }

        private void фон6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = bmp6;
            groupBox1.ForeColor = Color.WhiteSmoke;
            label3.ForeColor = Color.WhiteSmoke;
            //groupBox3.ForeColor = Color.WhiteSmoke;
            //groupBox4.ForeColor = Color.WhiteSmoke;
            //button1.ForeColor = Color.Black;
            //label4.ForeColor = Color.WhiteSmoke;
            //label1.ForeColor = Color.WhiteSmoke;
            //label7.ForeColor = Color.WhiteSmoke;
            toolStripStatusLabel1.ForeColor = Color.WhiteSmoke;
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start("chrome.exe", e.LinkText);
        }

        private void richTextBox1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.Show("Для вставки списка используйте клавиши Ctrl+V", richTextBox1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string all_spisok = string.Empty;
            if (groups_id.Count == users_id.Count)
            {
                for(int i=0;i<groups_id.Count;i++)
                {
                    all_spisok += (i + 1).ToString() + ". http://vk.com//club" + groups_id[i] + "\t http://vk.com/id" + users_id[i] + Environment.NewLine; 
                }
                    
            }
            ChekList cl = new ChekList();
            cl.text = all_spisok;
            cl.ShowDialog();
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
            Process.Start("chrome.exe", "https://vk.com/defectorvk");
        }

        private void вставитьCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = Clipboard.GetText();
        }

        private void стеретьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button9_Click(object sender, EventArgs e)
        {

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

        private void оПрограммеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //AboutBox1 box = new AboutBox1();
            //box.Show();
            About dlg = new About();
            dlg.appID = appID;
            dlg.Show();
        
    }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("chrome.exe","https://vk.com/defectorvk?w=page-141917223_52089283");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            foreach (var in_str in richTextBox2.Lines)
            {
                string[] temp = in_str.Split(' ');
                string[] uri_l = new string[2];
                int l = 0;
                for (int i = 0; i < temp.Length; i++)
                {

                    if (Uri.IsWellFormedUriString(temp[i], UriKind.Absolute) && temp[i].Contains("vk.com"))
                    {
                        if (l == 0)
                        {
                            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                            //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            col.HeaderText = temp[0] + "\n " + temp[i];
                            col.Width = 25;
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                            col.Width = 45;
                            dataGridView2.Columns.Add(col);
                        }
                        if (l == 1)
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.HeaderCell.Value = temp[0] + " " + temp[i];
                            dataGridView2.Rows.Add(row);
                        }
                        l++;
                    }
                }
            }
        }
        public struct RePost
        {
            public int index { get; set; }
            public System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.Attachments.Post> posts { get; set; }
            public List<long> likes { get; set; }

            public List<long> repost { get; set; }
        }
        public void Create_Massive_date(VkNet.VkApi vk, ref List<RePost> rep)
        {
            //List<RePost> rep = new List<RePost>();
            foreach (DataGridViewColumn col in dataGridView2.Columns)
            {
                string pattern = @"\w(/public+\d)";
                Regex reg = new Regex(pattern);
                if (reg.IsMatch(col.HeaderText))
                {
                    col.HeaderText = col.HeaderText.Replace("public", "club");
                }

                //  gr_id = vk.Utils.ResolveScreenName(col_header[1]);
                var groups_ids = vk.Utils.ResolveScreenName(col.HeaderText.Substring(col.HeaderText.LastIndexOf('/') + 1));
                try
                {
                    var posts = vk.Wall.Get(new WallGetParams() { OwnerId = (-1) * groups_ids.Id, Count = 100 });
                    List<Post> analize_posts = new List<Post>();
                    foreach (var p in posts.WallPosts)
                    {
                        if (dateTimePicker1.Value.Date == p.Date.Value.Date)
                            analize_posts.Add(p);
                    }


                    RePost re = new RePost();
                    re.index = col.Index;
                    re.posts = posts.WallPosts;
                    rep.Add(re);
                }
                catch(VkNet.Exception.VkApiException ex)
                {
                    MessageBox.Show(string.Format("Ошибка: {0}" + Environment.NewLine + "Для продолжения нажмити ОК", ex.Message));
                }
            }

        }
        public void Create_Massive_count(VkNet.VkApi vk,List<long> users, ref List<RePost> rep)
        {
            vk.RequestsPerSecond = 2;
            for (int i = 0; i < users.Count; i++)
            {
                //List<RePost> rep = new List<RePost>();
                foreach (DataGridViewColumn col in dataGridView2.Columns)
                {
                    string pattern = @"\w(/public+\d)";
                    Regex reg = new Regex(pattern);
                    if (reg.IsMatch(col.HeaderText))
                    {
                        col.HeaderText = col.HeaderText.Replace("public", "club");
                    }

                    //  gr_id = vk.Utils.ResolveScreenName(col_header[1]);
                    var groups_ids = vk.Utils.ResolveScreenName(col.HeaderText.Substring(col.HeaderText.LastIndexOf('/') + 1));
                    WallGetObject posts = null;
                    List<Post> analize_posts = new List<Post>();
                    if (radioButton11.Checked)
                    posts = vk.Wall.Get(new WallGetParams() { OwnerId = (-1) * groups_ids.Id, Count = Convert.ToUInt64(numericUpDown1.Value) });
                    else
                        if(radioButton10.Checked)
                    {
                        posts = vk.Wall.Get(new WallGetParams() { OwnerId = (-1) * groups_ids.Id, Count = 100 });
                    }


                    //else
                    //    posts = vk.Wall.Get(new WallGetParams() { OwnerId = (-1) * groups_ids.Id, Count = 100  });
                    // List<Post> analize_posts = new List<Post>();
                    List<long> likes = new List<long>();
                    List<long> reposts = new List<long>();
                    foreach (var p in posts.WallPosts)
                    {
                        if (radioButton11.Checked)
                        {
                            if (checkBox3.Checked)
                            {
                                var list = vk.Likes.GetList(new LikesGetListParams() { Extended = true, ItemId = p.Id.Value, OwnerId = p.OwnerId, Type = LikeObjectType.Post, Filter = LikesFilter.Likes });
                                likes.AddRange(list);
                            }
                            // Thread.Sleep(500);
                            if (checkBox4.Checked)
                            {
                                var list = vk.Likes.GetList(new LikesGetListParams() { Extended = true, ItemId = p.Id.Value, OwnerId = p.OwnerId, Type = LikeObjectType.Post, Filter = LikesFilter.Copies });
                                reposts.AddRange(list);
                            }
                        }
                        if (radioButton10.Checked)
                        {
                            if (dateTimePicker1.Value.Date == p.Date.Value.Date)
                            {
                                analize_posts.Add(p);
                                if (checkBox3.Checked)
                                {
                                    var list = vk.Likes.GetList(new LikesGetListParams() { Extended = true, ItemId = p.Id.Value, OwnerId = p.OwnerId, Type = LikeObjectType.Post, Filter = LikesFilter.Likes });
                                    likes.AddRange(list);
                                }
                                // Thread.Sleep(500);
                                if (checkBox4.Checked)
                                {
                                    var list = vk.Likes.GetList(new LikesGetListParams() { Extended = true, ItemId = p.Id.Value, OwnerId = p.OwnerId, Type = LikeObjectType.Post, Filter = LikesFilter.Copies });
                                    reposts.AddRange(list);
                                }
                            }
                                
                        }
                    }
                    var likes_c = (from data in likes where data == users[i] select data).Count();
                    var repost_c = (from data in reposts where data == users[i] select data).Count();
                    string result = string.Empty;
                    if (radioButton11.Checked)
                    {
                        if (checkBox3.Checked && checkBox4.Checked)
                            result = string.Format("{0}/{1}", likes_c, repost_c);
                        if (!checkBox3.Checked && checkBox4.Checked)
                            result = string.Format("{0}", repost_c);
                        if (checkBox3.Checked && !checkBox4.Checked)
                            result = string.Format("{0}", likes_c);
                    }
                    if(radioButton10.Checked)
                    {
                        if (checkBox3.Checked && checkBox4.Checked)
                        {
                            result = string.Format("{0}/{1}/{2}", likes_c, repost_c,analize_posts.Count);

                        }

                        if (!checkBox3.Checked && checkBox4.Checked)
                            result = string.Format("{0}/{1}", repost_c, analize_posts.Count);
                        if (checkBox3.Checked && !checkBox4.Checked)
                            result = string.Format("{0}/{1}", likes_c, analize_posts.Count);
                    }
                    if (!checkBox3.Checked && !checkBox4.Checked)
                    {
                        MessageBox.Show("Пустые выходные данные. Поставте хотябы одну галочку на лайки или репосты!!!");
                        break;
                    }
                    if (likes_c < numericUpDown2.Value)
                        dataGridView2.Rows[i].Cells[col.Index].Style.BackColor = Color.LightPink;
                    if (likes_c >= numericUpDown2.Value)
                        dataGridView2.Rows[i].Cells[col.Index].Style.BackColor = Color.LightGreen;
                    if (checkBox4.Checked)
                    {
                        if (repost_c < numericUpDown3.Value)
                            dataGridView2.Rows[i].Cells[col.Index].Style.BackColor = Color.LightSalmon;
                        if (repost_c >= numericUpDown3.Value)
                            dataGridView2.Rows[i].Cells[col.Index].Style.BackColor = Color.PaleGreen;
                    }
                    dataGridView2.Rows[i].Cells[col.Index].Value = result;
                    dataGridView2.Refresh();
                    //RePost re = new RePost();
                    //re.index = col.Index;
                    //re.posts = posts.WallPosts;
                    //re.likes = likes;
                    //re.repost = reposts;
                    //rep.Add(re);
                }
            }

        }
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                List<long> us_id = new List<long>();
                toolStripProgressBar1.Maximum = dataGridView2.Rows.Count;
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                toolStripStatusLabel1.Text = "Получение ID участников";
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    try
                    {
                        string[] header = row.HeaderCell.Value.ToString().Split(' ');

                        header[1] = header[1].Substring(header[1].LastIndexOf('/') + 1);
                        var id = vk.Utils.ResolveScreenName(header[1]).Id.Value;
                        us_id.Add(id);
                        toolStripProgressBar1.Value = row.Index;
                        StringBuilder strb = new StringBuilder();
                        strb.AppendFormat("Получение ID участников: {0}/{1}", row.Index, dataGridView2.Rows.Count);
                        toolStripStatusLabel1.Text = strb.ToString();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("!!!Проверте ссылки участников!!! где-то неправильная ссылка " + ex.Message);
                        break;
                    }
                }
                toolStripStatusLabel1.Text = "Определение вступивших";
                //us_id.Sort();
                List<RePost> rep = new List<RePost>();
                Create_Massive_count(vk,us_id, ref rep);
                #region хлам
                //    for (int i = 0; i < us_id.Count; i++)
                //    {

                //        // List<String> gr_str = new List<string>();
                //        foreach (var analize_post in rep)
                //        {

                //            //bool copied = false;
                //            var repost = (from data in analize_post.repost where data == us_id[i] select data).Count();
                //            var likes_c = (from data in analize_post.likes where data == us_id[i] select data).Count();




                //            //foreach (var post in analize_post.posts)
                //            //{

                //            //    var likes = vk.Likes.IsLiked(out copied, LikeObjectType.Post, post.Id.Value, us_id[i], post.OwnerId);
                //            //    if (copied)
                //            //        repost++;
                //            //    if (likes)
                //            //        likes_c++;
                //            //}
                //            string result = string.Empty;
                //            if (checkBox3.Checked && checkBox4.Checked)
                //                result = string.Format("{0}/{1}", likes_c, repost);
                //            if (!checkBox3.Checked && checkBox4.Checked)
                //                result = string.Format("{0}", repost);
                //            if (checkBox3.Checked && !checkBox4.Checked)
                //                result = string.Format("{0}", likes_c);
                //            if (!checkBox3.Checked && !checkBox4.Checked)
                //            {
                //                MessageBox.Show("Пустые выходные данные. Поставте хотябы одну галочку на лайки или репосты!!!");
                //                break;
                //            }
                //            if (likes_c < numericUpDown2.Value)
                //                dataGridView2.Rows[i].Cells[analize_post.index].Style.BackColor = Color.LightPink;
                //            if (likes_c >= numericUpDown2.Value)
                //                dataGridView2.Rows[i].Cells[analize_post.index].Style.BackColor = Color.LightGreen;
                //            if (checkBox4.Checked)
                //            {
                //                if (repost < numericUpDown3.Value)
                //                    dataGridView2.Rows[i].Cells[analize_post.index].Style.BackColor = Color.LightSalmon;
                //                if (repost >= numericUpDown3.Value)
                //                    dataGridView2.Rows[i].Cells[analize_post.index].Style.BackColor = Color.PaleGreen;
                //            }
                //            dataGridView2.Rows[i].Cells[analize_post.index].Value = result;
                //            dataGridView2.Refresh();
                //        }
                //    }

                //if (radioButton10.Checked)
                //{
                //    Create_Massive_date(vk, ref rep);
                //    for (int i = 0; i < us_id.Count; i++)
                //    {

                //        foreach (var analize_post in rep)
                //        {

                //            bool copied = false;
                //            bool likes = false;
                //            int repost = 0;
                //            int likes_c = 0;
                //            vk.RequestsPerSecond = 2;

                //            //(new WallSearchParams() { OwnerId = (-1) * groups_ids.Id, Query = dateTimePicker1.Text });
                //            foreach (var post in analize_post.posts)
                //            {

                //                try
                //                {
                //                    likes = vk.Likes.IsLiked(out copied, LikeObjectType.Post, post.Id.Value, us_id[i], post.OwnerId);
                //                }
                //                catch (VkNet.Exception.VkApiException ex)
                //                {
                //                    MessageBox.Show(ex.Message);
                //                }
                //                if (copied)
                //                    repost++;
                //                if (likes)
                //                    likes_c++;
                //            }
                //            string result = string.Empty;
                //            if (checkBox3.Checked && checkBox4.Checked)
                //            {
                //                result = string.Format("{0}/{1}/{2}", likes_c, repost, analize_post.posts.Count);

                //            }

                //            if (!checkBox3.Checked && checkBox4.Checked)
                //                result = string.Format("{0}/{1}", repost, analize_post.posts.Count);
                //            if (checkBox3.Checked && !checkBox4.Checked)
                //                result = string.Format("{0}/{1}", likes_c, analize_post.posts.Count);
                //            if (!checkBox3.Checked && !checkBox4.Checked)
                //            {
                //                MessageBox.Show("Пустые выходные данные. Поставте хотябы одну галочку на лайки или репосты!!!");
                //                break;
                //            }
                //            if (likes_c < numericUpDown2.Value)
                //                dataGridView2.Rows[i].Cells[analize_post.index].Style.BackColor = Color.LightPink;
                //            if (likes_c >= numericUpDown2.Value)
                //                dataGridView2.Rows[i].Cells[analize_post.index].Style.BackColor = Color.LightGreen;
                //            if (checkBox4.Checked)
                //            {
                //                if (repost < numericUpDown3.Value)
                //                    dataGridView2.Rows[i].Cells[analize_post.index].Style.BackColor = Color.LightSalmon;
                //                if (repost >= numericUpDown3.Value)
                //                    dataGridView2.Rows[i].Cells[analize_post.index].Style.BackColor = Color.PaleGreen;
                //            }
                //            dataGridView2.Rows[i].Cells[analize_post.index].Value = result;
                //            dataGridView2.Refresh();
                //        }
                //    }
                //}
#endregion
                toolStripStatusLabel1.Text = "Определение завершено";
            }
            catch (VkNet.Exception.AccessTokenInvalidException ex)
            {
                MessageBox.Show("Вы забыли авторизоваться. Ввойдите в ВК! " + ex.Message);
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                MessageBox.Show("Проверте входные данные1: " + ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            users_id.Clear();
            groups_id.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
        }

        private void richTextBox2_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start( e.LinkText);
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked)
                label14.Text = "Формат вывода: лайки / репосты / всего постов за дату";

        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked)
                label14.Text = "Формат вывода: лайки / репосты ";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                textBox3.Visible = true;
                textBox4.Visible = true;
            }
            else
            {
                textBox3.Visible = false;
                textBox4.Visible = false; 
            }
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if(textBox3.Text == "IP прокси сервера")
            {
                textBox3.Text = string.Empty;
                textBox3.ForeColor = Color.Black;
            }
        }

        private void textBox4_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox4.Text == "Порт")
            {
                textBox4.Text = string.Empty;
                textBox4.ForeColor = Color.Black;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start( "http://kitona.ru");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://money.yandex.ru/to/410013587572327"); 
        }
    }
}
