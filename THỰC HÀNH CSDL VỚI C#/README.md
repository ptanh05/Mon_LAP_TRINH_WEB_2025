# Ứng dụng Quản lý Hàng hóa - C# Windows Forms

## Mô tả

Ứng dụng quản lý hàng hóa được xây dựng bằng C# Windows Forms với kết nối SQL Server. Ứng dụng bao gồm hệ thống đăng nhập và quản lý danh mục hàng hóa với các chức năng CRUD.

## Yêu cầu hệ thống

- Visual Studio 2019 trở lên
- SQL Server (SQLEXPRESS hoặc SQL Server)
- .NET Framework 4.7.2

## Cài đặt và chạy

### Bước 1: Tạo Database

1. Mở SQL Server Management Studio
2. Chạy file `DatabaseSetup.sql` để tạo database và dữ liệu mẫu
3. Kiểm tra kết nối trong file `App.config` (mặc định: `.\SQLEXPRESS`)

### Bước 2: Chạy ứng dụng

1. Mở solution trong Visual Studio
2. Build solution (Ctrl+Shift+B)
3. Chạy ứng dụng (F5)

### Bước 3: Đăng nhập

- Tài khoản mẫu: `admin` / `123`
- Hoặc: `nguyenthuhuong` / `123`

## Chức năng chính

### Form Đăng nhập (frmDangNhap)

- Xác thực người dùng với database
- Giao diện không thể thay đổi kích thước
- Hiển thị ở giữa màn hình
- Mật khẩu được ẩn bằng ký tự \*

### Form Quản lý Hàng hóa (frmHang)

- **Hiển thị thông tin**: Tên người dùng đã đăng nhập
- **Quản lý dữ liệu**:
  - Thêm hàng hóa mới
  - Sửa thông tin hàng hóa
  - Xóa hàng hóa
  - Xem danh sách hàng hóa
- **Tải ảnh**: Chọn và hiển thị ảnh sản phẩm
- **Validation**:
  - Kiểm tra dữ liệu đầu vào
  - Đơn giá bán phải lớn hơn đơn giá nhập
  - Mã hàng không được trùng
  - Chỉ cho phép nhập số cho số lượng và giá

## Cấu trúc Database

### Bảng tblUser

- `userName` (NVARCHAR(50), PRIMARY KEY)
- `passWord` (NVARCHAR(50))

### Bảng tblHang

- `MaHang` (NVARCHAR(50), PRIMARY KEY)
- `TenHang` (NVARCHAR(50))
- `ChatLieu` (NVARCHAR(50))
- `SoLuong` (INT)
- `DonGiaNhap` (FLOAT)
- `DonGiaBan` (FLOAT)
- `Anh` (NVARCHAR(100))

## Cấu trúc Project

```
├── Form1.cs (frmHang - Form chính)
├── Form1.Designer.cs
├── frmDangNhap.cs (Form đăng nhập)
├── frmDangNhap.Designer.cs
├── Program.cs
├── App.config (Connection string)
├── DatabaseSetup.sql (Script tạo database)
└── README.md
```

## Lưu ý

- Đảm bảo SQL Server đang chạy trước khi chạy ứng dụng
- Nếu sử dụng SQL Server khác SQLEXPRESS, cần cập nhật connection string trong App.config
- Ảnh sản phẩm sẽ được lưu tên file trong database, file ảnh cần được copy vào thư mục bin/Debug của ứng dụng
