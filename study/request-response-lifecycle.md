# Vòng đời Request & Response trong MusicShop

Tài liệu này tổng hợp kiến thức về cách một Request được xử lý, từ khâu xác thực dữ liệu cho đến khi được chuyển đổi thành JSON để trả về cho Client.

---

## 1. Giai đoạn Request & Validation (MediatR Pipeline)

Khi một Request (Command/Query) được gửi vào hệ thống, nó sẽ đi qua các **MediatR Pipeline Behaviors** trước khi chạm tới Handler.

### ValidationBehavior
Hệ thống sử dụng `ValidationBehavior` để thực hiện kiểm tra dữ liệu tự động:
- **AbstractValidator**: Các class như `RegisterCommandValidator` kế thừa từ `AbstractValidator<T>` sẽ định nghĩa tập luật cho dữ liệu.
- **Thực thi**: `ValidationBehavior` sẽ inject tất cả validators tương ứng và gọi `ValidateAsync`.
- **Ngắt quãng**: Nếu có lỗi, một `ValidationException` sẽ được ném ra ngay lập tức, ngăn chặn việc thực thi logic nghiệp vụ trong Handler.

---

## 2. Giai đoạn Logic nghiệp vụ (Application Layer)

Sau khi vượt qua bước Validation, Request sẽ được xử lý bởi Handler. Kết quả trả về thường sử dụng **Result Pattern**.

### Result Pattern
Thay vì ném Exception cho các lỗi nghiệp vụ (ví dụ: User không tồn tại), hệ thống trả về đối tượng `Result<T>`:
- **Success**: Chứa dữ liệu trả về (`Value`).
- **Failure**: Chứa đối tượng `Error` (bao gồm `Code` và `Message`).

---

## 3. Giai đoạn Phản hồi (API Layer)

Tại lớp API, các Controller kế thừa từ `BaseApiController` sẽ chuyển đổi `Result<T>` thành `ActionResult`.

### BaseApiController & HandleResult
Các phương thức như `HandleResult` hoặc `HandlePaginatedResult` thực hiện:
- Nếu thành công: Trả về `Ok(ApiResponse.SuccessResult(value))`.
- Nếu thất bại: Gọi `MapError` để phân loại lỗi.

### Phân loại lỗi (Mapping Errors)
Hàm `MapError` dựa vào `Error.Code` để trả về Status Code phù hợp:
- `*.NotFound` -> 404 Not Found.
- `*.Conflict` -> 409 Conflict.
- Mặc định -> 400 Bad Request.

---

## 4. Giai đoạn Xử lý lỗi toàn cục (Global Error Handling)

Nếu có một Exception (ngoại lệ ngoài ý muốn) xảy ra, `GlobalExceptionHandler` sẽ can thiệp.

### GlobalExceptionHandler & ProblemDetails
- **Xử lý ValidationException**: Nếu lỗi đến từ bước Validation, nó sẽ được định dạng lại thành cấu trúc `ProblemDetails` (chuẩn RFC 7807).
- **Extensions["errors"]**: Các lỗi sẽ được nhóm theo tên thuộc tính (`PropertyName`) để Client dễ dàng hiển thị lỗi dưới từng ô nhập liệu.

---

## 5. Giai đoạn Tuần tự hóa (Serialization - Output Formatters)

Đây là bước cuối cùng trước khi dữ liệu rời khỏi server.

### Quy trình Serialization
1. **ActionResult Execution**: ASP.NET Core thực thi kết quả trả về từ Controller.
2. **Output Formatters**: Framework sử dụng bộ lọc (mặc định là `SystemTextJsonOutputFormatter`) để kiểm tra Header `Accept`.
3. **WriteAsJsonAsync**: Đối tượng C# (`ApiResponse` hoặc `ProblemDetails`) được thư viện `System.Text.Json` chuyển đổi thành chuỗi JSON và ghi vào `Response.Body`.
4. **Content-Type**: Header `Content-Type` được thiết lập là `application/json`.

---

> [!NOTE]
> Việc tách biệt các khâu này giúp hệ thống đảm bảo tính nhất quán (Consistency), dễ bảo trì và cung cấp thông tin lỗi tường minh cho Client theo chuẩn quốc tế.
