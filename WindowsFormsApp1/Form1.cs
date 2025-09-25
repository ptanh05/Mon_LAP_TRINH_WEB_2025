using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string currentPath = "";
        private string[] supportedMediaExtensions = { ".mp3", ".mp4", ".avi", ".wmv", ".wav", ".m4a", ".flv", ".mov" };
        private string[] supportedTextExtensions = { ".txt", ".rtf", ".log", ".ini", ".cfg", ".xml", ".json", ".csv" };
        private bool isLoadingDirectories = false;

        public Form1()
        {
            InitializeComponent();
            LoadDrives();
            AddContextMenu();
        }


        // Load danh sách ổ đĩa
        private void LoadDrives()
        {
            try
            {
                isLoadingDirectories = true; // Set flag để tránh recursive calls
                
                comboBox1.Items.Clear();
                DriveInfo[] drives = DriveInfo.GetDrives();
                
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        comboBox1.Items.Add($"{drive.Name} ({drive.VolumeLabel}) - {drive.DriveType}");
                    }
                    else
                    {
                        comboBox1.Items.Add($"{drive.Name} - {drive.DriveType} (Không sẵn sàng)");
                    }
                }
                
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load ổ đĩa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingDirectories = false; // Reset flag
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Tránh vòng lặp vô hạn khi đang load directories
            if (isLoadingDirectories || comboBox1.SelectedItem == null)
                return;
                
            string selectedDrive = comboBox1.SelectedItem.ToString();
            string driveLetter = selectedDrive.Substring(0, 2); // Lấy ký tự ổ đĩa (VD: C:)
            
            try
            {
                DriveInfo drive = new DriveInfo(driveLetter);
                if (drive.IsReady)
                {
                    currentPath = drive.RootDirectory.FullName;
                    LoadDirectories(currentPath);
                    LoadFiles(currentPath);
                }
                else
                {
                    MessageBox.Show($"Ổ đĩa {driveLetter} không sẵn sàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi truy cập ổ đĩa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Tránh vòng lặp vô hạn khi đang load directories
            if (isLoadingDirectories || comboBox2.SelectedItem == null)
                return;
                
            string selectedDirectory = comboBox2.SelectedItem.ToString();
            if (selectedDirectory == "..")
            {
                // Quay lại thư mục cha
                DirectoryInfo parentDir = Directory.GetParent(currentPath);
                if (parentDir != null)
                {
                    currentPath = parentDir.FullName;
                }
            }
            else
            {
                currentPath = Path.Combine(currentPath, selectedDirectory);
            }
            
            LoadDirectories(currentPath);
            LoadFiles(currentPath);
        }

        // Load danh sách thư mục
        private void LoadDirectories(string path)
        {
            try
            {
                isLoadingDirectories = true; // Set flag để tránh recursive calls
                
                comboBox2.Items.Clear();
                
                // Thêm tùy chọn quay lại thư mục cha
                if (Directory.GetParent(path) != null)
                {
                    comboBox2.Items.Add("..");
                }
                
                // Load các thư mục con
                string[] directories = Directory.GetDirectories(path);
                foreach (string dir in directories)
                {
                    string dirName = Path.GetFileName(dir);
                    comboBox2.Items.Add(dirName);
                }
                
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load thư mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingDirectories = false; // Reset flag
            }
        }

        // Load danh sách file
        private void LoadFiles(string path)
        {
            try
            {
                listBox1.Items.Clear();
                string[] files = Directory.GetFiles(path);
                
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    listBox1.Items.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedFile = listBox1.SelectedItem.ToString();
                string fullPath = Path.Combine(currentPath, selectedFile);
                
                try
                {
                    // Hiển thị thông tin file trong richTextBox
                    DisplayFileInfo(fullPath);
                    
                    // Kiểm tra và xử lý file
                    string extension = Path.GetExtension(fullPath).ToLower();
                    if (supportedMediaExtensions.Contains(extension))
                    {
                        PlayMediaFile(fullPath);
                    }
                    else if (supportedTextExtensions.Contains(extension))
                    {
                        DisplayTextFile(fullPath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xử lý file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Hiển thị thông tin file
        private void DisplayFileInfo(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                StringBuilder info = new StringBuilder();
                
                info.AppendLine($"Tên file: {fileInfo.Name}");
                info.AppendLine($"Đường dẫn: {fileInfo.FullName}");
                info.AppendLine($"Kích thước: {FormatFileSize(fileInfo.Length)}");
                info.AppendLine($"Ngày tạo: {fileInfo.CreationTime}");
                info.AppendLine($"Ngày sửa đổi: {fileInfo.LastWriteTime}");
                info.AppendLine($"Thuộc tính: {fileInfo.Attributes}");
                info.AppendLine($"Phần mở rộng: {fileInfo.Extension}");
                
                richTextBox1.Text = info.ToString();
            }
            catch (Exception ex)
            {
                richTextBox1.Text = $"Lỗi khi đọc thông tin file: {ex.Message}";
            }
        }

        // Format kích thước file
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void PlayMediaFile(string filePath)
        {
            try
            {
                // Hiển thị tên bài hát
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                // Phát nhạc bằng Windows Media Player
                axWindowsMediaPlayer1.URL = filePath;
                axWindowsMediaPlayer1.Ctlcontrols.play();

                // Kiểm tra file lời bài hát (txt hoặc rtf)
                string lyricTxt = Path.ChangeExtension(filePath, ".txt");
                string lyricRtf = Path.ChangeExtension(filePath, ".rtf");

                if (File.Exists(lyricTxt))
                {
                    richTextBox1.Text = File.ReadAllText(lyricTxt, Encoding.UTF8);
                }
                else if (File.Exists(lyricRtf))
                {
                    richTextBox1.LoadFile(lyricRtf, RichTextBoxStreamType.RichText);
                }
                else
                {
                    richTextBox1.Text = "Không tìm thấy lời bài hát.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi phát nhạc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hiển thị nội dung file text
        private void DisplayTextFile(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath, Encoding.UTF8);
                richTextBox1.Text = content;
            }
            catch (Exception ex)
            {
                richTextBox1.Text = $"Lỗi khi đọc file text: {ex.Message}";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Có thể thêm logic xử lý khi text thay đổi nếu cần
        }

        // Thêm menu context cho listBox để có thêm tùy chọn
        private void AddContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            
            ToolStripMenuItem openItem = new ToolStripMenuItem("Mở file");
            openItem.Click += OpenFile_Click;
            contextMenu.Items.Add(openItem);
            
            ToolStripMenuItem playItem = new ToolStripMenuItem("Phát media");
            playItem.Click += PlayFile_Click;
            contextMenu.Items.Add(playItem);
            
            ToolStripMenuItem copyPathItem = new ToolStripMenuItem("Copy đường dẫn");
            copyPathItem.Click += CopyPath_Click;
            contextMenu.Items.Add(copyPathItem);
            
            ToolStripMenuItem refreshItem = new ToolStripMenuItem("Làm mới");
            refreshItem.Click += Refresh_Click;
            contextMenu.Items.Add(refreshItem);
            
            listBox1.ContextMenuStrip = contextMenu;
        }

        private void PlayFile_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedFile = listBox1.SelectedItem.ToString();
                string fullPath = Path.Combine(currentPath, selectedFile);
                
                try
                {
                    string extension = Path.GetExtension(fullPath).ToLower();
                    if (supportedMediaExtensions.Contains(extension))
                    {
                        PlayMediaFile(fullPath);
                    }
                    else
                    {
                        MessageBox.Show("File này không phải là file media!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể phát file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedFile = listBox1.SelectedItem.ToString();
                string fullPath = Path.Combine(currentPath, selectedFile);
                
                try
                {
                    System.Diagnostics.Process.Start(fullPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể mở file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CopyPath_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedFile = listBox1.SelectedItem.ToString();
                string fullPath = Path.Combine(currentPath, selectedFile);
                Clipboard.SetText(fullPath);
                MessageBox.Show("Đã copy đường dẫn vào clipboard!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadFiles(currentPath);
        }



        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_Enter_1(object sender, EventArgs e)
        {

        }
    }
}
