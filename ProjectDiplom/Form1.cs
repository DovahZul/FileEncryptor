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
using System.Threading;
namespace ProjectDiplom
{
    public partial class Form1 : Form
    {
       public BackgroundWorker bw = new BackgroundWorker();
      
        private int sortColumn = -1;
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
        public List<string[]> files = new List<string[]>();
        public string workdir = Application.CommonAppDataPath + @"\workdir\default\";// @"D:/workdir/default/";
        public Base main;
        public Form1(int xpos,int ypos,Base m=null)
        {
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
           // bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            main = m;
            this.StartPosition = m.StartPosition;
            this.Left = 0;//xpos;
            this.Top = 0;// ypos;
            InitializeComponent();
            this.listView1.ColumnClick +=
            new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            textBox1.Text = workdir;
            DirectoryInfo dir = new DirectoryInfo(workdir);
              if(!Directory.Exists(workdir))
                  try
                  {
                      Directory.CreateDirectory(workdir);
                      toolStripStatusLabel1.Text = "Создание рабочей папки...";
                  }
                  catch (Exception e) { MessageBox.Show(e.Message); }
                  finally { toolStripStatusLabel1.Text = "Рабочая папка успешно создана"; }
              dir.Attributes = FileAttributes.Hidden;
           // MessageBox.Show(files.Count.ToString());
            refreshFileList(textBox1.Text);
           // panel3.DragOver = new DragEventHandler() { this.dra};
            
        
        }
        private void refreshFileList(string path)
        {
            listView1.Items.Clear();
            files.Clear();
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (FileInfo file in dir.GetFiles()) // извлекаем все файлы и кидаем их в список 
                {
                    if (Path.GetExtension(file.FullName) == ".tth")
                        files.Add(new string[4] { file.Name.Substring(0,file.Name.Length-4), file.Length.ToString() + " Байт", file.CreationTime.ToString(), file.FullName });
                    // получаем полный путь к файлу и кидаем его в список 
                    //  MessageBox.Show(files.ElementAt(0).ElementAt(0));

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            // listView1.Clear();
            if (files.Count > 0)
            {
                NoFilesTextBox.Visible=false;
                foreach (string[] s in files)
                {
                    ListViewItem i = new ListViewItem(s);
                    listView1.Items.Add(i);
                }
            }
            else { button9.Visible = false; NoFilesTextBox.Visible = true; }
        
        
        }
        private void button2_Click(object sender, EventArgs e)
        {
            main.Enabled = true; main.Show();
            this.Close();
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog f = new OpenFileDialog();
           f.InitialDirectory =@"D:/";
           f.Filter = "Microsoft Word files (.doc,.docx)|*.doc;*.docx|Micosoft Exel (.xls)|*.xls|txt files (*.txt)|*.txt|All files|*.*";
            f.FilterIndex = 1;
            f.RestoreDirectory = true;
            f.Title = "Добавить новый документ";
            if (f.ShowDialog() == DialogResult.OK)
            {
               //columnHeader1.ListView.Items.Add(f.SafeFileName);
               //columnHeader2.ListView.Items.Add(File.GetAttributes(f.FileName).ToString());
               //columnHeader3.ListView.Items.Add(File.GetCreationTime(f.FileName).ToString());
               //columnHeader4.LView.Items.Add(f.FileName);
                byte[] hash = ElementalEncryptor.getHashSha256(Properties.Settings.Default.Password);
                var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};
                try
                {
                    byte[] enc = ElementalEncryptor.encryptStream(File.ReadAllBytes(f.FileName), hash, sevenItems);
                    File.WriteAllBytes(workdir + System.IO.Path.GetFileName(f.FileName) + ".tth", enc);
                    //скрытый
                    System.IO.File.SetAttributes(workdir + System.IO.Path.GetFileName(f.FileName) + ".tth", System.IO.FileAttributes.Hidden);
                    //только для чтения
                   // System.IO.File.SetAttributes(workdir + System.IO.Path.GetFileName(f.FileName) + ".tth", System.IO.FileAttributes.ReadOnly);
                    //системный
                    //System.IO.File.SetAttributes(workdir + System.IO.Path.GetFileName(f.FileName) + ".tth", System.IO.FileAttributes.System);
                }
                catch (Exception ex) {/* MessageBox.Show(ex.Message); return;*/ }
               // ElementalEncryptor.EncryptFile(f.FileName, workdir + System.IO.Path.GetFileName(f.FileName));
               // string[] row = { f.SafeFileName, workdir+System.IO.Path.GetFileName(f.FileName) };
               // ListViewItem i = new ListViewItem(row);
               // listView1.Items.Add(i);
                //files.Add(row);
               // MessageBox.Show(workdir + System.IO.Path.GetFileName(f.FileName));
                toolStripStatusLabel1.Text = f.FileName + " добавлен";
                refreshFileList(textBox1.Text);
                // listView1.Column
            
            
            
            }
        }


