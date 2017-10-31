using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjectDiplom
{
    public partial class ConfirmationSubWindow : Form
    {
        Settings main;
        public void ConfirmationSubWindow(int xpos,int ypos,Settings m)
        {
            main = m;
            InitializeComponent();   
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            main.Enabled = true;
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            main.Enabled = true;
            this.Close(); 
        }

        private bool button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == Properties.Settings.Default.Password)
            {
                main.Enabled=true;
                //m
                this.Close();
                return true;
            }
            else { MessageBox.Show("неверный пароль!"); return false; }
        }

       
    }
}
