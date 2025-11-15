# Lab 05 - Lọc dữ liệu Learner theo MajorID

## Mô tả
Dự án này thực hiện chức năng lọc dữ liệu Learner theo MajorID với 2 phương pháp:
1. **Load dữ liệu đồng bộ**: Sử dụng ViewComponent và link navigation (có thể thêm asp-controller, asp-action vào RenderMajor.cshtml)
2. **Load dữ liệu không đồng bộ**: Sử dụng AJAX (đã implement)

## Cấu trúc dự án

- **Models**: Learner, Major
- **Data**: SchoolContext với DbContext và seed data
- **ViewComponents**: MajorViewComponent để hiển thị danh sách Major
- **Controllers**: LearnerController với các action Index, LearnerByMajorID, Create, Edit, Delete
- **Views**: 
  - Index.cshtml: Hiển thị danh sách Learner với AJAX filtering
  - LearnerTable.cshtml: PartialView để hiển thị bảng Learner
  - RenderMajor.cshtml: View cho MajorViewComponent (hỗ trợ AJAX)
  - Create.cshtml, Edit.cshtml, Delete.cshtml: CRUD views

## Cài đặt và chạy

### Bước 1: Restore packages
```bash
cd lab05
dotnet restore
```

### Bước 2: Restore client-side libraries (jQuery, Bootstrap)
```bash
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
libman restore
```
Hoặc nếu đã có LibMan trong Visual Studio, có thể restore qua UI.

### Bước 3: Tạo database và chạy migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Lưu ý**: Nếu chưa cài đặt EF Core tools, chạy:
```bash
dotnet tool install --global dotnet-ef
```

### Bước 4: Chạy ứng dụng
```bash
dotnet run
```

### Bước 5: Truy cập ứng dụng
Mở trình duyệt và truy cập: **http://localhost:5000/Learner**

## Chức năng

✅ **Hiển thị danh sách Learner**: Hiển thị tất cả Learner trong bảng
✅ **Lọc Learner theo Major**: Click vào tab Major (IT, Economics, Mathematics) để lọc danh sách Learner theo MajorID (sử dụng AJAX)
✅ **CRUD Operations**: 
   - Create: Tạo Learner mới
   - Edit: Sửa thông tin Learner
   - Delete: Xóa Learner
✅ **ViewComponent**: Hiển thị danh sách Major dưới dạng navigation tabs

## Công nghệ sử dụng

- ASP.NET Core MVC 8.0
- Entity Framework Core 8.0
- SQL Server LocalDB
- jQuery 3.7.1
- Bootstrap 5.3.2
- AJAX cho filtering không đồng bộ

## Cấu hình Database

Connection string được cấu hình trong `appsettings.json`:
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Lab05DB;Trusted_Connection=True;MultipleActiveResultSets=true"
```

Database sẽ được tạo tự động khi chạy migrations với tên `Lab05DB`.

