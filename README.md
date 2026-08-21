# 🛒 PRN232 — eBay Clone (Seller Subsystem)
Dự án môn **PRN232** — Xây dựng phân hệ Người bán (Seller Subsystem) cho sàn thương mại điện tử **Clone eBay**, mô phỏng các quy trình nghiệp vụ thực tế từ đăng ký/xác minh người bán, đăng bán sản phẩm & đấu giá, quản lý mã giảm giá, xử lý đơn hàng & vận chuyển, chăm sóc khách hàng, đổi trả/khiếu nại đến báo cáo doanh số chi tiết.
---
## 📌 1. Tổng quan & Công nghệ sử dụng
### 🛠 Tech Stack
- **Backend (BE):** .NET 8 Web API, Entity Framework Core 8.0, SQL Server, SignalR (Realtime Chat), JWT Authentication, BCrypt.NET.
- **Frontend (FE):** ASP.NET Core MVC (.NET 8), Razor Views, Bootstrap / Tailwind CSS, Axios / Fetch API.
- **Database:** SQL Server (`CloneEbayDB`) với **17 bảng dữ liệu cốt lõi + 1 bảng tổng hợp hiệu năng** (`SalesSummary`).
---
## 👥 2. Phân công 7 Module Fullstack (Nhóm 2)
| Module | Thành viên phụ trách | Phạm vi nghiệp vụ & Bảng dữ liệu |
| :--- | :--- | :--- |
| **Module 1** | **Thành viên 1 (Leader)** | Auth (Register, Login JWT, Refresh Token), Nộp & Duyệt hồ sơ Seller (Mô phỏng KYC 3 lớp), Quản lý gian hàng (`StoreProfile`).<br>👉 *Bảng:* `User`, `Store` |
| **Module 2** | **Thành viên 2** | Đăng bán sản phẩm (Fixed price & Auction), Danh sách & Filter, Cập nhật/Ẩn/Xoá sản phẩm, Quản lý tồn kho (`Inventory`), Tự động đóng phiên đấu giá.<br>👉 *Bảng:* `Product`, `Inventory`, `Category`, `Bid` |
| **Module 3** | **Thành viên 3** | Tiếp thị & Mã giảm giá (`Coupon` gắn từng sản phẩm), Kiểm tra hiệu lực mã lúc checkout, Thống kê lượt dùng mã.<br>👉 *Bảng:* `Coupon` |
| **Module 4** | **Thành viên 4** | Quản lý Đơn hàng Seller, Xác nhận đơn & Trừ tồn kho, Cập nhật trạng thái vận chuyển theo State Machine, Tạo nhãn vận chuyển giả lập (`ShippingInfo`).<br>👉 *Bảng:* `OrderTable`, `OrderItem`, `Payment`, `ShippingInfo` |
| **Module 5** | **Thành viên 5** | Xem & Phản hồi Đánh giá (`Review` — phản hồi 1 lần duy nhất), Tính chỉ số uy tín (`Feedback`), Xử lý Yêu cầu Đổi trả (`ReturnRequest` - Money Back Guarantee), Giải quyết Khiếu nại (`Dispute`).<br>👉 *Bảng:* `Review`, `Feedback`, `ReturnRequest`, `Dispute` |
| **Module 6** | **Thành viên 6** | Dashboard Báo cáo Doanh số (KPI Cards, Top sản phẩm bán chạy, Biểu đồ doanh thu theo thời gian, Chỉ số hiệu suất giao trễ/dispute).<br>👉 *Bảng:* `SalesSummary` (Denormalized) |
| **Module 7** | **Thành viên 7** | Kênh Chat CSKH Realtime (SignalR / Fallback Polling), Lịch sử hội thoại 1-1, Đánh dấu đã đọc & Badge chưa đọc.<br>👉 *Bảng:* `Message` |
---
## 🗄 3. Cấu trúc Database (`CloneEbayDB`)
Sơ đồ gồm **17 bảng chính + 1 bảng tổng hợp**:
1. `User`: Tài khoản hệ thống (Role: Buyer, Seller, Admin).
2. `Store`: Hồ sơ gian hàng & trạng thái xác minh KYC (`Pending`, `Approved`, `Rejected`).
3. `Address`: Địa chỉ người dùng.
4. `Category`: Danh mục sản phẩm (hỗ trợ đa cấp).
5. `Product`: Thông tin sản phẩm (Fixed Price hoặc Auction đấu giá).
6. `Inventory`: Tồn kho sản phẩm.
7. `Coupon`: Mã giảm giá gắn theo từng sản phẩm.
8. `OrderTable`: Đơn hàng tổng.
9. `OrderItem`: Chi tiết từng món hàng trong đơn.
10. `Payment`: Trạng thái thanh toán (VNPay, CreditCard, PayPal, COD).
11. `ShippingInfo`: Trạng thái & Mã vận đơn (`Preparing` → `LabelCreated` → `HandedToCarrier` → `InTransit` → `Delivered`).
12. `ReturnRequest`: Yêu cầu đổi trả trong chính sách eBay Money Back Guarantee.
13. `Bid`: Lượt đặt giá cho các sản phẩm đấu giá.
14. `Review`: Đánh giá của Buyer & Phản hồi 1 lần của Seller.
15. `Feedback`: Chỉ số uy tín tổng hợp của Seller (% Positive Rating).
16. `Dispute`: Khiếu nại khi 2 bên không thoả thuận được đổi trả.
17. `Message`: Hộp thư trao đổi giữa Buyer & Seller.
18. `SalesSummary`: Bảng tổng hợp doanh thu phục vụ Dashboard load < 1s.
---
## 🔌 4. Quy ước chuẩn API (API Standard)
- **Base URL:** `/api`
- **Authentication:** `Authorization: Bearer <access_token>`
- **Response Envelope chuẩn:**
  - **Thành công (200 / 201):**
    ```json
    {
      "success": true,
      "data": { ... },
      "meta": { "traceId": "c7a1e2f0-..." }
    }
    ```
  - **Thất bại (4xx / 5xx):**
    ```json
    {
      "success": false,
      "error": {
        "code": "PRODUCT_NOT_FOUND",
        "message": "Không tìm thấy sản phẩm",
        "details": []
      },
      "meta": { "traceId": "c7a1e2f0-..." }
    }
    ```
