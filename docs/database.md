# EF Core - Database Best Practices

Tài liệu này ghi lại các nguyên lý vận hành cốt lõi và các kỹ thuật tối ưu hóa khi làm việc với Entity Framework Core trong dự án MusicShop.

---

## 1. Change Tracking (Cơ chế Theo dõi)

EF Core sử dụng bộ máy `Change Tracker` để quản lý trạng thái của các thực thể.

- **Cơ chế:** Khi load dữ liệu lên, EF Core chụp một bản "ảnh snapshot" (bản gốc). Khi gọi `SaveChangesAsync()`, nó so sánh thực thể hiện tại với bản gốc để biết cần `UPDATE` cột nào.
- **Tối ưu (AsNoTracking):** Với các truy vấn chỉ để hiển thị (Read-only), luôn sử dụng `.AsNoTracking()`.
    - **Lợi ích:** Tiết kiệm RAM (không cần lưu snapshot) và tăng tốc độ xử lý (không cần thực hiện "Dirty Checking").

---

## 2. Eager Loading (Include & ThenInclude)

Dùng để tải các bảng liên quan trong cùng một truy vấn duy nhất.

- **Navigation Properties:** Là các "đường ống" kết nối giữa các đối tượng trong C# (Object Graph).
- **Include:** Mở van cho các quan hệ trực tiếp (Cha -> Con).
- **ThenInclude:** Đi sâu vào các quan hệ cấp thấp hơn (Con -> Cháu).
- **Nguyên lý:** EF Core biến đổi kết quả "Phẳng" (Flat Row) từ SQL thành cấu trúc "Cây" (Hierarchy) trong C#.

---

## 3. Cartesian Explosion (Bùng nổ dữ liệu)

Xảy ra khi bạn `Include` nhiều quan hệ 1-Nhiều (List) cùng lúc trong một câu lệnh JOIN duy nhất.

- **Vấn đề:** Số dòng SQL trả về tăng theo cấp số nhân (Tích của các tập hợp con).
- **Ví dụ:** 1 Artist x 10 Albums x 10 Genres = 100 dòng SQL cho chỉ 1 đối tượng Artist duy nhất.
- **Hệ quả:** Tốn băng thông mạng và RAM của Application Server để khử trùng lặp dữ liệu.

---

## 4. Giải pháp: Split Query

Sử dụng `.AsSplitQuery()` (EF Core 5.0+) để giải quyết vấn đề bùng nổ dữ liệu.

- **Cách hoạt động:** Thay vì 1 câu JOIN khổng lồ, EF Core tách thành nhiều câu lệnh SELECT riêng biệt và tự lắp ghép lại trong C#.
- **Khi nào chọn:**
    - **Single Query (Mặc định):** Dùng khi dữ liệu ít, quan hệ 1-1, hoặc cần tính nhất quán dữ liệu tuyệt đối trong một Transaction.
    - **Split Query:** Dùng khi `Include` nhiều danh sách (List) hoặc dữ liệu bảng con cực lớn.

---

## 5. Quy tắc bỏ túi (Rules of Thumb)

| Trường hợp | Kỹ thuật ưu tiên |
| :--- | :--- |
| Truy vấn danh sách để hiển thị | `AsNoTracking()` |
| Lấy 1 bản ghi kèm nhiều dữ liệu con | `Include` + `AsSplitQuery()` |
| Cập nhật bản ghi | Query mặc định (Tracking) + `SaveChangesAsync()` |
| Tối ưu hiệu năng tối đa | Sử dụng Projection (`.Select()`) để chỉ lấy các cột cần thiết |
