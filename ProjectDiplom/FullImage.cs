using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace ProjectDiplom
{
    public partial class FullImage : Form
    {
        public ImageManager main;
       //dragdrop
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
        //dragdrop???
        public static Bitmap ByteToImage(byte[] blob)
        {
           
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;

        }
        public FullImage(byte[] bytes,ImageManager m,int x,int y)
        {
            this.Top = y;
            this.Left = x;
            InitializeComponent();
            main = m;
            
           // this.OnClosing+=new FormClosingEventHandler(this.closeEvnt);
            try
            {
                pictureBox1.Image = ByteToImage(bytes);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            main.Enabled = true;
            this.Close();
        }
        //public void closeEvnt(object s,EventArgs e)
      //  {
        //MessageBox.Show("OK");
        }
    }

