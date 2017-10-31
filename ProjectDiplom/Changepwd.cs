using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ProjectDiplom
{
    public partial class Changepwd : Form
    {
        public BackgroundWorker bw = new BackgroundWorker();
        private string workdir1 = Application.CommonAppDataPath + @"\workdir\default\";// @"D:/workdir/default/";
        private string workdir2 = Application.CommonAppDataPath + @"\workdir\Images\";//@"D:/workdir/images/";
        private Regex regex = new Regex("[a-zA-Z]{5,15}");
        private Regex pwd = new Regex("[a-zA-Z0-9]{6,15}");
        private List<string> files=new List<string>();
        Settings main;
        /// ////////////drag-drop

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_VAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_VAPTION, 0);

        }
        /// ////////////drag-drop
        public Changepwd(Settings m)
        {
            main = m;
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            this.AcceptButton = button1;
           
        }
        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //make sure ProgressBar is not continuous, and has a max value
            progressBar1.Value = e.ProgressPercentage;
           // panel1.Visible = true;
           // label5.Focus();
           // label4.Text = "Обработка...";
        }
        private void reencrypt(string patch, string passwdold, string passwdnew)
        {
           // panel3.Visible = true;
            int i = 0;
            byte[] hash;
                    byte[] newhash;
                    try
                    {
                       // panel3.Visible = true;
                        DirectoryInfo dir = new DirectoryInfo(patch);
                        foreach (FileInfo file in dir.GetFiles()) // извлекаем все файлы и кидаем их в список 
                        {
                           // panel3.Visible = true;
                            if (Path.GetExtension(file.FullName) == ".tth")
                            {
                               // label4.Text = "Обработка...";
                                hash = ElementalEncryptor.getHashSha256(passwdold);
                                newhash = ElementalEncryptor.getHashSha256(passwdnew);
                                var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};
                                if(i<100)
                                bw.ReportProgress(i % dir.GetFiles().Count());
                                i++;
                                // byte[] dec = ElementalEncryptor.decryptStream(File.ReadAllBytes(file.FullName), hash, sevenItems);
                                // byte[] enc = ElementalEncryptor.encryptStream(dec, hash, sevenItems);
                                // MessageBox.Show(file.FullName);

                                //  File.WriteAllBytes(file.FullName, dec);
                                System.IO.File.SetAttributes(file.FullName, System.IO.FileAttributes.Normal);
                                File.WriteAllBytes(file.FullName, ElementalEncryptor.encryptStream(ElementalEncryptor.decryptStream(File.ReadAllBytes(file.FullName), hash, sevenItems), newhash, sevenItems));
                                //  i++;

                            }
                        }
                    }
                    catch (Exception ex) { /*MessageBox.Show(ex.Message);*/ }
          //
        }
 
           
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            label5.Visible = true;
            panel1.Visible = true;
          //  bw.RunWorkerAsync(label5.Text="123");
           // label5.Text="123123";
            label5.Focus();
            try
            {
                if (textBox1.Text.Length != 0 && textBox2.Text.Length != 0 && textBox3.Text.Length != 0)
                {
                    if (textBox1.Text == Properties.Settings.Default.Password)
                    {
                        if (textBox2.Text == textBox3.Text)
                        {
                            //MessageBox.Show("Смена пароля,пожалуйста подождите...","Обработка нового ключа");
                          //  panel1.Visible=true;
                           // reencrypt(workdir1, textBox1.Text, textBox2.Text);
                            reencrypt(workdir2, Properties.Settings.Default.Password, textBox2.Text);
                            reencrypt(workdir1, Properties.Settings.Default.Password, textBox2.Text);
                            Properties.Settings.Default.Password = textBox2.Text;
                            Properties.Settings.Default.Save(); MessageBox.Show("Изменения сохранены"); 
                            panel3.Visible=false;
                            // ConfigurationManager.AppSettings.Add("password", textBox2.Text);
                            main.Enabled = true; main.Show();
                            this.Close();
                        }
                        else MessageBox.Show("Пароли не совпадают");
                    }
                    else MessageBox.Show("неверный пароль");
                }
                else MessageBox.Show("Поля заполнены неверно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); main.Enabled = true; main.Show();
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            main.Enabled = true; main.Show();
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.PasswordChar = '\0';
                textBox2.PasswordChar = '\0';
                textBox3.PasswordChar = '\0';

            }
            else
            {
                textBox1.PasswordChar = '*';
                textBox2.PasswordChar = '*';
                textBox3.PasswordChar = '*';
            }
            }

        private void button3_Click(object sender, EventArgs e)
        {
            main.Enabled = true; main.Show();
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
            if (textBox2.Text.Length != 0)
            {
                pictureBox1.Visible = true;
            }else pictureBox1.Visible = false;
           
            if (pwd.IsMatch(textBox2.Text))
            {
                pictureBox1.ImageLocation = "accept.png";
               
            }
            else
            {
                pictureBox1.ImageLocation = "cancel.ico";

            } if (textBox2.Text == textBox3.Text)
                pictureBox2.ImageLocation = "accept.png";
            else
                pictureBox2.ImageLocation = "cancel.ico";
          
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length != 0)
            {
                pictureBox2.Visible = true;
            }
            else pictureBox2.Visible = false;
            if (pwd.IsMatch(textBox2.Text))
          pictureBox1.ImageLocation = "accept.png";
            else
                pictureBox1.ImageLocation = "cancel.ico";
            if (textBox2.Text == textBox3.Text)
                pictureBox2.ImageLocation = "accept.png";
            else
                pictureBox2.ImageLocation = "cancel.ico";

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
            

         
    }
}
