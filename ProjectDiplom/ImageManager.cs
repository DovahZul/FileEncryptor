using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;
using System.Runtime.InteropServices;
namespace ProjectDiplom
{
    public partial class ImageManager : Form
    {
        private int sortColumn = -1;
        private string workdir = Application.CommonAppDataPath + @"\workdir\Images\";//@"D:/workdir/Images/";
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
        private Base main;
        List<string[]> files=new List<string[]>();
        public ImageManager(int xpos,int ypos,Base m)
        {
            main = m;
            this.Left = xpos;
            this.Top = ypos;
            InitializeComponent();
           // NoFilesTextBox.Visible = true;
            this.listView1.ColumnClick +=
new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
          //  listView1.ContextMenu = contextMenuStrip1;
            DirectoryInfo dir = new DirectoryInfo(workdir);
            if (!Directory.Exists(workdir))
                try
                {
                    Directory.CreateDirectory(workdir);
                    toolStripStatusLabel1.Text = "Создание рабочей папки...";
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
                finally { toolStripStatusLabel1.Text = "Рабочая папка успешно создана"; }
            dir.Attributes = FileAttributes.Hidden;
            refreshFileList();
        }
        private void refreshFileList()
        {

            listView1.Items.Clear();
            files.Clear();
            DirectoryInfo dir = new DirectoryInfo(workdir);
            foreach (FileInfo file in dir.GetFiles()) // извлекаем все файлы и кидаем их в список 
            {
                if(Path.GetExtension(file.FullName)==".tth")
                    files.Add(new string[3] { file.Name.Substring(0, file.Name.Length - 4), file.Length.ToString() + " Байт", file.FullName });
                // получаем полный путь к файлу и кидаем его в список 
                //  MessageBox.Show(files.ElementAt(0).ElementAt(0));

            }
            // listView1.Clear();
            if (files.Count > 0)
            {
                //NoFilesTextBox.Visible = false;
                foreach (string[] s in files)
                {
                    ListViewItem i = new ListViewItem(s);
                    listView1.Items.Add(i);
                }
            }
            else {/* NoFilesTextBox.Visible = true; */}
            //if (files.Count == 0) NoFilesTextBox.Visible = true;
            //else
            //    NoFilesTextBox.Visible = false;
        
        }
 
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Filter = "image files(*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            fileChooser.InitialDirectory = "D:/";
            if (fileChooser.ShowDialog() == DialogResult.OK)
            {
               PreviewPicture.ImageLocation = fileChooser.FileName;
               HoverPanel.Visible = true;
                //  .ImageLocation = fileChooser.FileName;
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            HoverPanel.Visible = false;
            PreviewPicture.ImageLocation = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            main.Enabled = true; main.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           // byte[] imgData;
            byte[] hash = ElementalEncryptor.getHashSha256(Properties.Settings.Default.Password);
            var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};
            byte[] enc = ElementalEncryptor.encryptStream(File.ReadAllBytes(PreviewPicture.ImageLocation/*f.FileName*/), hash, sevenItems);
            File.WriteAllBytes(workdir + System.IO.Path.GetFileName(PreviewPicture.ImageLocation+".tth"/*f.FileName*/), enc);
            //скрытый
            System.IO.File.SetAttributes(workdir + System.IO.Path.GetFileName(PreviewPicture.ImageLocation+".tth"), System.IO.FileAttributes.Hidden);
            //только для чтения
          //  System.IO.File.SetAttributes(workdir + System.IO.Path.GetFileName(PreviewPicture.ImageLocation+".tth"), System.IO.FileAttributes.ReadOnly);
            //системный
            //System.IO.File.SetAttributes(workdir + System.IO.Path.GetFileName(PreviewPicture.ImageLocation+".tth"), System.IO.FileAttributes.System);
            // ElementalEncryptor.EncryptFile(f.FileName, workdir + System.IO.Path.GetFileName(f.FileName));
            // string[] row = { f.SafeFileName, workdir+System.IO.Path.GetFileName(f.FileName) };
            // ListViewItem i = new ListViewItem(row);
            // listView1.Items.Add(i);
            //files.Add(row);
           // MessageBox.Show(workdir + System.IO.Path.GetFileName(PreviewPicture.ImageLocation/*f.FileName*/));
            
            refreshFileList();
            HoverPanel.Visible = false;
            //SqlCeConnection cnn = new SqlCeConnection(Properties.Settings.Default.ImageDataBaseConnectionString);
        
            //try
            //{
            //    imgData = File.ReadAllBytes(PreviewPicture.ImageLocation);
            //    SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Imagetbl (Pics,Filename) Values (@pic,@name)", cnn);
            //    cmd.Parameters.Add("@pic", imgData);
            //    cmd.Parameters.Add("@name", Path.GetFileName(PreviewPicture.ImageLocation));
            //   // cmd.Parameters.Add("@date", DateTime.Today);
            //    cnn.Open();
            //    cmd.ExecuteNonQuery();
            //    cnn.Close();
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { MessageBox.Show("Изображние добавлено"); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Filter = "image files(*.jpg,*.jpeg,*,*.png,*.bmp)|*.jpg;*jpeg;*.png;*.bmp";
            fileChooser.InitialDirectory = "D:/";
            if (fileChooser.ShowDialog() == DialogResult.OK)
            {
                PreviewPicture.ImageLocation = fileChooser.FileName;
                HoverPanel.Visible = true;
                //  .ImageLocation = fileChooser.FileName;
            }
        }
        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;

        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // if(listView1.SelectedItems[0].Index>0)
            try
            {
                int index = listView1.SelectedItems[0].Index;
                byte[] hash = ElementalEncryptor.getHashSha256(Properties.Settings.Default.Password);
                var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};
                byte[] dec = ElementalEncryptor.decryptStream(File.ReadAllBytes(files.ElementAt(index).ElementAt(2)), hash, sevenItems);
                // File.WriteAllBytes(fd.FileName, dec);
                MiniaturePicture.Image = ByteToImage(dec);
                label3.Text=Path.GetExtension(files.ElementAt(index).ElementAt(0));
                //using (System.Drawing.Image objImage = System.Drawing.Image.FromFile(files.ElementAt(index).ElementAt(2)))
                //{
                //    label4.Text = objImage.Width.ToString();
                //    //lbl_ImageHeight.Text = objImage.Height.ToString();
                //}
                //System.Drawing.Image objImage = System.Drawing.Image.FromFile(files.ElementAt(1).ElementAt(2));
               // label4.Text = objImage.Width.ToString() + ":" + objImage.Height.ToString();
                
                // width = objImage.Width;
               // height = objImage.Height; 
                 label4.Text=File.GetCreationTime(files.ElementAt(index).ElementAt(2)).ToString();
            }
            catch (Exception ex) {/* MessageBox.Show(ex.Message);*/MiniaturePicture.Image = null; }
            }

