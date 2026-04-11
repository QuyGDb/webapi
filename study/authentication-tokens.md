# Cơ chế Authentication: Access Token & Refresh Token (Nâng cao)

Tài liệu này giải thích chi tiết về cách hệ thống sử dụng cặp bài trùng Access Token và Refresh Token để đảm bảo an toàn và trải nghiệm người dùng, bao gồm các kịch bản thực tế về bảo mật.

---

## 1. Định nghĩa các loại Token

### Access Token (JWT) - "Thẻ gửi xe"
- **Bản chất**: Stateless (Không trạng thái), Transparent (Trong suốt).
- **Đặc điểm**: Server không cần lưu vào DB, chỉ cần check chữ ký. Chứa đầy đủ thông tin định danh.
- **Thời gian sống**: Rất ngắn (15-30 phút) để giảm thiểu thiệt hại nếu bị đánh cắp.

### Refresh Token (Opaque) - "Thẻ căn cước"
- **Bản chất**: Stateful (Có trạng thái), Opaque (Mờ đục).
- **Đặc điểm**: Bắt buộc phải lưu vào Database. Là một chuỗi ngẫu nhiên vô nghĩa nếu không có Database đối soát.
- **Thời gian sống**: Dài (nhiều ngày/tuần) để duy trì phiên đăng nhập.

---

## 2. Cơ chế xoay vòng (Token Rotation)

Hệ thống luôn tạo mới **cả hai** loại Token mỗi khi thực hiện làm mới (Refresh).

1.  **Cấp mới**: Khi AT1 hết hạn, Client gửi RT1 lên Server.
2.  **Xoay vòng**: Server kiểm tra RT1 hợp lệ -> Cấp cặp mới (**AT2** + **RT2**).
3.  **Hủy mã cũ**: Server đánh dấu RT1 đã bị thu hồi (`RevokedAt`) và liên kết nó với mã mới (`ReplacedByTokenHash`).

---

## 3. Kịch bản phát hiện tấn công (Replay Attack)

Đây là lợi ích lớn nhất của cơ chế Rotation: **Dùng mã cũ làm "cái bẫy"**.

### Kịch bản Hacker đánh cắp Token:
1.  Hacker trộm được **RT1** và dùng nó trước người dùng thật -> Hacker lấy được **RT2** (hợp lệ).
2.  Người dùng thật sau đó dùng **RT1** để truy cập -> Server nhận ra lỗi: *"RT1 đã bị thu hồi và được thay thế bởi RT2 (mà Hacker đang giữ)"*.
3.  **Phản ứng**: Hệ thống khẳng định có sự xâm nhập trái phép. Lập tức **hủy toàn bộ chuỗi Token** (xóa cả RT2 của Hacker).
4.  **Kết quả**: Hacker bị đá văng ra ngay lập tức. Người dùng thật bị logout và được yêu cầu đổi mật khẩu.

> [!WARNING]
> Hacker có thể giữ quyền truy cập vĩnh viễn nếu họ liên tục xoay vòng token mà người dùng thật không bao giờ quay lại để "kích hoạt bẫy". Do đó, cần có thêm **Thời hạn tuyệt đối (Absolute Expiration)**.

---

## 4. Thời gian ân hạn (Grace Period) & Concurrent Requests

Đôi khi mạng chậm hoặc Client gửi 2 request Refresh cùng lúc dẫn đến lỗi "Mã cũ đã bị thu hồi".

- **Giải pháp**: Cho phép mã cũ (`RT1`) tiếp tục có hiệu lực trong một khoảng thời gian cực ngắn (ví dụ: **30-60 giây**) sau khi nó đã được xoay vòng sang `RT2`.
- **Lợi ích**: Giúp trải nghiệm mượt mà, tránh việc người dùng bị logout oan do trùng lặp request hoặc mạng yếu.

---

## 5. Chiến lược dọn dẹp Database (Cleanup Strategies)

Việc lưu trữ hàng triệu mã đã thu hồi sẽ làm nặng Database.

| Loại Token | Thời điểm xóa | Lý do |
| :--- | :--- | :--- |
| **Token Expired** | Xóa ngay lập tức (via Background Job) | Không còn giá trị sử dụng hay đối soát. |
| **Token Revoked** | Giữ lại **24-48 tiếng** | Để phục vụ việc đối soát và phát hiện Replay Attack. |
| **Logout** | Có thể xóa ngay | Dọn dẹp sạch sẽ tài nguyên của phiên đó. |

- **Background Job**: Sử dụng các công cụ như *Hangfire* hoặc *Worker Service* để quét DB và xóa vật lý các bản ghi đã quá hạn.

---

## 6. Tại sao mỗi lần Refresh lại tạo ra Cả hai (AT & RT)?

1.  **Để thực hiện Rotation**: Nếu dùng lại RT cũ, bạn không thể phát hiện nếu nó bị đánh cắp.
2.  **Tính atomic**: Mỗi "phiên làm mới" là một sự kiện duy nhất. Việc cấp một cặp khóa mới đảm bảo chuỗi xoay vòng luôn được cập nhật.

---

> [!IMPORTANT]
> **Key Takeaway**: Cơ chế Token hiện đại không chỉ là cấp quyền, mà còn là một hệ thống **Audit Log** sống động giúp phát hiện và ngăn chặn gian lận ngay khi nó xảy ra.