- **Correlation ID:** Mọi response đều mang Header `X-Correlation-Id` giúp truy vết log hệ thống.
---
## 🚀 5. Hướng dẫn Chạy dự án
### Step 1: Khởi tạo Database
1. Mở **SQL Server Management Studio (SSMS)** hoặc Azure Data Studio.
2. Mở file [`clone_ebay_sqlserver_schema.sql`](./clone_ebay_sqlserver_schema.sql) và nhấn **Execute**.
3. Hệ thống sẽ tự động tạo database `CloneEbayDB`, toàn bộ 18 bảng, Index và dữ liệu thử nghiệm ban đầu (Tài khoản mẫu: `admin`, `seller1`, `buyer1` / Password: `Passw0rd!`).
### Step 2: Chạy Backend (`BE`)
```bash
cd BE
dotnet restore
dotnet run

Step 3: Chạy Frontend (FE)
bash


cd FE
dotnet restore
dotnet run

📁 6. Cấu trúc thư mục Source Code
text


PRN232_Ebay_Group_Project/
├── clone_ebay_sqlserver_schema.sql   # File khởi tạo Database & Seed Data
├── PRN232_Ebay_Group_Project.sln     # Solution file (.NET 8)
├── BE/                               # Project Backend (ASP.NET Core Web API)
│   ├── Common/                       # ApiResponse Envelope, PaginationFilter
│   ├── Controllers/                  # 14 API Controllers (Module 1 - 7)
│   ├── Data/                         # AppDbContext & EF Core Configurations
│   ├── Entities/                     # 18 Entity Models
│   ├── Guards/                       # SellerVerifiedAttribute (Auth Guard)
│   ├── Hubs/                         # SignalR MessageHub (Chat Realtime)
│   ├── Middleware/                   # CorrelationIdMiddleware
│   ├── Services/                     # Interfaces & Service Implementations
│   └── Program.cs                    # DI Registration & App Startup
└── FE/                               # Project Frontend (ASP.NET Core MVC)
    ├── Controllers/                  # MVC View Controllers (Module 1 - 7)
    ├── Models/                       # View Models
    └── Views/                        # Razor Views UI