        private void извлечьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count==1)
                try
                {
                    int index = listView1.SelectedItems[0].Index;
                    string extention = Path.GetExtension(listView1.Items[index].SubItems[3].Text.Substring(0, listView1.Items[index].SubItems[3].Text.Length - 4));
                    //  MessageBox.Show(extention + " " + listView1.Items[index].SubItems[3].Text);
                    //MessageBox.Show(listView1.Items[index].SubItems[3].Text.Substring(0,listView1.Items[index].SubItems[3].Text.Length));
                    if (listView1.SelectedItems.Count > 0)
                    {
                        SaveFileDialog fd = new SaveFileDialog();

                        fd.Filter = "Normalized file(" + extention + ")|*" + extention;//"txt files (*.txt)|*.txt|Microsoft Word files (.doc, .docx)|*.doc;*.docx|Microsoft Exel (.xls)|*.xls";
                        fd.FilterIndex = 1;
                        fd.Title = "Извлечение исходного файла";
                        if (fd.ShowDialog() == DialogResult.OK)
                        {
                            // ElementalEncryptor.DecryptFile(listView1.Items[index].SubItems[3].Text, fd.FileName);
                            byte[] hash = ElementalEncryptor.getHashSha256(Properties.Settings.Default.Password);
                            var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};
                            byte[] dec = ElementalEncryptor.decryptStream(File.ReadAllBytes(listView1.Items[index].SubItems[3].Text), hash, sevenItems);
                            File.WriteAllBytes(fd.FileName, dec);
                            toolStripStatusLabel1.Text = fd.FileName + " дешифрован"; 
                
                        }
                           }
                }
                catch (Exception ex) {/* MessageBox.Show(ex.Message);*/ }
        
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
               // workdir = fbd.SelectedPath;
                refreshFileList(textBox1.Text);
            }
                // string[] files = Directory.GetFiles(fbd.SelectedPath);
           // System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
        }

        //private void button6_Click(object sender, EventArgs e)
        //{
            
        //    m.Activate();
        //    m.Show();
        //    //this.
        //}

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            try
            {
            int index = listView1.SelectedItems[0].Index;
            string extention = Path.GetExtension(listView1.Items[index].SubItems[3].Text);
           
                File.Delete(listView1.Items[index].SubItems[3].Text);
                statusStrip1.Text =listView1.Items[index].SubItems[3].Text+ " Успешно удален";
                refreshFileList(textBox1.Text);
            }
            catch (Exception ex) {/* MessageBox.Show("Удалить не удалось, "+ex.Message);*/ }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            try
            {
                int index = listView1.SelectedItems[0].Index;
                string extention = Path.GetExtension(listView1.Items[index].SubItems[3].Text.Substring(0, listView1.Items[index].SubItems[3].Text.Length - 4));
              //  MessageBox.Show(extention + " " + listView1.Items[index].SubItems[3].Text);
                //MessageBox.Show(listView1.Items[index].SubItems[3].Text.Substring(0,listView1.Items[index].SubItems[3].Text.Length));
                if (listView1.SelectedItems.Count > 0)
                {
                    SaveFileDialog fd = new SaveFileDialog();

                    fd.Filter = "Normalized file(" + extention + ")|*" + extention;//"txt files (*.txt)|*.txt|Microsoft Word files (.doc, .docx)|*.doc;*.docx|Microsoft Exel (.xls)|*.xls";
                    fd.FilterIndex = 1;
                    fd.Title = "Извлечение исходного файла";
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        // ElementalEncryptor.DecryptFile(listView1.Items[index].SubItems[3].Text, fd.FileName);
                        byte[] hash = ElementalEncryptor.getHashSha256(Properties.Settings.Default.Password);
                        var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};
                        byte[] dec = ElementalEncryptor.decryptStream(File.ReadAllBytes(listView1.Items[index].SubItems[3].Text), hash, sevenItems);
                        File.WriteAllBytes(fd.FileName, dec);
                    }

                }
            }
            catch (Exception ex) { /*MessageBox.Show(ex.Message);*/ }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            label4.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Underline);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            label4.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Underline);
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            main.Enabled = true; main.Show();
            main.Top = this.Top;
            main.Left = this.Left;
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listView1.SelectedItems[0].Index;
                string extention = Path.GetExtension(listView1.Items[index].SubItems[3].Text);

                File.Delete(listView1.Items[index].SubItems[3].Text);
                refreshFileList(textBox1.Text);
                statusStrip1.Text =listView1.Items[index].SubItems[3].Text+ " Успешно удален";
            }
            catch (Exception ex) {/* MessageBox.Show("Удалить не удалось, " + ex.Message); */}
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            try
            {
                int index = listView1.SelectedItems[0].Index;
                string extention = Path.GetExtension(listView1.Items[index].SubItems[3].Text.Substring(0, listView1.Items[index].SubItems[3].Text.Length - 4));
               // MessageBox.Show(extention + " " + listView1.Items[index].SubItems[3].Text);
                if (listView1.SelectedItems.Count > 0)
                {
                    SaveFileDialog fd = new SaveFileDialog();

                    fd.Filter = "Normalized file(" + extention + ")|*" + extention;//"txt files (*.txt)|*.txt|Microsoft Word files (.doc, .docx)|*.doc;*.docx|Microsoft Exel (.xls)|*.xls";
                    fd.FilterIndex = 1;
                    fd.Title = "Извлечение исходного файла";
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        // ElementalEncryptor.DecryptFile(listView1.Items[index].SubItems[3].Text, fd.FileName);
                        byte[] hash = ElementalEncryptor.getHashSha256(Properties.Settings.Default.Password);
                        var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};
                        byte[] dec = ElementalEncryptor.decryptStream(File.ReadAllBytes(listView1.Items[index].SubItems[3].Text), hash, sevenItems);
                        File.WriteAllBytes(fd.FileName, dec);
                    }

                }
            }
            catch (Exception ex) {/* MessageBox.Show(ex.Message);*/ }
        }

        private void button6_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(workdir);
          //  string s = "l;l;"; 
            textBox1.Text = workdir;
            refreshFileList(textBox1.Text);
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

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            //toolStripStatusLabel2.Text = "Обработка...";
            e.Effect = DragDropEffects.Copy;
           
        }
        public Thread encThread;
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            bool flag = false;
            int x = 0;
            toolStripStatusLabel1.Text = "Обработка...";
          //  bw.CancelAsync(); 
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i = 0;
            if (files.Count() < 100)
                foreach (string file in files)
                {
                    byte[] hash = ElementalEncryptor.getHashSha256(Properties.Settings.Default.Password);
                    var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 
                ,0x20,0x20,0x20,0x20,0x20,0x20};

                    try
                    {

                        byte[] enc = ElementalEncryptor.encryptStream(File.ReadAllBytes(file), hash, sevenItems);
                        File.WriteAllBytes(workdir + System.IO.Path.GetFileName(file) + ".tth", enc);
                        //скрытый
                        System.IO.File.SetAttributes(workdir + System.IO.Path.GetFileName(file) + ".tth", System.IO.FileAttributes.Hidden);
                        //только для чтения
                       // System.IO.File.SetAttributes(workdir + System.IO.Path.GetFileName(file) + ".tth", System.IO.FileAttributes.ReadOnly);
                        //системный
                       // System.IO.File.SetAttributes(workdir + System.IO.Path.GetFileName(file) + ".tth", System.IO.FileAttributes.System);
                        //toolStripProgressBar1.Value = i / files.Count()*100;
                        i++;
                        //                     Thread some_thread = new Thread
                        //                     (delegate()
                        //                     {
                        //    {
                        bw.ReportProgress(i,files.Count());

                        //        Invoke((MethodInvoker)delegate
                        //        {
                        //            toolStripProgressBar1.Value = i / files.Count() * 100;          //update the progress bar here
                        //        });

                        //    }
                        //});some_thread.Start();

                    }
                    catch (Exception ex) { /*MessageBox.Show(ex.Message); */flag = true; return; }
                    // MessageBox.Show(workdir + System.IO.Path.GetFileName(file));
                    //  bw.CancelAsync();
                    finally
                    {
                        toolStripStatusLabel1.Text = "Добавлено "+files.Count().ToString()+" файлов";
                        //  toolStripStatusLabel1.Text = "Обработка..."+e.
                    }
                }
            else MessageBox.Show("Можно добавлять не больше 100 файлов за раз","Предупреждение");
            //toolStripStatusLabel1.Text = "Обработка...";
            refreshFileList(textBox1.Text);
            toolStripProgressBar1.Value = 0;
       
        }


        //void bw_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //Task in background
        //    //

        //    //inside loop
        //    bw.ReportProgress(100); //50 is percentage
        //}

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //make sure ProgressBar is not continuous, and has a max value
            toolStripProgressBar1.Value = e.ProgressPercentage;
            //  toolStripStatusLabel1.Text = "Обработка..."+i.ToString()+" из "+max.ToString();
            
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            
           // ToolTip t = new ToolTip();
           // t.ToolTipTitle = textBox1.Text;
         //   t.Show(textBox1.Text,this,textBox1.Left,textBox1.Top,200);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 1)
                button9.Visible = true;
            else
                button9.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 1)
            {
                DialogResult result = MessageBox.Show("Удалить выбранные объекты (" + listView1.SelectedItems.Count.ToString() + ")шт?", "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    try
                    {
                        for (int i = 0; i < listView1.SelectedItems.Count; i++)
                            File.Delete(listView1.SelectedItems[i].SubItems[3].Text);
                        toolStripStatusLabel1.Text = "Файлы удалены"; //"Удалено файлов: " + listView1.SelectedItexms.Count.ToString(); 
                    }
                    catch (Exception ex) { }
            }
               // finally { MessageBox.Show("Выбранные файлы успешно удалены"); }
            refreshFileList(textBox1.Text);
                    //MessageBox.Show(listView1.SelectedItems[i].SubItems[3].Text);
                 //foreach (ListView.ListViewItemCollection i in listView1.SelectedItems)
                 //{
                 //    MessageBox.Show(i.ToString());//Path.GetExtension(listView1.Items[index].SubItems[3].Text.Substring(0, listView1.Items[index].SubItems[3].Text.Length - 4)));
                 
                 //}
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
           // toolTip1.Show(textBox1.Text, this);
        }


    }
}

      

    
