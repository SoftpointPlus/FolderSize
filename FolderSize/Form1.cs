using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderSize
{
    public partial class Form1 : Form
    {
        private List<SizeInfo> sizeList;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetFolderInfo fi = new GetFolderInfo();
            sizeList = new List<SizeInfo>();
            fi.RecurciveFileSearch(sizeList, textBox1.Text);
            sizeList.Sort(delegate(SizeInfo mc1, SizeInfo mc2)
            {
                return mc1.size.CompareTo(mc2.size) * (-1);
            }

                );
            listInfo.VirtualListSize = sizeList.Count;
            listInfo.Refresh();

        }

        private void listInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listInfo_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listInfo.SelectedIndices.Count == 0) return;
                Process.Start(sizeList[listInfo.SelectedIndices[0]]._path, "-p");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия папки.\n" + ex.Message, "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void listInfo_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= 0 && e.ItemIndex < sizeList.Count)
            {
                SizeInfo sz;
                sz = sizeList[e.ItemIndex];
                e.Item = new ListViewItem(sz._path.ToString());
                e.Item.SubItems.Add(GetHumanFileSize(sz.size));
            }

        }
        private string GetHumanFileSize(long fileSize)
        {

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (fileSize >= 1024 && order < sizes.Length - 1)
            {
                order++;
                fileSize /= 1024;
            }
            return String.Format("{0:0.##} {1}", fileSize, sizes[order]);
        }
        

        private void listInfo_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {


        }

        private void listInfo_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
 
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                Filesize.Visible = true;
                Filesize.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                Filesize.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Filesize.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog.SelectedPath;
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить содержимое выбранных папок и все содержимое рекурсивно?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (int index in listInfo.SelectedIndices)
                {
                    DeleteAllFiles(sizeList[index]._path);
                }
            }
        }
        private void DeleteAllFiles(string path)
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }



        }
    }
}