        //private void извлечьToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    int index = listView1.SelectedItems[0].Index;
        //    string extention = Path.GetExtension(listView1.Items[index].SubItems[3].Text);
        //    MessageBox.Show(extention + " " + listView1.Items[index].SubItems[3].Text);
        //    if (listView1.SelectedItems.Count > 0)
        //    {
        //        SaveFileDialog fd = new SaveFileDialog();

        //        fd.Filter = "Normalized file(" + extention + ")|*" + extention;//"txt files (*.txt)|*.txt|Microsoft Word files (.doc, .docx)|*.doc;*.docx|Microsoft Exel (.xls)|*.xls";
        //        fd.FilterIndex = 1;
        //        fd.Title = "Извлечение исходного файла";
        //        if (fd.ShowDialog() == DialogResult.OK)
        //        {
        //            // ElementalEncryptor.DecryptFile(listView1.Items[index].SubItems[3].Text, fd.FileName);
        //            byte[] hash = ElementalEncryptor.getHashSha256("qwerty");
        //            var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
        //        ,0x20,0x20,0x20,0x20,0x20,0x20};
        //            byte[] dec = ElementalEncryptor.decryptStream(File.ReadAllBytes(listView1.Items[index].SubItems[3].Text), hash, sevenItems);
        //            File.WriteAllBytes(fd.FileName, dec);
        //        }

