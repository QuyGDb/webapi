# Tài liệu chuyên sâu về Swagger & OpenAPI trong ASP.NET Core

Tài liệu này tổng hợp các kiến thức từ cơ bản đến nâng cao khi tích hợp Swagger vào dự án MusicShop, giúp tối ưu hóa tài liệu API cho lập trình viên.

---

## 1. Kiến trúc tổng quan (Architecture Overview)

Swagger trong .NET Core không chỉ là giao diện UI. Nó là một bộ công cụ gồm 3 phần chính:
1. **Swashbuckle.AspNetCore.Swagger**: Bộ sinh tài liệu (Object Model).
2. **Swashbuckle.AspNetCore.SwaggerGen**: Trình tạo file `swagger.json` từ code.
3. **Swashbuckle.AspNetCore.SwaggerUI**: Giao diện người dùng để tương tác với API.

---

## 2. Các khái niệm cốt lõi (Core Concepts)

### `SwaggerDoc` (Định nghĩa bản tài liệu)
Dùng để khai báo một phiên bản hoặc một nhóm API cụ thể.
* **Tác dụng**: Giúp phân tách phiên bản API (v1, v2) hoặc phân loại đối tượng sử dụng (Customer vs Admin).

### `AddSecurityDefinition` (Khai báo bảo mật)
Thông báo cho Swagger về các phương thức xác thực mà hệ thống hỗ trợ.

#### Các loại Security Schemes phổ biến:

| Loại | Đặc điểm | Độ khó | Khi nào nên dùng? |
| --- | --- | --- | --- |
| **ApiKey** | Gửi Token qua Header, Query hoặc Cookie. | Dễ | Team muốn tự kiểm soát mọi thứ, làm nhanh gọn. |
| **HTTP Basic** | Gửi Username/Password (Base64). | Rất dễ | Các hệ thống nội bộ cũ, đơn giản. |
| **OAuth2** | Luồng xác thực phức tạp (Flows). | Khó | Hệ thống lớn, cần phân quyền chi tiết, bảo mật cao. |
| **OIDC** | Lớp nâng cao của OAuth2 (OpenID Connect). | Trung bình | Khi dùng các dịch vụ Identity bên thứ 3 (Google, Keycloak). |

*   **MusicShop**: Hiện đang sử dụng **ApiKey** (Bearer Token) truyền qua Header vì tính đơn giản và linh hoạt.

---

## 3. Kỹ thuật nâng cao: Operation Filter vs Global Requirement

Có hai cách để áp dụng bảo mật vào tài liệu Swagger. Việc lựa chọn phụ thuộc vào quy mô và độ chuyên nghiệp của dự án.

### A. Cấu hình Toàn cục (Global Requirement)
*   **Cách làm**: Gọi `.AddSecurityRequirement()` trong `AddSwaggerGen`.
*   **Bản chất**: Thiết lập chính sách bảo mật cho toàn bộ tài liệu (Document-wide).
*   **Hiển thị**: Icon ổ khóa xuất hiện ở **mọi** Endpoint (Kể cả Login/Register).

### B. Cấu hình Chọn lọc (Operation Filter)
*   **Cách làm**: Sử dụng `IOperationFilter` để can thiệp vào từng Endpoint.
*   **Bản chất**: Thiết lập chính sách bảo mật ở cấp độ từng tác vụ (Operation-specific).
*   **Hiển thị**: Icon ổ khóa chỉ xuất hiện ở những nơi thực sự có `[Authorize]`.

#### So sánh nhanh:

| Đặc điểm | Toàn cục (Global) | Chọn lọc (Operation Filter) |
| --- | --- | --- |
| **Độ chính xác** | Thấp (Cào bằng mọi API) | Tuyệt đối (Khớp với Code) |
| **UX người dùng** | Gây bối rối (Thừa ổ khóa) | Minh bạch, rõ ràng |
| **Linh hoạt** | Kém | Cao (Hỗ trợ tốt `[AllowAnonymous]`) |
| **Phù hợp** | Demo, dự án siêu nhỏ | Dự án thực tế, chuyên nghiệp |

> [!TIP]
> **Lợi ích**: Giúp frontend biết chính xác API nào cần Token, tránh việc gửi Token dư thừa hoặc gây bối rối cho người dùng ở các trang công cộng như Login/Register.

---

## 4. Danh mục phản hồi (Response Documentation)

Một tài liệu tốt cần mô tả rõ các tình huống lỗi. Trong dự án này, chúng ta đã tự động hóa việc chèn các Response Code:
* **401 Unauthorized**: Khi Token sai hoặc thiếu.
* **403 Forbidden**: Khi Token đúng nhưng không đủ quyền hạn.

---

## 5. Hướng dẫn Test API bảo mật

Quy trình chuẩn để test trên giao diện Swagger:
1. Gọi `POST /auth/login` -> Lấy `accessToken`.
2. Nhấn nút **Authorize** (Icon ổ khóa xanh).
3. Nhập chuỗi: `Bearer <token_của_bạn>`.
4. Gọi các API có biểu tượng ổ khóa đóng.

---
*Tài liệu này được lưu trữ tại thư mục `/study` để phục vụ việc tra cứu và nghiên cứu lâu dài.*
