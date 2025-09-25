# Ứng dụng MVC - Thực hành Model

Đây là một ứng dụng web ASP.NET Core MVC hoàn chỉnh với các chức năng quản lý người dùng và sản phẩm.

## Cấu trúc dự án

### Models

- **User.cs**: Model cho người dùng với các thuộc tính Name, Email, Phone
- **Product.cs**: Model cho sản phẩm với các thuộc tính Name, Description, Price, Quantity
- **Order.cs**: Model cho đơn hàng với quan hệ với User và Product
- **ErrorViewModel.cs**: Model cho trang lỗi

### Controllers

- **HomeController**: Controller chính hiển thị trang chủ và các sản phẩm nổi bật
- **UserController**: Controller quản lý người dùng (CRUD operations)
- **ProductController**: Controller quản lý sản phẩm (CRUD operations)

### Services

- **IUserService & UserService**: Service layer cho business logic của User
- **IProductService & ProductService**: Service layer cho business logic của Product

### Views

- **Shared/\_Layout.cshtml**: Layout chính của ứng dụng
- **Home/**: Views cho trang chủ, privacy, error
- **User/**: Views cho CRUD operations của User
- **Product/**: Views cho CRUD operations của Product

## Tính năng

### Quản lý Người dùng

- Xem danh sách người dùng
- Tạo người dùng mới
- Xem chi tiết người dùng
- Sửa thông tin người dùng
- Xóa người dùng

### Quản lý Sản phẩm

- Xem danh sách sản phẩm
- Tìm kiếm sản phẩm
- Tạo sản phẩm mới
- Xem chi tiết sản phẩm
- Sửa thông tin sản phẩm
- Xóa sản phẩm

### Tính năng khác

- Responsive design với Bootstrap
- Validation form với Data Annotations
- Thông báo thành công/lỗi
- Xác nhận trước khi xóa
- Auto-hide alerts

## Cách chạy ứng dụng

1. Mở terminal/command prompt
2. Di chuyển đến thư mục dự án
3. Chạy lệnh: `dotnet run`
4. Mở trình duyệt và truy cập: `https://localhost:5001` hoặc `http://localhost:5000`

## Công nghệ sử dụng

- **ASP.NET Core 8.0**: Framework web
- **MVC Pattern**: Kiến trúc Model-View-Controller
- **Bootstrap 5**: CSS framework cho responsive design
- **jQuery**: JavaScript library
- **Data Annotations**: Validation
- **Dependency Injection**: Quản lý dependencies

## Cấu trúc MVC

```
├── Controllers/          # Controllers xử lý HTTP requests
├── Models/              # Data models và business entities
├── Views/               # Razor views cho UI
├── Services/            # Business logic layer
├── wwwroot/             # Static files (CSS, JS, images)
└── Program.cs           # Application entry point
```

Ứng dụng này minh họa các khái niệm cơ bản của MVC pattern trong ASP.NET Core, bao gồm:

- Separation of Concerns
- Dependency Injection
- Model Binding
- Validation
- Routing
- View Engines
