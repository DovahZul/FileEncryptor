using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.IO;

namespace ProjectDiplom
{
    public partial class AutorizationForm : Form
    {
        private string workdir = Application.CommonAppDataPath + @"\workdir\default\";// @"D:/workdir/";
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
        private int pop =Convert.ToInt16(Properties.Settings.Default.tryes);
        public AutorizationForm()
        {
            Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            InitializeComponent();
            this.Top = 1000;//resolution.Height/2;
            this.Left = 5000;// resolution.Width / 2;
            this.AcceptButton = button4;
            textBox1.Select();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.PasswordChar ='\0';

            }else textBox1.PasswordChar='*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Configuration config = ConfigurationManager.OpenExeConfiguration("D:/ProjectDiplom.exe.config"); //WebConfigurationManager.OpenWebConfiguration("/");
            //    ConfigurationSection appSettings = config.GetSection("userSetting");

            //    if (appSettings.SectionInformation.IsProtected)
            //        appSettings.SectionInformation.UnprotectSection();
            //    else
            //        appSettings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");

            //    config.Save();
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            ////ConnectionStrings["base"].ConnectionString.ToString());

            if (Properties.Settings.Default.Password == textBox1.Text)
            {
                MessageBox.Show("Добро пожаловать!");
                Base b = new Base();
                b.Activate();
              //Application.Run(b);
                b.Show();
               //Close();
                this.Close();
               
            }
            else

            { 
                MessageBox.Show("Неверный пароль!");
                textBox1.Text = null;
                if (pop <= 0)
                {
                    MessageBox.Show("Попытки кончились");
                    try
                    {
                        if (Directory.Exists(workdir))
                            Directory.Delete(workdir, true);
                        else {/* MessageBox.Show("папки нет");*/ this.Close(); }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    finally { MessageBox.Show("хранилище очищено"); }
                    this.Close();
                    Application.Exit();
                }
                if(pop<=3)MessageBox.Show("Внимание! Осталось попыток: "+pop.ToString());
                 if(Properties.Settings.Default.candelete)
                pop--;
                
            }
        }

        private void AutorizationForm_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           // if ( == Keys.Enter) button4.PerformClick();
        }
    }
}
