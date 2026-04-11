# Tìm hiểu về HttpContext trong ASP.NET Core

`HttpContext` là trung tâm của mọi quá trình xử lý Request/Response trong ASP.NET Core. Nó đóng vai trò là một "container" chứa toàn bộ thông tin về một phiên giao dịch HTTP giữa Client và Server.

---

## 1. Ba thành phần cốt lõi

### HttpRequest (Request)
Chứa những gì Client gửi lên:
- **Method**: GET, POST, PUT, DELETE...
- **Path & Query**: URL và các tham số truy vấn.
- **Headers**: Chứa `Authorization` (JWT), `User-Agent`, `Content-Type`.
- **Body**: Dữ liệu JSON hoặc Form gửi kèm.

### HttpResponse (Response)
Chứa những gì Server trả về:
- **StatusCode**: Mã trạng thái (200, 201, 400, 401, 500...).
- **Headers**: Các thông tin bổ trợ như `Set-Cookie`.
- **Body**: Dữ liệu kết quả (thường là JSON).

### ClaimsPrincipal (User)
Đại diện cho danh tính của người thực hiện Request:
- **Identity**: Chứa cờ `IsAuthenticated` (đã đăng nhập hay chưa).
- **Claims**: Các cặp thông tin (Key-Value) giải mã từ JWT (như `UserId`, `Email`, `Role`).

---

## 2. Các thuộc tính quan trọng khác

### RequestServices (DI Container)
- Cung cấp quyền truy cập vào các Service đã đăng ký trong hệ thống Dependency Injection (Scoped Services).
- Giúp lấy ra các Repository, DBContext ngay trong vòng đời của Request.

### Items (Temporary Storage)
- Là một Dictionary (`IDictionary<object, object>`) dùng để chia sẻ dữ liệu giữa các Middleware hoặc giữa Middleware và Controller.
- Dữ liệu trong `Items` sẽ bị **hủy bỏ hoàn toàn** ngay khi Request kết thúc.

### Connection
- Cung cấp thông tin mạng như địa chỉ IP của Client (`RemoteIpAddress`) và Port.

---

## 3. IsAuthenticated vs Authorization

- **IsAuthenticated**: Có biết khách hàng là ai không? (Có thẻ căn cước/JWT hợp lệ không?).
- **Authorization**: Khách hàng có được phép làm hành động này không? (Có quyền Admin không?).

> Trong ASP.NET Core, `HttpContext.User` không bao giờ null. Nếu chưa đăng nhập, nó sẽ chứa một "Anonymous User" với `IsAuthenticated = false`.

---

## 4. Mối quan hệ giữa HttpContext và Database

| Đặc điểm | HttpContext.User | User Entity (DB) |
| :--- | :--- | :--- |
| **Nguồn gốc** | Giải mã từ JWT | Truy vấn từ Database |
| **Dữ liệu** | Chỉ chứa các Claims cơ bản (ID, Role) | Chứa toàn bộ thông tin (Password, Profile...) |
| **Mục đích** | Xác thực nhanh và phân quyền | Xử lý logic nghiệp vụ chi tiết |

> **Quy tắc**: `HttpContext` cung cấp cho bạn `UserId`. Bạn dùng `UserId` đó để tìm `User Entity` trong DB khi cần xử lý sâu hơn.

---

> [!TIP]
> Luôn sử dụng `IHttpContextAccessor` để truy cập `HttpContext` từ các lớp không phải là Controller (như Services ở lớp Infrastructure).
