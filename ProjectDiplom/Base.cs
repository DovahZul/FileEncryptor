using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
namespace ProjectDiplom
{
    public partial class Base : Form
    {
       private string workdir = Application.CommonAppDataPath+@"\workdir\";//@"D:/workdir/";
       private string workdirdoc = Application.CommonAppDataPath +@"\workdir\default\";//@"D:/workdir/default/";
       private string workdirimg = Application.CommonAppDataPath+@"\workdir\Images\";//@"D:/workdir/Images/";
        public Base(int xpos=320,int ypos=240)
        {
           // File.Create("D:/testattr.txt");
            //MessageBox.Show(Application.CommonAppDataPath + "/workdir/default/");
            //File.SetAttributes("D:/testattr.txt", System.IO.FileAttributes.ReadOnly);
            File.SetAttributes("D:/testattr.txt", System.IO.FileAttributes.System);
           // File.SetAttributes("D:/testattr.txt", System.IO.FileAttributes.Hidden);
            this.Left = xpos;
            this.Top = ypos;
            InitializeComponent();
            ////////////////////////////////
            DirectoryInfo dir = new DirectoryInfo(workdir);
            if (!Directory.Exists(workdir))
                try
                {
                    Directory.CreateDirectory(workdir);
                    toolStripStatusLabel1.Text = "Создание главной папки...";
                    
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
                finally { toolStripStatusLabel1.Text = "Главная папка успешно создана"; }
            ////////////////////////////////
            DirectoryInfo dirdoc = new DirectoryInfo(workdirdoc);
            if (!Directory.Exists(workdirdoc))
                try
                {
                    Directory.CreateDirectory(workdirdoc);
                    toolStripStatusLabel1.Text = "Создание главной папки...";

                }
                catch (Exception e) { MessageBox.Show(e.Message); }
                finally { toolStripStatusLabel1.Text = "Главная папка успешно создана"; }
            /////////////////////////////////////////////////////
            DirectoryInfo dirimg = new DirectoryInfo(workdirimg);
            if (!Directory.Exists(workdirimg))
                try
                {
                    Directory.CreateDirectory(workdirimg);
                    toolStripStatusLabel1.Text = "Создание главной папки...";

                }
                catch (Exception e) { MessageBox.Show(e.Message); }
                finally { toolStripStatusLabel1.Text = "Главная папка успешно создана"; }
            /////////////////////////////////////////////////////////
            dir.Attributes = FileAttributes.Hidden;
            dirimg.Attributes = FileAttributes.Hidden;
            dirdoc.Attributes = FileAttributes.Hidden;
        int numfile=(Directory.GetFiles(workdirdoc,"*.tth").Count()+Directory.GetFiles(workdirimg,"*.tth").Count());
       // MessageBox.Show(numfile.ToString());
            label8.Text=numfile.ToString();
            
        }
        //public bool flag = false;
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
        [Flags]
        enum AnimateWindowFlags
        {
            AW_HOR_POSITIVE = 0x00000001,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_ACTIVATE = 0x00020000,
            AW_SLIDE = 0x00040000,
            AW_BLEND = 0x00080000
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool AnimateWindow(IntPtr hWnd, int time, AnimateWindowFlags flags);

        
        
        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1(Top,Left,this);
            f.Top = this.Top;
            f.Left = this.Left;
            f.Show();
            this.Enabled = false; this.Hide();
        }
        //public void ActivateDoc()
        //{
        //    Form1 f = new Form1(this.Left,this.Top,this);
        //    f.Show();
        //    this.Enabled = false; this.Hide();
        //}
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ImageManager f = new ImageManager(this.Left, this.Top, this);
            f.Show();
            this.Enabled = false; this.Hide();
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
           
        }

        private void pictureBox2_MouseEnter(object sender, DragEventArgs e)
        {
        
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            label2.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Regular);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Settings f = new Settings(this.Left, this.Top, this);
            //f.Top = this.Top;
            //f.Left = this.Left;
            f.Show();
            this.Enabled = false;
           // this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("заглушка");
            
            try
            {
                panel5.Visible = true;
                richTextBox1.Focus();
                richTextBox1.LoadFile("readme.txt", RichTextBoxStreamType.PlainText);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Base_Load(object sender, EventArgs e)
        {
           // this.label5.Text = DateTime.Now.ToLocalTime().ToString();
   // this.monthCalendar1.SelectionRange.Start.ToShortDateString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AutorizationForm a = new AutorizationForm();
            a.Activate();
            a.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;

        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void Base_Load_1(object sender, EventArgs e)
        {
    //        AnimateWindow(this.Handle, 1000,
    //AnimateWindowFlags.AW_BLEND |
    //AnimateWindowFlags.AW_VER_POSITIVE);
        
        }

        private void Base_FormClosing(object sender, FormClosingEventArgs e)
        {
            //AnimateWindow(this.Handle, 1000, AnimateWindowFlags.AW_BLEND | AnimateWindowFlags.AW_HIDE);
        }

        private void Base_Activated(object sender, EventArgs e)
        {
            string tale="0 Байт";
            DirectoryInfo dir = new DirectoryInfo(workdirdoc);
           double maxsize=0;
              try
            {
                  
                DirectoryInfo dir1 = new DirectoryInfo(workdirdoc);
                foreach (FileInfo file in dir1.GetFiles())
                {
                    if (Path.GetExtension(file.FullName) == ".tth")
                        maxsize+=file.Length;
                }
                  DirectoryInfo dir2 = new DirectoryInfo(workdirimg);
                    foreach (FileInfo file in dir2.GetFiles())
                {
                    if (Path.GetExtension(file.FullName) == ".tth")
                         maxsize+=file.Length;
                }
            }
            catch (Exception ex) { /*MessageBox.Show(ex.Message);*/ }
            if (Directory.Exists(workdirdoc) && Directory.Exists(workdirimg))
            {
                int numfile = (Directory.GetFiles(workdirdoc, "*.tth").Count() + Directory.GetFiles(workdirimg, "*.tth").Count());
                // MessageBox.Show(numfile.ToString());
                label8.Text = numfile.ToString();
                switch (maxsize.ToString().Length)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        tale = String.Format("{0:0.00}", maxsize) + " Байт"; break;// "Б"; label9.Text = maxsize.ToString() + " " + tale; break;
                    case 4:
                    case 5:
                    case 6:tale = String.Format("{0:0.00}", maxsize/1000) + " КБ"; break;// "КБ"; label9.Text = (maxsize).ToString() + " " + tale; break;
                    case 7:
                    case 8:
                    case 9:
                     tale = String.Format("{0:0.00}", maxsize/1024000) + " МБ"; break;//"МБ"; label9.Text = (maxsize).ToString() + " " + tale; break;
                    default: tale = String.Format("{0:0.00}", maxsize/1048576000) + " ГБ"; break;//"Гбйййй"; label9.Text = (maxsize / 1048576000).ToString() + " " + tale; break;
                 
                }
                label9.Text = tale; //(maxsize / 1024000).ToString() + " " + (maxsize).ToString() + " " + tale;
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            label1.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Underline);
        }

        private void pictureBox1_MouseLeave_1(object sender, EventArgs e)
        {
            label1.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Regular);
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            label2.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Underline);
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            label3.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Underline);
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            label3.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Regular);
        }

        private void label10_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            richTextBox1.Text = "";
        }

        private void label10_MouseEnter(object sender, EventArgs e)
        {
            label10.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Underline);
        }

        private void label10_MouseLeave(object sender, EventArgs e)
        {
            label10.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
        }

      


    

       
    }
}
