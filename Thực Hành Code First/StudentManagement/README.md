# Student Management System

## Mô tả

Hệ thống quản lý sinh viên được xây dựng bằng ASP.NET Core MVC và Entity Framework Core theo hướng Code First. Ứng dụng demo việc sử dụng EF Core để quản lý dữ liệu sinh viên, khóa học và việc đăng ký khóa học.

## Công nghệ sử dụng

- **.NET 8**
- **ASP.NET Core MVC**
- **Entity Framework Core 9.0.9**
- **SQL Server LocalDB**
- **Bootstrap 5**
- **Font Awesome Icons**

## Tính năng chính

- ✅ Code First Migration
- ✅ CRUD Operations cho Student
- ✅ Data Validation
- ✅ Responsive Design
- ✅ Seed Data
- ✅ Relationship Mapping (Student ↔ Enrollment ↔ Course)

## Cấu trúc Database

### Bảng Students

- `Id` (Primary Key)
- `Name` (Họ và tên)
- `BirthDate` (Ngày sinh)
- `Email` (Email - Unique)
- `PhoneNumber` (Số điện thoại)
- `Address` (Địa chỉ)

### Bảng Courses

- `Id` (Primary Key)
- `CourseCode` (Mã khóa học - Unique)
- `Title` (Tên khóa học)
- `Description` (Mô tả)
- `Credits` (Số tín chỉ)

### Bảng Enrollments

- `Id` (Primary Key)
- `StudentId` (Foreign Key → Students)
- `CourseId` (Foreign Key → Courses)
- `EnrollmentDate` (Ngày đăng ký)
- `Grade` (Điểm số)
- `Status` (Trạng thái: Active, Completed, Dropped)

## Cách chạy ứng dụng

### Yêu cầu hệ thống

- .NET 8 SDK
- SQL Server LocalDB (đã có sẵn với Visual Studio)

### Các bước thực hiện

1. **Clone hoặc tải project**

   ```bash
   git clone <repository-url>
   cd StudentManagement
   ```

2. **Restore packages**

   ```bash
   dotnet restore
   ```

3. **Tạo và cập nhật database**

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Chạy ứng dụng**

   ```bash
   dotnet run
   ```

5. **Truy cập ứng dụng**
   - Mở trình duyệt và truy cập: `https://localhost:7xxx` hoặc `http://localhost:5xxx`
   - URL sẽ được hiển thị trong terminal sau khi chạy `dotnet run`

## Hướng dẫn sử dụng

### Trang chủ

- Hiển thị thông tin tổng quan về ứng dụng
- Có các link dẫn đến các chức năng chính

### Quản lý Sinh viên

- **Xem danh sách**: Hiển thị tất cả sinh viên với thông tin cơ bản
- **Thêm mới**: Tạo sinh viên mới với validation đầy đủ
- **Xem chi tiết**: Hiển thị thông tin chi tiết và các khóa học đã đăng ký
- **Chỉnh sửa**: Cập nhật thông tin sinh viên
- **Xóa**: Xóa sinh viên (có cảnh báo và xác nhận)

### Dữ liệu mẫu

Ứng dụng đã có sẵn dữ liệu mẫu:

- 3 sinh viên
- 3 khóa học
- 4 enrollment records

## Cấu trúc Project

```
StudentManagement/
├── Controllers/
│   ├── HomeController.cs
│   └── StudentsController.cs
├── Data/
│   ├── SchoolContext.cs
│   └── SchoolContextFactory.cs
├── Models/
│   ├── Student.cs
│   ├── Course.cs
│   ├── Enrollment.cs
│   └── ErrorViewModel.cs
├── Views/
│   ├── Home/
│   ├── Students/
│   └── Shared/
├── Migrations/
├── wwwroot/
├── Program.cs
└── appsettings.json
```

## Migration Commands

```bash
# Tạo migration mới
dotnet ef migrations add <MigrationName>

# Cập nhật database
dotnet ef database update

# Xóa migration cuối cùng
dotnet ef migrations remove

# Xem tất cả migrations
dotnet ef migrations list
```

## Connection String

Connection string được cấu hình trong `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SchoolDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## Validation Rules

### Student

- `Name`: Required, MaxLength(100)
- `Email`: Required, Email format, Unique
- `BirthDate`: Required, Date type
- `PhoneNumber`: Phone format
- `Address`: MaxLength(200)

### Course

- `CourseCode`: Required, MaxLength(10), Unique
- `Title`: Required, MaxLength(100)
- `Description`: MaxLength(500)
- `Credits`: Required, Range(1-6)

### Enrollment

- `StudentId`: Required
- `CourseId`: Required
- `EnrollmentDate`: Required, Date type
- `Grade`: Range(0-10)
- `Status`: Required, MaxLength(20)
- Unique constraint: (StudentId, CourseId)

## Troubleshooting

### Lỗi thường gặp

1. **"No DbContext was found"**

   - Đảm bảo đã cài đặt EF Tools: `dotnet tool install --global dotnet-ef`
   - Kiểm tra SchoolContextFactory.cs đã được tạo

2. **"Cannot connect to database"**

   - Kiểm tra SQL Server LocalDB đã được cài đặt
   - Kiểm tra connection string trong appsettings.json

3. **"Migration already exists"**
   - Xóa thư mục Migrations và tạo lại: `dotnet ef migrations add InitialCreate`

## Mở rộng

Để mở rộng ứng dụng, bạn có thể:

1. **Thêm Course Management**

   - Tạo CoursesController
   - Tạo Views cho Course CRUD
   - Thêm navigation menu

2. **Thêm Enrollment Management**

   - Tạo EnrollmentsController
   - Quản lý việc đăng ký khóa học
   - Thống kê điểm số

3. **Thêm Authentication**

   - Identity Framework
   - Login/Logout
   - Authorization

4. **Thêm API**
   - Web API Controllers
   - Swagger documentation
   - JSON responses

## Liên hệ

Nếu có thắc mắc hoặc cần hỗ trợ, vui lòng tạo issue trong repository.

---

**Lưu ý**: Đây là project demo để học tập Entity Framework Core Code First. Không sử dụng cho production mà không có các cải tiến về bảo mật và hiệu suất.
