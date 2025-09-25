# File Manager & Media Player

Ứng dụng Windows Forms để quản lý file và phát media với Windows Media Player tích hợp.

## Yêu cầu đề bài

### Tính năng chính:

- ✅ **Chọn ổ đĩa/Thư mục**: Duyệt và chọn ổ đĩa, thư mục
- ✅ **Hiển thị lời bài hát**: Tự động tìm và hiển thị file lời (.txt hoặc .rtf)
- ✅ **Windows Media Player**: Tích hợp Windows Media Player để phát nhạc

### Quy tắc file lời bài hát:

- File âm thanh: `Baihat.mp3`
- File lời: `Baihat.txt` hoặc `Baihat.rtf`
- Cùng tên, cùng thư mục

## Hướng dẫn cài đặt

### Bước 1: Thêm Windows Media Player vào Toolbox

1. Mở Visual Studio
2. Chuột phải vào Toolbox → Add Tab
3. Đặt tên tab: "Windows Player"
4. Chuột phải vào tab mới → Choose Items...
5. Chọn tab "COM Components"
6. Tìm và chọn "Windows Media Player"
7. Click OK

### Bước 2: Tạo giao diện

- ComboBox cho ổ đĩa
- ComboBox cho thư mục
- ListBox cho danh sách file
- RichTextBox cho hiển thị lời bài hát
- Windows Media Player control

### Bước 3: Mã lệnh

#### 3.1. Duyệt ổ đĩa và thư mục

```csharp
// Load danh sách ổ đĩa
DriveInfo[] drives = DriveInfo.GetDrives();
foreach (DriveInfo drive in drives)
{
    if (drive.IsReady)
    {
        comboBox1.Items.Add($"{drive.Name} ({drive.VolumeLabel}) - {drive.DriveType}");
    }
}

// Load thư mục con
string[] directories = Directory.GetDirectories(path);
foreach (string dir in directories)
{
    string dirName = Path.GetFileName(dir);
    comboBox2.Items.Add(dirName);
}
```

#### 3.2. Hiển thị lời bài hát và phát nhạc

```csharp
// Phát nhạc
axWindowsMediaPlayer1.URL = filePath;
axWindowsMediaPlayer1.Ctlcontrols.play();

// Tìm file lời bài hát
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
```

## Cách sử dụng

1. **Chọn ổ đĩa**: Click vào ComboBox "Ổ đĩa" để chọn ổ đĩa
2. **Chọn thư mục**: Click vào ComboBox "Thư mục" để chọn thư mục
3. **Chọn bài hát**: Click vào file trong ListBox "Tập tin"
4. **Tự động phát**: Bài hát sẽ tự động phát trong Windows Media Player
5. **Xem lời**: Lời bài hát sẽ hiển thị trong RichTextBox (nếu có file .txt hoặc .rtf)

## Cấu trúc file

```
WindowsFormsApp1/
├── Form1.cs              # Logic chính
├── Form1.Designer.cs     # Thiết kế giao diện
├── Program.cs           # Entry point
├── Properties/          # Thông tin assembly
│   ├── AssemblyInfo.cs
│   ├── Resources.Designer.cs
│   └── Settings.Designer.cs
├── WindowsFormsApp1.csproj # Project file
└── README.md           # Hướng dẫn
```

## Lưu ý

- Ứng dụng sử dụng Windows Media Player ActiveX control
- File lời bài hát phải cùng tên với file nhạc
- Hỗ trợ định dạng: .mp3, .wav, .wma, .avi, .mp4, .wmv
- File lời hỗ trợ: .txt (UTF-8), .rtf (Rich Text Format)

## Yêu cầu hệ thống

- Windows 10/11
- .NET Framework 4.7.2+
- Windows Media Player (có sẵn trên Windows)
- Visual Studio 2019/2022
