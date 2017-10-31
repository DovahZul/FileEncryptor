using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ProjectDiplom
{
    public partial class Settings : Form
    {
        public bool flag = false;
        
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
        Base main;
        public Settings(int xpos,int ypos,Base m)
        {
        main=m;
       
            InitializeComponent();
            this.Left = m.Left;
            this.Top = m.Top;
            LoadLocalSettings();
        }
        private void LoadLocalSettings()
    {
        comboBox1.Text=Properties.Settings.Default.tryes;
        checkBox1.Checked = Properties.Settings.Default.candelete;
    
    }
        private void button4_Click(object sender, EventArgs e)
        {
            //ConfirmationSubWindow c = new ConfirmationSubWindow(this.Left, this.Top, this);
           // c.Show();
           // this.Enabled = false;
           // if (flag) MessageBox.Show("saved"); else MessageBox.Show("unsaved");
            Properties.Settings.Default.candelete = checkBox1.Checked;
            //MessageBox.Show( comboBox1.SelectedItem.ToString());
             Properties.Settings.Default.tryes = comboBox1.SelectedItem.ToString();
             try
             {
                 Properties.Settings.Default.Save(); MessageBox.Show("Изменения сохранены"); main.Enabled = true; main.Show();
                 this.Close();
             }
             catch (Exception ex) { MessageBox.Show(ex.Message); }
             
        }

        private void button6_Click(object sender, EventArgs e)
        {
            main.Enabled = true; main.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            main.Enabled = true;
            main.Show();
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBox1.Visible = true;
                label2.Visible = true;
                label4.Visible = true;
            }
            else
            {
                comboBox1.Visible = false;
                label2.Visible = false;
                label4.Visible = false;
            }
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            //label1.Font = new Font(label1.Font, label1.Font.Style & ~FontStyle.Bold);
            label1.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Underline);
        }

        

        private void label1_MouseLeave(object sender, EventArgs e)
        {
           // label1.Font = new Font(label1.Font, label1.Font.Style & ~FontStyle.Regular);
            label1.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
        }

        private void checkBox1_DragEnter(object sender, DragEventArgs e)
        {
            
            
        }

        private void checkBox1_MouseEnter(object sender, EventArgs e)
        {
          checkBox1.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Underline);
        }

        private void checkBox1_MouseLeave(object sender, EventArgs e)
        {
            checkBox1.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Changepwd c = new Changepwd(this);
            c.Show();
            this.Enabled = false;

        }

     

    }
}