        //    }
        //}

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
             try
            {
            int index = listView1.SelectedItems[0].Index;
          //  string extention = Path.GetExtension(listView1.Items[index].SubItems[3].Text);
           
                File.Delete(files.ElementAt(index).ElementAt(2));
                refreshFileList();
                MessageBox.Show("Файл успешно удалён");
                statusStrip1.Text = "Файл удален";
            }
            catch (Exception ex) {/* MessageBox.Show("Удалить нe удалось, "+ex.Message);*/ }
        }
        

        

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listView1.SelectedItems[0].Index;
                byte[] hash = ElementalEncryptor.getHashSha256(Properties.Settings.Default.Password);
                var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};
                byte[] dec = ElementalEncryptor.decryptStream(File.ReadAllBytes(files.ElementAt(index).ElementAt(2)), hash, sevenItems);
                // File.WriteAllBytes(fd.FileName, dec);
                // MiniaturePicture.Image = ByteToImage(dec);
                // label3.Text=Path.GetExtension(files.ElementAt(index).ElementAt(2));
                FullImage f = new FullImage(dec,this,this.Left,this.Top);
                f.Show();
                this.Enabled = false;
            }
            catch (Exception ex) {/* MessageBox.Show(ex.Message);*/ }
        }

        private void button9_Click(object sender, EventArgs e)
        {
             try
            {
            int index = listView1.SelectedItems[0].Index;
          //  string extention = Path.GetExtension(listView1.Items[index].SubItems[3].Text);
           
                File.Delete(files.ElementAt(index).ElementAt(2));
                refreshFileList();
                MessageBox.Show("Файл успешно удалён");
                statusStrip1.Text = "Файл удален";
            }
            catch (Exception ex) {/* MessageBox.Show("Удалить н удалось, "+ex.Message); */}
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;
            byte[] hash = ElementalEncryptor.getHashSha256(Properties.Settings.Default.Password);
            var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};
            byte[] dec = ElementalEncryptor.decryptStream(File.ReadAllBytes(files.ElementAt(index).ElementAt(2)), hash, sevenItems);
            // File.WriteAllBytes(fd.FileName, dec);
            // MiniaturePicture.Image = ByteToImage(dec);
            // label3.Text=Path.GetExtension(files.ElementAt(index).ElementAt(2));
            try
            {
                FullImage f = new FullImage(dec, this, this.Left, this.Top);
       
            this.Enabled = false;
            f.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            main.Enabled = true; main.Show();
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            main.Enabled = true;
            main.Top = this.Top;
            main.Left = this.Left;
            main.Show();
            this.Close();
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Underline);
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            label6.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Underline);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            label6.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
        }
        private void listView1_ColumnClick(object sender,
 System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Определение того, совпадает ли столбец с последним выбранным столбцом.

            if (e.Column != sortColumn)
            {
                // Установка сортировки нового столбца.
                sortColumn = e.Column;
                // Установка порядка сортировки "по возрастанию" как порядка по
                // умолчанию.
                listView1.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Определение и последующее изменение последнего порядка сортировки.
                if (listView1.Sorting == SortOrder.Ascending)
                    listView1.Sorting = SortOrder.Descending;
                else
                    listView1.Sorting = SortOrder.Ascending;
            }

            // Вызов метода ручной сортировки.
            listView1.Sort();
            // Установка свойства ListViewItemSorter на новый объект
            // ListViewItemComparer.
            this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column,
                                                                  listView1.Sorting);
        }

      
      
        

       
    }
}
