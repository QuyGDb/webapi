# Kiến thức Cốt lõi về Thiết kế Database

Tài liệu này tổng hợp các kiến thức về thiết kế cơ sở dữ liệu đã thảo luận trong dự án MusicShop.

---

## 1. Các loại Khóa (Keys)

- **Primary Key (PK - Khóa chính):** Định danh duy nhất cho mỗi dòng. Luôn luôn duy nhất và không được để trống (Not Null).
- **Foreign Key (FK - Khóa ngoại):** Tạo mối liên kết giữa các bảng bằng cách trỏ tới PK của bảng khác.
- **Surrogate Key (Khóa giả):** Một giá trị vô nghĩa (như UUID hoặc Auto-increment ID) dùng làm PK để đảm bảo tính ổn định cao nhất, không thay đổi theo nghiệp vụ.
- **Composite Key (Khóa kết hợp):** Khóa chính được tạo thành từ 2 hoặc nhiều cột kết hợp lại. Thường dùng trong các bảng trung gian (Junction Tables).

---

## 2. Bảng trung gian (Junction Tables)

- **Định nghĩa:** Dùng để giải quyết quan hệ Nhiều-Nhiều (Nhiều-Nhiều).
- **Quy tắc thiết kế:** Nên sử dụng **Composite Key** thay vì ID riêng nếu hàng đó không cần được tham chiếu bởi bên thứ ba nào khác.
- **Lợi ích:** Đảm bảo dữ liệu không bị trùng lặp ở mức schema và tiết kiệm không gian lưu trữ.

---

## 3. Extension Table & Shared Primary Key

Trong quan hệ 1-1 mở rộng (ví dụ: `ProductVariant` và `VinylAttributes`):
- **Shared Primary Key:** Sử dụng luôn ID của bảng chính làm khóa chính của bảng mở rộng.
- **Cấu trúc:** Cột `ProductVariantId` trong bảng `VinylAttributes` vừa là Khóa chính (PK), vừa là Khóa ngoại (FK).
- **Ưu điểm:** Cưỡng chế quan hệ 1-1 tuyệt đối ở tầng database và làm gọn schema.

---

## 4. Normalization vs Denormalization

- **Normalization (Chuẩn hóa):** Chia nhỏ dữ liệu ra các bảng để tránh trùng lặp. Giúp dữ liệu nhất quán nhưng làm tăng số lượng lệnh `JOIN`.
- **Denormalization (Phi chuẩn hóa):** Cố tình lưu thừa dữ liệu ở một bảng khác để tăng tốc độ truy vấn (Read Performance).
- **Khi nào dùng Denormalize?** Khi hệ thống gặp nút thắt về hiệu năng lọc/tìm kiếm, hoặc cần lưu lại ảnh chụp thông tin tại một thời điểm (như lưu giá sản phẩm vào đơn hàng).

---

## 5. Navigation Properties trong EF Core

Trong code C#, chúng ta thường thấy cả cột ID và một đối tượng Entity:
```csharp
public Guid ProductId { get; set; }     // Foreign Key (Cỗ máy dùng)
public Product Product { get; set; }    // Navigation Property (Lập trình viên dùng)
```
- **Id:** Dùng để database biết đường liên kết.
- **Navigation Property:** Giúp lập trình viên có thể truy cập trực tiếp các thuộc tính của đối tượng liên quan (ví dụ: `variant.Product.Name`) mà không cần viết lệnh JOIN thủ công.

---

> [!TIP]
> Luôn bắt đầu bằng việc thiết kế Database chuẩn (Normalized). Chỉ thực hiện Denormalization khi thực sự cần thiết cho hiệu năng.
