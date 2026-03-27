# Khu vực Quản trị (Admin Workspace)

Thư mục này chứa toàn bộ giao diện và logic dành cho phân quyền Admin.

## Cách chạy Admin

```bash
# Chạy ứng dụng Admin (cửa sổ riêng, không cần đăng nhập)
MoneyFlowApp.exe --admin
```

Hoặc từ Visual Studio: Cấu hình **Command line arguments** = `--admin` trong Project Properties → Debug.

## Cấu trúc

| File | Mô tả |
|------|-------|
| AdminWindow.xaml | Cửa sổ chính Admin |
| AdminDashboardControl | Tổng quan - System Metrics, biểu đồ đăng ký |
| AdminCategoryControl | Quản lý Danh mục (Giao dịch + Ngân sách) |
| AdminUserControl | Quản lý Người dùng |
| AdminCategoryEditWindow | Thêm/Sửa danh mục |

## Chức năng

- **Dashboard**: Tổng User, User Active, Tổng giao dịch, biểu đồ đăng ký 30 ngày
- **Danh mục**: CRUD + Soft Delete + Toggle trạng thái
- **User**: Tìm kiếm, lọc, Ban/Unban, Data Masking (metadata only)

> **Lưu ý**: Login/Logout do module khác phụ trách. Admin hiện chạy độc lập để test.
