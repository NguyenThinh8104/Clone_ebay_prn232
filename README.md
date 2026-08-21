Đặc tả nghiệp vụ & API --- Seller Module (Clone eBay) {#đặc-tả-nghiệp-vụ--api--seller-module-clone-ebay}

Nhóm 2 · 7 Module Fullstack độc lập · Ngày lập: 20/08/2026

Tài liệu đặc tả chi tiết phân hệ Seller của hệ thống Clone eBay, bao gồm nghiệp vụ, phân công module, quy ước API, input/output, state machine và các quy tắc/ràng buộc nghiệp vụ.

Mục lục

0. Ghi chú đối chiếu & hiệu chỉnh nghiệp vụ

I. Quy ước chung cho toàn bộ API

II. Đặc tả nghiệp vụ & API theo module

Module 1 --- Auth, Seller Verification & Store

Module 2 --- Product, Listing & Inventory

Module 3 --- Marketing & Coupon

Module 4 --- Order & Shipping

Module 5 --- Review, Feedback, Return & Dispute

Module 6 --- Dashboard & Sales Report

Module 7 --- Message

Phạm vi

Tài liệu này bổ sung/nối tiếp kế hoạch Module Seller Nhóm 2, giữ nguyên phân công 7 module và lịch trình 6 ngày; nội dung đào sâu đặc tả nghiệp vụ từng feature và chuẩn hóa input/output API dựa trên tài liệu nguồn và schema CloneEbayDB 17 bảng.

ĐẶC TẢ NGHIỆP VỤ & API CHI TIẾT

PHÂN HỆ NGƯỜI BÁN (SELLER) --- CLONE EBAY

Tài liệu bổ sung --- đối chiếu nghiệp vụ thực tế eBay, đặc tả từng feature theo thành viên, chuẩn API Input/Output

Ngày lập: 20/08/2026 • Nhóm 2 • 7 Module Fullstack độc lập

Phạm vi tài liệu: bổ sung/nối tiếp file "Kế hoạch Module Seller Nhóm 2" đã có --- giữ nguyên toàn bộ phân công 7 module và lịch trình 6 ngày, chỉ đào sâu phần đặc tả nghiệp vụ từng feature và chuẩn hoá input/output của toàn bộ API, dựa trên đối chiếu với nghiệp vụ thực tế đăng bán/quản lý gian hàng trên eBay (eBay Help Center, Seller Center) và với schema CloneEbayDB thực tế (17 bảng).

0. Ghi chú đối chiếu & hiệu chỉnh nghiệp vụ so với eBay thực tế {#0-ghi-chú-đối-chiếu--hiệu-chỉnh-nghiệp-vụ-so-với-ebay-thực-tế}

Sau khi tra cứu quy trình nghiệp vụ thực tế của eBay (đăng ký seller, đăng bán, khuyến mãi, đơn hàng & vận chuyển, đổi trả/khiếu nại, feedback, hiệu suất seller, tin nhắn) và đối chiếu với schema CloneEbayDB cùng bảng phân công 7 module trong kế hoạch gốc, dưới đây là các điểm cần lưu ý/hiệu chỉnh để phần đặc tả ở các mục sau đúng nghiệp vụ hơn:

1. Số lượng bảng dữ liệu

Tài liệu kế hoạch gốc ghi "schema CloneEbayDB (15 bảng)" và "EF Core migration cho toàn bộ 15 bảng", nhưng file schema thực tế (clone_ebay_sqlserver_schema.sql) có 17 bảng: User, Address, Category, Product, OrderTable, OrderItem, Payment, ShippingInfo, ReturnRequest, Bid, Review, Message, Coupon, Inventory, Feedback, Dispute, Store. Cần cập nhật lại con số này trong tài liệu kế hoạch và trong migration checklist Ngày 1 để không sót bảng khi seed/migrate.

2. Đấu giá (Auction/Bid) chưa được module nào phụ trách

Schema có sẵn Product.isAuction, Product.auctionEndTime và cả bảng Bid --- đúng như eBay thật (eBay bán theo 2 hình thức song song: Fixed price và Auction), nhưng bảng phân công 7 module trong kế hoạch gốc không giao bảng Bid cho ai. Nếu giữ nguyên, tính năng đấu giá coi như bị bỏ trống dù đã có sẵn trong DB. Khuyến nghị: gộp một feature tối thiểu "Đấu giá" vào Module 2 (Thành viên 2 --- chủ sở hữu Product) vì auction gắn liền vòng đời của Product. Đặc tả bổ sung ở mục 2.5 bên dưới, kèm API xem danh sách bid và job tự động đóng phiên đấu giá.

3. Xác minh Seller (KYC) cần mô tả rõ hơn để đúng tinh thần eBay

eBay thật yêu cầu xác minh 3 lớp trước khi seller được bán: (1) định danh cá nhân/doanh nghiệp --- họ tên/tên pháp nhân, ngày sinh hoặc mã số thuế doanh nghiệp; (2) thông tin thuế (SSN/EIN); (3) tài khoản ngân hàng nhận tiền. Với quy mô đồ án 6 ngày, nhóm không cần tích hợp KYC thật, nhưng hồ sơ "đăng ký làm Seller" (POST /api/seller/apply) nên thu thập tối thiểu: loại seller (Individual/Business), họ tên hoặc tên doanh nghiệp, số điện thoại liên hệ --- để bản mô phỏng duyệt hồ sơ có dữ liệu thực chất thay vì chỉ đổi role. Đồng thời Store nên có thêm trường rejectionReason khi admin từ chối, để đúng nghiệp vụ "Pending → Rejected kèm lý do" như eBay.

4. Coupon: làm rõ đây là mã do buyer tự nhập ở checkout

eBay có nhiều hình thức khuyến mãi (Markdown sale theo %, Volume pricing, Order-level coupon code). Schema chỉ hỗ trợ 1 hình thức: Coupon gắn 1 productId, có trường code --- tức đúng kiểu "mã giảm giá buyer tự nhập lúc đặt hàng" (giống Order-level coupon code của eBay), không phải giảm giá tự động hiển thị ngay trên trang sản phẩm. Cần ghi rõ trong đặc tả để Thành viên 3 và Thành viên 4 thống nhất luồng: giá hiển thị trên trang Product KHÔNG đổi, chỉ khi buyer nhập đúng code lúc tạo đơn thì OrderItem.unitPrice mới được chiết khấu.

5. Tách rõ luồng Return Request và Dispute theo Money Back Guarantee

Trên eBay, "Return request" (đổi trả trong Money Back Guarantee) và "Dispute/Case" (khiếu nại leo thang khi 2 bên không tự thoả thuận được) là hai luồng khác nhau về timeline: Return request seller có 3 ngày làm việc để phản hồi (Accept/Offer refund/Decline nếu chính sách không nhận đổi trả do đổi ý), sau khi nhận hàng trả phải hoàn tiền trong 2 ngày làm việc; nếu quá 3 ngày không thống nhất, 1 trong 2 bên có thể yêu cầu eBay can thiệp (Dispute), eBay phản hồi trong ~48 giờ. Kế hoạch gốc gộp chung 2 bảng ReturnRequest và Dispute vào một feature xử lý --- nên giữ 2 API và 2 state machine riêng như đặc tả bên dưới (mục 5.3 và 5.4) để đúng nghiệp vụ thật.

6. Feedback: seller chỉ được phản hồi đúng 1 lần, không sửa/xoá

eBay quy định seller chỉ reply 1 lần cho mỗi feedback và không thể sửa/thu hồi sau khi gửi. API POST /api/reviews/{id}/reply cần chặn gọi lần 2 (trả 409 Conflict) nếu Review.response đã có giá trị, thay vì cho phép ghi đè tuỳ ý như một PUT thông thường.

7. Bổ sung khái niệm Seller Level / Performance cơ bản cho Dashboard

eBay xếp hạng seller theo 3 mức (Top Rated/Above Standard/Below Standard) dựa trên Defect Rate, Late Shipment Rate, Cases Closed Without Seller Resolution --- đánh giá theo chu kỳ hàng tháng. Dashboard M6 trong kế hoạch gốc chỉ có doanh số/top sản phẩm. Đề xuất bổ sung (mức nice-to-have, không bắt buộc trong 6 ngày) một chỉ số performance đơn giản: tỉ lệ đơn giao trễ (đếm ShippingInfo trễ so với estimatedArrival) và tỉ lệ Dispute/Return trên tổng đơn --- đặc tả ở mục 6.4.

8. Tách rõ "tạo phiếu vận chuyển" và "đã giao cho đơn vị vận chuyển"

Trên eBay, việc tạo nhãn vận chuyển (purchase/print label) và việc thực sự bàn giao hàng cho đơn vị vận chuyển là 2 hành động khác nhau --- đây là nguồn khiếu nại phổ biến ("marked as shipped nhưng chưa thực gửi"). Kế hoạch gốc dùng chung 1 API POST /api/orders/{id}/shipping-label cho cả việc sinh phiếu. Đặc tả bên dưới giữ 1 API tạo phiếu (mô phỏng) nhưng quy định rõ: OrderTable.status chỉ chuyển sang "Shipped" khi ShippingInfo.trackingNumber đã được gán và seller xác nhận bàn giao, tránh đánh đồng 2 bước.

9. Message nên gắn ngữ cảnh sản phẩm (tuỳ chọn)

eBay cho buyer "Ask seller a question" gắn liền với 1 listing cụ thể trước khi mua. Bảng Message trong schema chỉ có senderId/receiverId/content, không có productId. Đề xuất bổ sung field productId (nullable) cho Message ở mức mở rộng --- không bắt buộc để không phá schema gốc, ghi nhận trong phần khuyến nghị mở rộng cuối tài liệu.

I. Quy ước chung cho toàn bộ API {#i-quy-ước-chung-cho-toàn-bộ-api}

Toàn bộ API của 7 module dùng chung một quy ước để các module ghép nối được với nhau đúng tiến độ Ngày 1--2 theo tinh thần "contract-first" đã nêu trong kế hoạch gốc (Swagger/OpenAPI + Postman collection).

Base URL: /api • Định dạng dữ liệu: JSON (UTF-8) • Thời gian: ISO 8601 (UTC)

Xác thực (Authentication & Authorization) {#xác-thực-authentication--authorization}

Xác thực bằng JWT Bearer Token trong header Authorization: Bearer <access_token>. Payload JWT tối thiểu gồm: sub (userId), role (Buyer|Seller|Admin), storeId (nếu là Seller), verified (true/false --- Seller đã được duyệt hay chưa). Các endpoint có gắn nhãn [Seller][Verified] bắt buộc đi qua middleware SellerVerifiedGuard do Thành viên 1 cung cấp.

Envelope chuẩn cho Response

Response thành công:

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p>{</p>
<p>"success": true,</p>
<p>"data": { /* object hoặc array tuỳ endpoint */ },</p>
<p>"meta": { "traceId": "c7a1e2f0-..." }</p>
<p>}</p></td>
</tr>
</tbody>
</table>

Response lỗi:

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p>{</p>
<p>"success": false,</p>
<p>"error": {</p>
<p>"code": "PRODUCT_NOT_FOUND",</p>
<p>"message": "Không tìm thấy sản phẩm",</p>
<p>"details": []</p>
<p>},</p>
<p>"meta": { "traceId": "c7a1e2f0-..." }</p>
<p>}</p></td>
</tr>
</tbody>
</table>

Phân trang (Pagination)

Các endpoint danh sách dùng chung query: page (mặc định 1), pageSize (mặc định 20, tối đa 100), sortBy, sortOrder (asc|desc). Response bọc trong data.items (mảng) và data.pagination { page, pageSize, totalItems, totalPages }.

Mã trạng thái HTTP dùng chung

200 OK (đọc/cập nhật thành công) • 201 Created (tạo mới) • 204 No Content (xoá thành công) • 400 Bad Request (sai định dạng input) • 401 Unauthorized (thiếu/hết hạn token) • 403 Forbidden (đúng token nhưng sai role hoặc chưa Verified) • 404 Not Found • 409 Conflict (vi phạm ràng buộc nghiệp vụ, ví dụ trùng dữ liệu hoặc hành động không hợp lệ ở trạng thái hiện tại) • 422 Unprocessable Entity (đúng định dạng nhưng sai business rule, ví dụ endDate < startDate) • 429 Too Many Requests (rate limit) • 500 Internal Server Error.

Truy vết log (Correlation-Id)

Mọi response đều có header X-Correlation-Id (do middleware logging của Thành viên 5 sinh ra) để tra cứu log tập trung theo traceId khi hỗ trợ seller (đúng yêu cầu #11 của đề bài).

II. Đặc tả nghiệp vụ & API chi tiết theo từng module (7 thành viên)

Mỗi module dưới đây trình bày theo cùng 1 khuôn: (1) Tổng quan nghiệp vụ đối chiếu eBay thật, (2) Đặc tả từng tính năng --- actor, luồng chính, quy tắc/ràng buộc, (3) API Spec đầy đủ input/output theo template chuẩn ở Mục I.

Module 1 --- Thành viên 1 (Leader) {#module-1--thành-viên-1-leader}

Auth, Xác minh Seller & Store Profile

Bảng dữ liệu phụ trách: User, Store

Tổng quan nghiệp vụ (đối chiếu eBay thực tế)

Đây là cổng vào của toàn hệ thống, tương ứng với luồng đăng ký tài khoản → nâng cấp lên Seller → xác minh danh tính → thiết lập gian hàng của eBay thật. Theo quy trình thực tế của eBay, một tài khoản phải hoàn tất 3 lớp xác minh (định danh, thông tin thuế, tài khoản ngân hàng nhận tiền) trước khi được phép đăng bán; trong phạm vi đồ án 6 ngày, nhóm mô phỏng lại tinh thần đó bằng cờ verificationStatus (Pending/Approved/Rejected) trên bảng Store và quy trình duyệt thủ công/giả lập, thay vì tích hợp KYC thật.

Đặc tả chi tiết theo từng tính năng

F1.1. Đăng ký / Đăng nhập / Làm mới phiên đăng nhập {#f11-đăng-ký--đăng-nhập--làm-mới-phiên-đăng-nhập}

Actor: Khách vãng lai (chưa có tài khoản), User đã đăng ký

Luồng nghiệp vụ chính:

Người dùng nhập email, mật khẩu, username để đăng ký → hệ thống kiểm tra email chưa tồn tại, hash mật khẩu (bcrypt/argon2), tạo User với role mặc định = Buyer.

Đăng nhập bằng email/mật khẩu → hệ thống cấp access token (JWT, hạn ngắn ~15--30 phút) và refresh token (hạn dài ~7 ngày, lưu ở DB hoặc Redis để có thể thu hồi).

Khi access token hết hạn, FE gọi /auth/refresh-token bằng refresh token để lấy access token mới mà không cần đăng nhập lại (Axios interceptor dùng chung cho cả 7 module).

Đăng xuất sẽ vô hiệu hoá refresh token hiện tại phía server.

Quy tắc nghiệp vụ / ràng buộc:

Mật khẩu tối thiểu 8 ký tự, không lưu plaintext.

Email là duy nhất (UNIQUE theo schema User.email).

role trong JWT chỉ có 3 giá trị: Buyer, Seller, Admin --- khớp cột User.role.

Login sai quá 5 lần trong 15 phút cho cùng 1 email → tạm khoá 15 phút (phối hợp cùng rate limiter Redis của Thành viên 2 và reCAPTCHA của Thành viên 3 ở form đăng nhập rủi ro cao).

F1.2. Đăng ký trở thành Seller (Seller Apply) {#f12-đăng-ký-trở-thành-seller-seller-apply}

Actor: User đã đăng nhập (role hiện tại = Buyer)

Luồng nghiệp vụ chính:

User điền form "Trở thành người bán": loại seller (Individual/Business), họ tên hoặc tên doanh nghiệp, số điện thoại liên hệ, tên gian hàng dự kiến.

Hệ thống tạo bản ghi Store mới gắn sellerId = userId, verificationStatus = 'Pending', role của User CHƯA đổi (vẫn là Buyer cho tới khi được duyệt) --- đúng tinh thần eBay: nộp hồ sơ xong vẫn phải chờ duyệt mới bán được.

Không cho phép nộp hồ sơ lần 2 nếu đã có Store ở trạng thái Pending hoặc Approved.

Quy tắc nghiệp vụ / ràng buộc:

Chặn spam bằng reCAPTCHA v3 (form rủi ro cao theo mục 1.3.b kế hoạch gốc).

verificationStatus mặc định 'Pending'.

F1.3. Duyệt hồ sơ Seller (mô phỏng KYC) {#f13-duyệt-hồ-sơ-seller-mô-phỏng-kyc}

Actor: Admin (hoặc job tự động giả lập auto-approve theo kế hoạch Ngày 3)

Luồng nghiệp vụ chính:

Admin xem danh sách Store đang Pending, kiểm tra thông tin đã nộp.

Duyệt (Approved): verificationStatus = 'Approved', role của User cập nhật thành 'Seller', JWT lần đăng nhập kế tiếp mang theo verified = true.

Từ chối (Rejected): verificationStatus = 'Rejected', ghi lại rejectionReason để Seller biết cần bổ sung gì và có thể nộp lại hồ sơ.

Quy tắc nghiệp vụ / ràng buộc:

Chỉ Admin mới gọi được API duyệt/từ chối.

SellerVerifiedGuard (middleware dùng chung cho các module khác) chỉ cho qua khi role = Seller và verified = true --- mọi API tạo Product/Coupon/... đều phải đi qua guard này.

F1.4. Thiết lập hồ sơ gian hàng (Store Profile) {#f14-thiết-lập-hồ-sơ-gian-hàng-store-profile}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Seller chỉnh sửa storeName, description, upload bannerImageURL.

Buyer/khách vãng lai xem được trang Store công khai (GET /api/store/{sellerId}) để biết uy tín gian hàng trước khi mua --- đúng tinh thần trang "Seller store" của eBay.

Quy tắc nghiệp vụ / ràng buộc:

Chỉ chủ Store (sellerId khớp userId trong JWT) mới sửa được Store của mình.

Ảnh banner giới hạn dung lượng/định dạng (ví dụ ≤ 5MB, jpg/png).

API Spec

POST /api/auth/register

Tạo tài khoản User mới với role mặc định Buyer.

Xác thực: Không cần token (public)

Request Body:

Trường   Kiểu         Bắt buộc   Mô tả

username     string           ✔              3--100 ký tự, không trùng khoảng trắng đầu/cuối
email        string (email)   ✔              Duy nhất trong hệ thống
password     string           ✔              Tối thiểu 8 ký tự

Response --- HTTP 201 Created:

Trường      Kiểu   Mô tả

data.id         number     Id User vừa tạo
data.username   string
data.email      string
data.role       string     Luôn là 'Buyer' khi mới đăng ký

Ví dụ Request:

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p>{</p>
<p>"username": "hieupt",</p>
<p>"email": "hieupt@example.com",</p>
<p>"password": "Passw0rd!"</p>
<p>}</p></td>
</tr>
</tbody>
</table>

Ví dụ Response:

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p>{</p>
<p>"success": true,</p>
<p>"data": { "id": 12, "username": "hieupt", "email": "hieupt@example.com", "role": "Buyer" }</p>
<p>}</p></td>
</tr>
</tbody>
</table>

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

400               VALIDATION_ERROR          Thiếu trường hoặc sai định dạng email/password
409               EMAIL_ALREADY_EXISTS      Email đã được đăng ký trước đó

POST /api/auth/login

Đăng nhập, trả về access token + refresh token.

Xác thực: Không cần token (public)

Request Body:

Trường       Kiểu   Bắt buộc   Mô tả

email            string     ✔
password         string     ✔
recaptchaToken   string     ✔              Token reCAPTCHA v3 sinh phía FE

Response --- HTTP 200 OK:

Trường          Kiểu       Mô tả

data.accessToken    string (JWT)   Hạn ~15--30 phút
data.refreshToken   string         Hạn ~7 ngày
data.user           object         { id, username, role, storeId, verified }

Ví dụ Request:

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p>{</p>
<p>"email": "seller01@example.com",</p>
<p>"password": "Passw0rd!",</p>
<p>"recaptchaToken": "03AGdBq27..."</p>
<p>}</p></td>
</tr>
</tbody>
</table>

Ví dụ Response:

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p>{</p>
<p>"success": true,</p>
<p>"data": {</p>
<p>"accessToken": "eyJhbGciOi...",</p>
<p>"refreshToken": "b7f1c2...",</p>
<p>"user": { "id": 5, "username": "seller01", "role": "Seller", "storeId": 3, "verified": true }</p>
<p>}</p>
<p>}</p></td>
</tr>
</tbody>
</table>

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

401               INVALID_CREDENTIALS       Sai email hoặc mật khẩu
403               RECAPTCHA_FAILED          Điểm reCAPTCHA < 0.5 và chưa xác thực bổ sung
429               TOO_MANY_ATTEMPTS         Đăng nhập sai quá 5 lần/15 phút

POST /api/auth/refresh-token

Cấp access token mới khi access token cũ hết hạn.

Xác thực: Không cần Access token, cần refreshToken hợp lệ trong body/cookie

Request Body:

Trường     Kiểu   Bắt buộc   Mô tả

refreshToken   string     ✔

Response --- HTTP 200 OK:

Trường         Kiểu   Mô tả

data.accessToken   string

Ví dụ Request:

{ "refreshToken": "b7f1c2..." }

Ví dụ Response:

{ "success": true, "data": { "accessToken": "eyJhbGciOi..." } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

401               REFRESH_TOKEN_INVALID     Refresh token sai/hết hạn/đã bị thu hồi

POST /api/auth/logout

Thu hồi refresh token hiện tại.

Xác thực: Bearer token

Request Body:

Trường     Kiểu   Bắt buộc   Mô tả

refreshToken   string     ✔

Response --- HTTP 204 No Content:

(Không có body)

Ví dụ Request:

{ "refreshToken": "b7f1c2..." }

Ví dụ Response:

(không có body)

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

401               UNAUTHORIZED              Token không hợp lệ

POST /api/seller/apply

Nộp hồ sơ đăng ký trở thành Seller, tạo Store trạng thái Pending.

Xác thực: Bearer token --- role Buyer hoặc Seller (Rejected) trở lên

Request Body:

Trường   Kiểu                           Bắt buộc   Mô tả

sellerType   string enum(Individual,Business)   ✔
legalName    string                             ✔              Họ tên hoặc tên doanh nghiệp
phone        string                             ✔
storeName    string                             ✔              Tên gian hàng dự kiến

Response --- HTTP 201 Created:

Trường                Kiểu   Mô tả

data.storeId              number
data.verificationStatus   string     Luôn 'Pending' khi mới nộp

Ví dụ Request:

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p>{</p>
<p>"sellerType": "Individual",</p>
<p>"legalName": "Pham Trung Hieu",</p>
<p>"phone": "0901234567",</p>
<p>"storeName": "Hieu's Gadget Store"</p>
<p>}</p></td>
</tr>
</tbody>
</table>

Ví dụ Response:

{ "success": true, "data": { "storeId": 8, "verificationStatus": "Pending" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)       Khi nào xảy ra

409               APPLICATION_ALREADY_PENDING   User đã có hồ sơ đang chờ duyệt hoặc đã Approved
422               INVALID_SELLER_TYPE           sellerType không thuộc Individual/Business

GET /api/seller/verification-status

Seller tự tra cứu trạng thái hồ sơ của mình.

Xác thực: Bearer token

Response --- HTTP 200 OK:

Trường                Kiểu       Mô tả

data.verificationStatus   string         Pending | Approved | Rejected
data.rejectionReason      string|null   Chỉ có khi Rejected

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "verificationStatus": "Rejected", "rejectionReason": "Số điện thoại không hợp lệ" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               STORE_NOT_FOUND           User chưa từng nộp hồ sơ

PUT /api/admin/seller/{id}/approve

Duyệt hồ sơ Seller: verificationStatus → Approved, role User → Seller.

Xác thực: Bearer token --- role Admin

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

id           number     ✔              storeId cần duyệt

Response --- HTTP 200 OK:

Trường                Kiểu   Mô tả

data.verificationStatus   string     'Approved'

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "storeId": 8, "verificationStatus": "Approved" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               STORE_NOT_FOUND
409               ALREADY_PROCESSED         Store không còn ở trạng thái Pending

PUT /api/admin/seller/{id}/reject

Từ chối hồ sơ Seller kèm lý do.

Xác thực: Bearer token --- role Admin

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

id           number     ✔              storeId cần từ chối

Request Body:

Trường   Kiểu   Bắt buộc   Mô tả

reason       string     ✔              Lý do từ chối, hiển thị lại cho Seller

Response --- HTTP 200 OK:

Trường                Kiểu   Mô tả

data.verificationStatus   string     'Rejected'

Ví dụ Request:

{ "reason": "Thông tin liên hệ chưa đầy đủ" }

Ví dụ Response:

{ "success": true, "data": { "storeId": 8, "verificationStatus": "Rejected" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               STORE_NOT_FOUND

GET /api/store/{sellerId}

Xem hồ sơ công khai của một gian hàng.

Xác thực: Không cần token (public --- buyer xem trang gian hàng)

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

sellerId     number     ✔

Response --- HTTP 200 OK:

Trường                Kiểu   Mô tả

data.storeName            string
data.description          string
data.bannerImageURL       string
data.verificationStatus   string

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "storeName": "Hieu's Gadget Store", "description": "...", "bannerImageURL": "https://...", "verificationStatus": "Approved" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               STORE_NOT_FOUND

PUT /api/store

Seller cập nhật hồ sơ gian hàng của chính mình.

Xác thực: Bearer token --- [Seller][Verified]

Request Body:

Trường       Kiểu   Bắt buộc   Mô tả

storeName        string     -
description      string     -
bannerImageURL   string     -             URL sau khi upload ảnh

Response --- HTTP 200 OK:

Trường   Kiểu       Mô tả

data         Store object   Bản ghi sau khi cập nhật

Ví dụ Request:

{ "storeName": "Hieu's Gadget Store 2.0", "description": "Chuyên phụ kiện công nghệ chính hãng" }

Ví dụ Response:

{ "success": true, "data": { "storeId": 8, "storeName": "Hieu's Gadget Store 2.0", "description": "...", "bannerImageURL": "https://..." } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

403               SELLER_NOT_VERIFIED       Seller chưa được duyệt (verified=false)

Module 2 --- Thành viên 2 {#module-2--thành-viên-2}

Đăng bán, CRUD Sản phẩm & Quản lý Tồn kho

Bảng dữ liệu phụ trách: Product, Inventory, Category (đọc), Bid (bổ sung --- xem ghi chú hiệu chỉnh)

Tổng quan nghiệp vụ (đối chiếu eBay thực tế)

Đây là module lõi vì gần như mọi module khác (Coupon, Order, Review, Dashboard) đều tham chiếu tới Product --- đúng vai trò 'listing' trung tâm trên eBay thật. Theo đúng schema, Product hỗ trợ song song 2 hình thức bán: Fixed price (isAuction=false) và Auction (isAuction=true, có auctionEndTime và bảng Bid) --- vì vậy đặc tả dưới đây bổ sung tối thiểu luồng đấu giá để không bỏ phí phần schema đã có sẵn.

Đặc tả chi tiết theo từng tính năng

F2.1. Đăng bán sản phẩm (Create Listing) {#f21-đăng-bán-sản-phẩm-create-listing}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Form nhiều bước giống eBay: (1) chọn Category, (2) nhập title/description/item condition, (3) upload nhiều ảnh, (4) chọn hình thức bán Fixed price (nhập price) hoặc Auction (nhập giá khởi điểm price + auctionEndTime), (5) nhập số lượng ban đầu.

Khi submit: hệ thống tạo Product gắn sellerId lấy từ JWT (không cho FE tự truyền sellerId), đồng thời khởi tạo Inventory tương ứng với quantity đã nhập.

Ảnh được upload lên storage (Cloudinary/S3 hoặc thư mục /uploads tĩnh), Product.images lưu danh sách URL dạng JSON string.

Quy tắc nghiệp vụ / ràng buộc:

Chỉ Seller đã Verified mới gọi được (SellerVerifiedGuard).

Nếu isAuction=true bắt buộc phải có auctionEndTime > thời điểm hiện tại.

Rate limit: tối đa 20 sản phẩm/giờ/seller (đếm bằng Redis) để chống đăng hàng loạt spam.

price > 0; ít nhất 1 ảnh.

F2.2. Danh sách & tìm kiếm sản phẩm của seller {#f22-danh-sách--tìm-kiếm-sản-phẩm-của-seller}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Trang 'Quản lý sản phẩm' hiển thị bảng có phân trang, filter theo category/status (Active/Hidden/Out of stock), tìm kiếm theo keyword trong title.

Quy tắc nghiệp vụ / ràng buộc:

Seller chỉ xem được sản phẩm có sellerId = chính mình (trừ khi gọi GET /api/products công khai để buyer duyệt hàng, không lọc theo sellerId).

F2.3. Cập nhật / Ẩn / Xoá sản phẩm {#f23-cập-nhật--ẩn--xoá-sản-phẩm}

Actor: Seller đã Verified (chủ sở hữu sản phẩm)

Luồng nghiệp vụ chính:

Sửa thông tin sản phẩm (PUT) --- không cho sửa isAuction sau khi đã có Bid đầu tiên, để tránh đổi luật chơi giữa phiên đấu giá (đúng nguyên tắc công bằng của eBay).

Ẩn tạm thời (PATCH .../hide) khi hết hàng hoặc seller muốn ngừng bán mà không mất dữ liệu lịch sử đơn hàng liên quan.

Xoá hẳn (DELETE) chỉ cho phép khi sản phẩm chưa từng phát sinh OrderItem nào, để không phá vỡ tính toàn vẹn báo cáo doanh thu.

Quy tắc nghiệp vụ / ràng buộc:

Chặn sửa/xoá sản phẩm không thuộc sở hữu (403).

DELETE trả 409 nếu đã có OrderItem tham chiếu --- khi đó chỉ nên PATCH hide.

F2.4. Quản lý tồn kho (Inventory) {#f24-quản-lý-tồn-kho-inventory}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Xem/tăng giảm quantity thủ công khi nhập thêm hàng.

Khi có đơn hàng mới được xác nhận (module 4), hệ thống tự trừ Inventory.quantity tương ứng (gọi nội bộ, không phải API public).

Khi quantity = 0, sản phẩm tự động hiển thị trạng thái 'Hết hàng' (Out of stock) trên trang buyer nhưng KHÔNG tự xoá/ẩn --- seller vẫn cần chủ động ẩn nếu muốn ngừng bán hẳn.

Quy tắc nghiệp vụ / ràng buộc:

quantity không được âm --- mọi thao tác trừ kho phải kiểm tra đủ hàng trước khi trừ (idempotent, tránh oversell khi nhiều buyer mua cùng lúc).

F2.5 (bổ sung). Đấu giá (Auction) --- theo dõi Bid & tự động đóng phiên {#f25-bổ-sung-đấu-giá-auction--theo-dõi-bid--tự-động-đóng-phiên}

Actor: Seller (xem), Buyer/Bidder (đặt giá --- thuộc phạm vi module Buyer, không phải Seller), Hệ thống (background job)

Luồng nghiệp vụ chính:

Với Product có isAuction=true, seller theo dõi danh sách Bid hiện tại (giá cao nhất, số lượt đặt) trên trang chi tiết sản phẩm --- việc buyer đặt Bid không thuộc phạm vi phân hệ Seller nên không đặc tả API tạo Bid ở đây, chỉ đặc tả API đọc phục vụ seller.

Background job (cron) quét các Product có auctionEndTime <= now và chưa đóng phiên: xác định Bid có amount cao nhất là người thắng, tự động tạo OrderTable + OrderItem cho người thắng (trạng thái chờ thanh toán), cập nhật Product sang trạng thái đã kết thúc đấu giá.

Nếu không có Bid nào khi hết giờ, sản phẩm tự chuyển về trạng thái không bán được nữa (tương tự tin đấu giá hết hạn không ai đặt giá trên eBay).

Quy tắc nghiệp vụ / ràng buộc:

Chỉ 1 lần đóng phiên cho mỗi Product (idempotent theo productId, tránh job chạy trùng tạo 2 đơn).

Giá thắng cuộc = MAX(Bid.amount) theo productId, nếu bằng nhau lấy Bid có bidTime sớm nhất.

API Spec

POST /api/products

Tạo sản phẩm mới kèm upload nhiều ảnh, đồng thời khởi tạo Inventory.

Xác thực: Bearer token --- [Seller][Verified] (multipart/form-data)

Request Body:

Trường       Kiểu   Bắt buộc            Mô tả

title            string     ✔                       ≤ 255 ký tự
description      string     ✔
price            decimal    ✔                       > 0
categoryId       number     ✔
isAuction        boolean    ✔
auctionEndTime   datetime   Có nếu isAuction=true   Phải > hiện tại
quantity         number     ✔                       Số lượng tồn kho ban đầu, ≥ 0
images           file[]   ✔                       Tối thiểu 1 ảnh, multipart

Response --- HTTP 201 Created:

Trường                Kiểu     Mô tả

data.id                   number
data.images               string[]   Danh sách URL ảnh sau khi upload
data.inventory.quantity   number

Ví dụ Request:

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p>multipart/form-data:</p>
<p>title=iPhone 13 Pro Max 256GB</p>
<p>description=Máy đẹp, fullbox</p>
<p>price=15000000</p>
<p>categoryId=4</p>
<p>isAuction=false</p>
<p>quantity=10</p>
<p>images=[file1.jpg, file2.jpg]</p></td>
</tr>
</tbody>
</table>

Ví dụ Response:

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p>{</p>
<p>"success": true,</p>
<p>"data": { "id": 101, "title": "iPhone 13 Pro Max 256GB", "sellerId": 5,</p>
<p>"images": ["https://cdn/.../file1.jpg", "https://cdn/.../file2.jpg"],</p>
<p>"inventory": { "quantity": 10 } }</p>
<p>}</p></td>
</tr>
</tbody>
</table>

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)     Khi nào xảy ra

422               AUCTION_END_TIME_REQUIRED   isAuction=true nhưng thiếu/quá khứ auctionEndTime
429               PRODUCT_CREATE_RATE_LIMIT   Vượt 20 sản phẩm/giờ

GET /api/products

Danh sách sản phẩm, hỗ trợ phân trang/filter/tìm kiếm.

Xác thực: Public (buyer duyệt hàng) --- thêm sellerId= để Seller xem hàng của mình

Query Params:

Trường                          Kiểu   Bắt buộc   Mô tả

sellerId                            number     -             Lọc theo seller
status                              string     -             Active|Hidden|OutOfStock
keyword                             string     -             Tìm trong title
page, pageSize, sortBy, sortOrder   -         -             Theo quy ước chung

Response --- HTTP 200 OK:

Trường        Kiểu      Mô tả

data.items[]    Product[]
data.pagination   object

Ví dụ Request:

GET /api/products?sellerId=5&status=Active&page=1&pageSize=20

Ví dụ Response:

{ "success": true, "data": { "items": [ { "id": 101, "title": "iPhone 13 Pro Max 256GB", "price": 15000000, "isAuction": false } ], "pagination": { "page": 1, "pageSize": 20, "totalItems": 34, "totalPages": 2 } } }

GET /api/products/{id}

Chi tiết 1 sản phẩm.

Xác thực: Public

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

id           number     ✔

Response --- HTTP 200 OK:

Trường   Kiểu                                  Mô tả

data         Product object (kèm inventory.quantity)

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "id": 101, "title": "iPhone 13 Pro Max 256GB", "price": 15000000, "images": ["..."], "inventory": { "quantity": 10 }, "isAuction": false } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               PRODUCT_NOT_FOUND

PUT /api/products/{id}

Cập nhật thông tin sản phẩm.

Xác thực: Bearer token --- [Seller][Verified] (chủ sở hữu)

Request Body:

Trường                              Kiểu   Bắt buộc   Mô tả

title, description, price, categoryId   -         tuỳ chọn       Không cho sửa isAuction nếu đã có Bid

Response --- HTTP 200 OK:

Trường   Kiểu                      Mô tả

data         Product object sau cập nhật

Ví dụ Request:

{ "price": 14500000, "description": "Giảm giá cuối tuần" }

Ví dụ Response:

{ "success": true, "data": { "id": 101, "price": 14500000 } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

403               FORBIDDEN_NOT_OWNER
409               AUCTION_ALREADY_HAS_BID   Cố sửa isAuction khi đã có Bid

PATCH /api/products/{id}/hide

Ẩn tạm sản phẩm khỏi trang buyer.

Xác thực: Bearer token --- [Seller][Verified] (chủ sở hữu)

Response --- HTTP 200 OK:

Trường    Kiểu   Mô tả

data.status   string     'Hidden'

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "id": 101, "status": "Hidden" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

403               FORBIDDEN_NOT_OWNER

DELETE /api/products/{id}

Xoá hẳn sản phẩm --- chỉ khi chưa từng có đơn hàng.

Xác thực: Bearer token --- [Seller][Verified] (chủ sở hữu)

Response --- HTTP 204 No Content:

(Không có body)

Ví dụ Request:

(không có body)

Ví dụ Response:

(không có body)

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

409               PRODUCT_HAS_ORDERS        Đã tồn tại OrderItem tham chiếu sản phẩm --- nên dùng hide thay vì delete

GET /api/inventory/{productId}

Xem tồn kho hiện tại.

Xác thực: Bearer token --- [Seller][Verified] (chủ sở hữu)

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

productId    number     ✔

Response --- HTTP 200 OK:

Trường         Kiểu   Mô tả

data.quantity      number
data.lastUpdated   datetime

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "productId": 101, "quantity": 8, "lastUpdated": "2026-08-19T10:00:00Z" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               INVENTORY_NOT_FOUND

PUT /api/inventory/{productId}

Cập nhật số lượng tồn kho thủ công.

Xác thực: Bearer token --- [Seller][Verified] (chủ sở hữu)

Request Body:

Trường   Kiểu   Bắt buộc   Mô tả

quantity     number     ✔              ≥ 0

Response --- HTTP 200 OK:

Trường      Kiểu   Mô tả

data.quantity   number

Ví dụ Request:

{ "quantity": 25 }

Ví dụ Response:

{ "success": true, "data": { "productId": 101, "quantity": 25, "lastUpdated": "2026-08-20T09:00:00Z" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

422               QUANTITY_NEGATIVE         quantity < 0

GET /api/products/{id}/bids

Seller xem danh sách lượt đặt giá của phiên đấu giá.

Xác thực: Bearer token --- [Seller][Verified] (chủ sở hữu) --- bổ sung

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

id           number     ✔              productId, phải có isAuction=true

Response --- HTTP 200 OK:

Trường        Kiểu   Mô tả

data.items[]    Bid[]    Sắp xếp amount giảm dần
data.highestBid   decimal

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "items": [ { "bidderId": 20, "amount": 5200000, "bidTime": "2026-08-19T08:00:00Z" } ], "highestBid": 5200000 } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

422               NOT_AN_AUCTION_PRODUCT    isAuction=false

POST /api/products/{id}/close-auction

Job tự động chạy khi auctionEndTime đã qua: xác định người thắng, tạo OrderTable/OrderItem chờ thanh toán.

Xác thực: Nội bộ (system/cron, không public --- bổ sung)

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

id           number     ✔

Response --- HTTP 200 OK:

Trường      Kiểu       Mô tả

data.winnerId   number|null
data.orderId    number|null

Ví dụ Request:

(chạy tự động, không có body)

Ví dụ Response:

{ "success": true, "data": { "winnerId": 20, "orderId": 555 } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

409               AUCTION_ALREADY_CLOSED    Job chạy trùng cho cùng productId

Module 3 --- Thành viên 3 {#module-3--thành-viên-3}

Marketing & Mã giảm giá (Coupon)

Bảng dữ liệu phụ trách: Coupon

Tổng quan nghiệp vụ (đối chiếu eBay thực tế)

Tương ứng với Order-level coupon code trong eBay Promotions Manager: seller tạo 1 mã giảm giá gắn với đúng 1 sản phẩm, có hiệu lực theo ngày và giới hạn số lượt dùng; buyer phải chủ động nhập đúng mã lúc đặt hàng thì mới được chiết khấu --- giá niêm yết trên trang sản phẩm không đổi.

Đặc tả chi tiết theo từng tính năng

F3.1. Tạo mã giảm giá {#f31-tạo-mã-giảm-giá}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Chọn 1 sản phẩm của mình, nhập code (duy nhất), discountPercent, startDate/endDate, maxUsage.

Form gắn Google reCAPTCHA v3 vì đây là chức năng dễ bị lạm dụng để spam mã.

Quy tắc nghiệp vụ / ràng buộc:

code duy nhất trong toàn hệ thống (không phân biệt hoa/thường khi so khớp).

0 < discountPercent ≤ 100.

endDate > startDate; maxUsage > 0.

productId phải thuộc chính seller đang tạo coupon.

F3.2. Quản lý danh sách Coupon {#f32-quản-lý-danh-sách-coupon}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Bảng danh sách coupon, filter theo sản phẩm/trạng thái hiệu lực (Active/Expired/Hết lượt --- tính từ startDate/endDate/maxUsage so với thời gian hiện tại và số lượt đã dùng).

Quy tắc nghiệp vụ / ràng buộc:

Trạng thái là tính toán (derived), không lưu cứng trong DB để tránh lệch dữ liệu.

F3.3. Xác thực mã giảm giá lúc đặt hàng (Validate) {#f33-xác-thực-mã-giảm-giá-lúc-đặt-hàng-validate}

Actor: Buyer (gọi gián tiếp qua module 4 lúc tạo đơn), Hệ thống

Luồng nghiệp vụ chính:

Module 4 (Order) gọi API này khi buyer nhập mã ở bước checkout, trước khi chốt OrderItem.unitPrice.

Nếu hợp lệ trả về discountPercent để module 4 tự tính giá sau giảm; nếu không hợp lệ trả lỗi rõ lý do (hết hạn/hết lượt/không đúng sản phẩm).

Quy tắc nghiệp vụ / ràng buộc:

Kiểm tra đủ 4 điều kiện: code tồn tại, đúng productId, trong khoảng startDate--endDate, số lượt đã dùng < maxUsage.

F3.4. Thống kê hiệu quả Coupon {#f34-thống-kê-hiệu-quả-coupon}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Xem số lượt đã dùng/maxUsage của từng coupon để đánh giá hiệu quả khuyến mãi.

Quy tắc nghiệp vụ / ràng buộc:

Số lượt đã dùng đếm bằng số OrderItem hợp lệ đã áp đúng coupon đó (đếm qua Order module, không lưu counter riêng để tránh lệch khi có huỷ đơn).

API Spec

POST /api/coupons

Tạo mã giảm giá cho 1 sản phẩm.

Xác thực: Bearer token --- [Seller][Verified]

Request Body:

Trường        Kiểu   Bắt buộc   Mô tả

productId         number     ✔              Phải thuộc seller hiện tại
code              string     ✔              Duy nhất, 4--50 ký tự
discountPercent   decimal    ✔              0 < x ≤ 100
startDate         date       ✔
endDate           date       ✔              > startDate
maxUsage          number     ✔              > 0

Response --- HTTP 201 Created:

Trường   Kiểu        Mô tả

data         Coupon object

Ví dụ Request:

{ "productId": 101, "code": "SALE20AUG", "discountPercent": 15, "startDate": "2026-08-20", "endDate": "2026-08-31", "maxUsage": 50 }

Ví dụ Response:

{ "success": true, "data": { "id": 9, "productId": 101, "code": "SALE20AUG", "discountPercent": 15, "startDate": "2026-08-20", "endDate": "2026-08-31", "maxUsage": 50 } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

409               COUPON_CODE_EXISTS
422               INVALID_DATE_RANGE        endDate ≤ startDate
403               PRODUCT_NOT_OWNED         productId không thuộc seller

GET /api/coupons

Danh sách coupon theo seller, filter theo sản phẩm/trạng thái.

Xác thực: Bearer token --- [Seller][Verified]

Query Params:

Trường                                    Kiểu   Bắt buộc   Mô tả

sellerId, productId, status, page, pageSize   -         -             status: Active|Expired|UsedUp

Response --- HTTP 200 OK:

Trường       Kiểu     Mô tả

data.items[]   Coupon[]

Ví dụ Request:

GET /api/coupons?sellerId=5&status=Active

Ví dụ Response:

{ "success": true, "data": { "items": [ { "id": 9, "code": "SALE20AUG", "discountPercent": 15, "usedCount": 12, "maxUsage": 50 } ], "pagination": { "page": 1, "pageSize": 20, "totalItems": 1, "totalPages": 1 } } }

PUT /api/coupons/{id}

Sửa thông tin coupon (chưa có lượt dùng nào).

Xác thực: Bearer token --- [Seller][Verified] (chủ sở hữu)

Request Body:

Trường                                      Kiểu   Bắt buộc   Mô tả

discountPercent, startDate, endDate, maxUsage   -         tuỳ chọn

Response --- HTTP 200 OK:

Trường   Kiểu        Mô tả

data         Coupon object

Ví dụ Request:

{ "maxUsage": 100 }

Ví dụ Response:

{ "success": true, "data": { "id": 9, "maxUsage": 100 } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

409               COUPON_ALREADY_USED       Đã có lượt dùng, không cho sửa discountPercent để tránh bất công giữa các buyer

DELETE /api/coupons/{id}

Xoá/huỷ coupon chưa dùng.

Xác thực: Bearer token --- [Seller][Verified] (chủ sở hữu)

Response --- HTTP 204 No Content:

(Không có body)

Ví dụ Request:

(không có body)

Ví dụ Response:

(không có body)

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

409               COUPON_ALREADY_USED

GET /api/coupons/validate

Kiểm tra mã giảm giá hợp lệ cho 1 sản phẩm trước khi tạo đơn.

Xác thực: Bearer token (buyer) --- dùng nội bộ bởi Module 4

Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

code         string     ✔
productId    number     ✔

Response --- HTTP 200 OK:

Trường             Kiểu   Mô tả

data.valid             boolean
data.discountPercent   decimal    Chỉ có khi valid=true

Ví dụ Request:

GET /api/coupons/validate?code=SALE20AUG&productId=101

Ví dụ Response:

{ "success": true, "data": { "valid": true, "discountPercent": 15 } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)     Khi nào xảy ra

404               COUPON_NOT_FOUND            Mã không tồn tại hoặc sai productId
422               COUPON_EXPIRED_OR_USED_UP   Ngoài hiệu lực hoặc hết lượt

GET /api/coupons/{id}/usage-stats

Thống kê số lượt đã dùng / maxUsage của 1 coupon.

Xác thực: Bearer token --- [Seller][Verified] (chủ sở hữu) --- bổ sung

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

id           number     ✔

Response --- HTTP 200 OK:

Trường       Kiểu   Mô tả

data.usedCount   number
data.maxUsage    number
data.usageRate   decimal    usedCount/maxUsage

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "usedCount": 12, "maxUsage": 50, "usageRate": 0.24 } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               COUPON_NOT_FOUND

Module 4 --- Thành viên 4 {#module-4--thành-viên-4}

Quản lý Đơn hàng & Phiếu vận chuyển

Bảng dữ liệu phụ trách: OrderTable, OrderItem, Payment (đọc), ShippingInfo

Tổng quan nghiệp vụ (đối chiếu eBay thực tế)

Mô phỏng luồng fulfillment của eBay: buyer đặt hàng (có thể áp coupon), seller xác nhận đơn, cập nhật trạng thái giao hàng, in phiếu vận chuyển. Theo đúng nghiệp vụ thật, 'tạo nhãn vận chuyển' và 'đã bàn giao cho đơn vị vận chuyển' là 2 bước khác nhau --- đặc tả dưới đây tách rõ để tránh tình trạng đơn bị đánh dấu 'đã giao vận chuyển' trước khi seller thực sự gửi hàng.

Đặc tả chi tiết theo từng tính năng

F4.1. Danh sách đơn hàng của Seller {#f41-danh-sách-đơn-hàng-của-seller}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Vì 1 đơn (OrderTable) có thể chứa sản phẩm của nhiều seller khác nhau (giống giỏ hàng chung của eBay), API lọc theo sellerId dựa trên join OrderItem → Product để chỉ trả về đơn có ít nhất 1 sản phẩm của seller đó, kèm chỉ những OrderItem thuộc seller này.

Quy tắc nghiệp vụ / ràng buộc:

Không trả về OrderItem của seller khác trong cùng 1 đơn --- seller chỉ thấy phần hàng của mình.

F4.2. Xác nhận đơn hàng {#f42-xác-nhận-đơn-hàng}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Sau khi buyer thanh toán (Payment.status = Completed), seller xác nhận đã nhận đơn và bắt đầu chuẩn bị hàng --- trạng thái OrderTable chuyển Pending → Confirmed.

Đồng thời hệ thống trừ Inventory.quantity tương ứng cho từng OrderItem (liên kết ngược tới Module 2).

Quy tắc nghiệp vụ / ràng buộc:

Chỉ xác nhận được khi Payment đã Completed.

Không đủ tồn kho tại thời điểm xác nhận → trả lỗi để seller chủ động liên hệ buyer.

F4.3. Cập nhật trạng thái vận chuyển {#f43-cập-nhật-trạng-thái-vận-chuyển}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

State machine ShippingInfo.status: Preparing → LabelCreated → HandedToCarrier (Shipped) → InTransit → Delivered.

OrderTable.status chỉ chuyển sang 'Shipped' khi ShippingInfo có trackingNumber VÀ status = HandedToCarrier trở lên --- không tự động chuyển ngay khi mới tạo nhãn.

Quy tắc nghiệp vụ / ràng buộc:

Không cho nhảy cóc trạng thái ngược (ví dụ Delivered → Preparing).

F4.4. In phiếu vận chuyển (giả lập) {#f44-in-phiếu-vận-chuyển-giả-lập}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Sinh phiếu vận chuyển giả lập (HTML → PDF) kèm mã vận đơn (trackingNumber) tự sinh, carrier chọn từ danh sách giả lập (GHN/GHTK/Viettel Post...).

Đây là bước 'tạo nhãn' (LabelCreated) --- CHƯA đồng nghĩa đã giao hàng; seller phải bấm 'Xác nhận đã bàn giao' riêng để chuyển sang HandedToCarrier.

Quy tắc nghiệp vụ / ràng buộc:

Chỉ tạo phiếu khi OrderTable.status = Confirmed.

F4.5. Theo dõi trạng thái thanh toán (đọc) {#f45-theo-dõi-trạng-thái-thanh-toán-đọc}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Seller xem trạng thái Payment (Pending/Completed/Failed/Refunded) của đơn --- chỉ đọc, không sửa (Payment do buyer/cổng thanh toán quản lý, ngoài phạm vi module Seller).

Quy tắc nghiệp vụ / ràng buộc:

Read-only.

API Spec

GET /api/orders

Danh sách đơn hàng có chứa sản phẩm của seller.

Xác thực: Bearer token --- [Seller][Verified]

Query Params:

Trường                         Kiểu   Bắt buộc   Mô tả

sellerId, status, page, pageSize   -         -             status: Pending|Confirmed|Shipped|Delivered|Cancelled

Response --- HTTP 200 OK:

Trường       Kiểu           Mô tả

data.items[]   OrderSummary[]   { id, orderDate, totalPrice(chỉ phần của seller), status }

Ví dụ Request:

GET /api/orders?sellerId=5&status=Confirmed

Ví dụ Response:

{ "success": true, "data": { "items": [ { "id": 555, "orderDate": "2026-08-19T10:00:00Z", "totalPrice": 15000000, "status": "Confirmed" } ], "pagination": { "page": 1, "pageSize": 20, "totalItems": 1, "totalPages": 1 } } }

GET /api/orders/{id}

Chi tiết đơn hàng, chỉ gồm OrderItem của seller hiện tại.

Xác thực: Bearer token --- [Seller][Verified]

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

id           number     ✔

Response --- HTTP 200 OK:

Trường            Kiểu        Mô tả

data.orderItems[]   OrderItem[]   { productId, title, quantity, unitPrice }
data.address          object          Địa chỉ giao hàng
data.payment.status   string          read-only
data.shipping         object|null

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "id": 555, "status": "Confirmed", "orderItems": [ { "productId": 101, "title": "iPhone 13 Pro Max", "quantity": 1, "unitPrice": 14500000 } ], "payment": { "status": "Completed" } } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               ORDER_NOT_FOUND

PUT /api/orders/{id}/confirm

Xác nhận đơn hàng, trừ tồn kho.

Xác thực: Bearer token --- [Seller][Verified]

Response --- HTTP 200 OK:

Trường    Kiểu   Mô tả

data.status   string     'Confirmed'

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "id": 555, "status": "Confirmed" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

409               PAYMENT_NOT_COMPLETED     Payment chưa Completed
409               INSUFFICIENT_INVENTORY    Không đủ tồn kho

PUT /api/orders/{id}/status

Cập nhật trạng thái vận chuyển của đơn (theo state machine).

Xác thực: Bearer token --- [Seller][Verified]

Request Body:

Trường   Kiểu      Bắt buộc   Mô tả

status       string enum   ✔              HandedToCarrier|InTransit|Delivered|Cancelled

Response --- HTTP 200 OK:

Trường    Kiểu   Mô tả

data.status   string

Ví dụ Request:

{ "status": "HandedToCarrier" }

Ví dụ Response:

{ "success": true, "data": { "id": 555, "status": "Shipped" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)     Khi nào xảy ra

422               INVALID_STATUS_TRANSITION   Chuyển trạng thái không hợp lệ theo state machine

POST /api/orders/{id}/shipping-label

Sinh phiếu vận chuyển giả lập (PDF/HTML) kèm mã vận đơn --- chuyển ShippingInfo.status = LabelCreated.

Xác thực: Bearer token --- [Seller][Verified]

Request Body:

Trường   Kiểu   Bắt buộc   Mô tả

carrier      string     ✔              Tên đơn vị vận chuyển (giả lập)

Response --- HTTP 201 Created:

Trường            Kiểu   Mô tả

data.trackingNumber   string     Mã vận đơn tự sinh
data.labelUrl         string     Link xem trước/tải PDF

Ví dụ Request:

{ "carrier": "GHN" }

Ví dụ Response:

{ "success": true, "data": { "orderId": 555, "carrier": "GHN", "trackingNumber": "GHN-2026081900123", "status": "LabelCreated", "labelUrl": "https://.../labels/555.pdf" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

409               ORDER_NOT_CONFIRMED       OrderTable.status khác Confirmed

GET /api/shipping/{orderId}

Xem thông tin vận chuyển hiện tại của đơn.

Xác thực: Bearer token --- [Seller][Verified]

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

orderId      number     ✔

Response --- HTTP 200 OK:

Trường                                               Kiểu   Mô tả

data.carrier, trackingNumber, status, estimatedArrival   -

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "carrier": "GHN", "trackingNumber": "GHN-2026081900123", "status": "InTransit", "estimatedArrival": "2026-08-23T00:00:00Z" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               SHIPPING_INFO_NOT_FOUND

GET /api/orders/{id}/payment

Xem trạng thái thanh toán của đơn.

Xác thực: Bearer token --- [Seller][Verified] --- bổ sung, read-only

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

id           number     ✔

Response --- HTTP 200 OK:

Trường                            Kiểu   Mô tả

data.amount, method, status, paidAt   -

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "amount": 14500000, "method": "VNPay", "status": "Completed", "paidAt": "2026-08-19T09:50:00Z" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               PAYMENT_NOT_FOUND

Module 5 --- Thành viên 5 {#module-5--thành-viên-5}

Đánh giá, Phản hồi & Xử lý Khiếu nại

Bảng dữ liệu phụ trách: Review, Feedback, Dispute, ReturnRequest

Tổng quan nghiệp vụ (đối chiếu eBay thực tế)

Mô phỏng eBay Feedback system + Money Back Guarantee. Theo đúng nghiệp vụ thật, đây là 3 luồng có timeline và quy tắc khác nhau, không nên gộp chung 1 API xử lý: (1) Feedback --- seller chỉ reply đúng 1 lần, không sửa/xoá; (2) Return Request --- trong Money Back Guarantee, seller có 3 ngày làm việc phản hồi và 2 ngày hoàn tiền sau khi nhận hàng trả; (3) Dispute --- khiếu nại leo thang khi 2 bên không tự thoả thuận, do eBay/Admin can thiệp.

Đặc tả chi tiết theo từng tính năng

F5.1. Xem & phản hồi đánh giá (Review) {#f51-xem--phản-hồi-đánh-giá-review}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Xem danh sách Review theo sản phẩm/gian hàng.

Phản hồi công khai (Review.response) --- CHỈ được gửi 1 lần, sau khi đã có response thì API reply bị khoá.

Quy tắc nghiệp vụ / ràng buộc:

POST reply lần 2 khi đã có response → 409 Conflict (đúng luật eBay: 'reply once, không sửa/thu hồi').

F5.2. Tổng quan Feedback {#f52-tổng-quan-feedback}

Actor: Seller đã Verified, Buyer (xem công khai)

Luồng nghiệp vụ chính:

Feedback.averageRating/totalReviews/positiveRate được đồng bộ (tính lại) mỗi khi có Review mới --- hiển thị dạng biểu đồ sao trên trang Store, tương tự chỉ số uy tín người bán trên eBay.

Quy tắc nghiệp vụ / ràng buộc:

positiveRate = tỉ lệ Review có rating ≥ 4 trên tổng số Review.

F5.3. Xử lý yêu cầu đổi trả (Return Request --- Money Back Guarantee) {#f53-xử-lý-yêu-cầu-đổi-trả-return-request--money-back-guarantee}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Buyer gửi ReturnRequest kèm reason cho 1 đơn hàng đã Delivered.

Seller có tối đa 3 ngày làm việc để phản hồi: Accept (đồng ý nhận hàng trả), OfferRefund (hoàn tiền không cần trả hàng), hoặc Decline (chỉ hợp lệ khi lý do là 'đổi ý' và chính sách shop không nhận đổi trả trong trường hợp này).

Sau khi Accept và nhận được hàng trả, seller phải xác nhận hoàn tiền (Refunded) trong vòng 2 ngày làm việc.

Nếu quá 3 ngày seller không phản hồi hoặc 2 bên không thống nhất → buyer/seller có thể mở Dispute (F5.4) để hệ thống/Admin can thiệp.

Quy tắc nghiệp vụ / ràng buộc:

State machine: Requested → (Accepted | RefundOffered | Declined) → (RefundedByReturn) → Closed.

Không cho Decline nếu lý do trả hàng là 'hàng lỗi/không đúng mô tả' --- chỉ Decline được khi lý do là đổi ý và policy shop ghi rõ không nhận đổi trả trường hợp này.

F5.4. Xử lý khiếu nại (Dispute) {#f54-xử-lý-khiếu-nại-dispute}

Actor: Seller đã Verified, Admin

Luồng nghiệp vụ chính:

Dispute được tạo khi Return Request không đi tới thống nhất trong 3 ngày, hoặc buyer khiếu nại vấn đề khác (ví dụ chưa nhận được hàng).

Seller nộp description/bằng chứng phản hồi; Admin/hệ thống ra resolution cuối cùng trong vòng 48 giờ mô phỏng.

Quy tắc nghiệp vụ / ràng buộc:

Dispute.status: Open → UnderReview → Resolved; resolution là bắt buộc khi chuyển sang Resolved.

API Spec

GET /api/reviews

Danh sách đánh giá theo sản phẩm hoặc gian hàng.

Xác thực: Public (xem) --- Bearer token nếu cần lọc riêng cho seller

Query Params:

Trường                            Kiểu   Bắt buộc   Mô tả

productId, sellerId, page, pageSize   -         -

Response --- HTTP 200 OK:

Trường       Kiểu     Mô tả

data.items[]   Review[]   { rating, comment, response, createdAt }

Ví dụ Request:

GET /api/reviews?productId=101

Ví dụ Response:

{ "success": true, "data": { "items": [ { "id": 30, "rating": 5, "comment": "Đóng gói cẩn thận", "response": null } ], "pagination": { "page": 1, "pageSize": 20, "totalItems": 1, "totalPages": 1 } } }

POST /api/reviews/{id}/reply

Seller phản hồi 1 đánh giá --- chỉ được gọi 1 lần duy nhất.

Xác thực: Bearer token --- [Seller][Verified] (chủ sản phẩm được đánh giá)

Request Body:

Trường   Kiểu   Bắt buộc   Mô tả

response     string     ✔              ≤ 1000 ký tự

Response --- HTTP 200 OK:

Trường      Kiểu   Mô tả

data.response   string

Ví dụ Request:

{ "response": "Cảm ơn bạn đã ủng hộ shop!" }

Ví dụ Response:

{ "success": true, "data": { "id": 30, "response": "Cảm ơn bạn đã ủng hộ shop!" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

409               REVIEW_ALREADY_REPLIED    Review.response đã có giá trị

GET /api/feedback/{sellerId}

Chỉ số uy tín tổng hợp của gian hàng.

Xác thực: Public

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

sellerId     number     ✔

Response --- HTTP 200 OK:

Trường                                       Kiểu   Mô tả

data.averageRating, totalReviews, positiveRate   -

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "averageRating": 4.8, "totalReviews": 152, "positiveRate": 96.7 } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

404               FEEDBACK_NOT_FOUND

GET /api/disputes

Danh sách khiếu nại liên quan tới seller.

Xác thực: Bearer token --- [Seller][Verified]

Query Params:

Trường                         Kiểu   Bắt buộc   Mô tả

sellerId, status, page, pageSize   -         -             status: Open|UnderReview|Resolved

Response --- HTTP 200 OK:

Trường       Kiểu      Mô tả

data.items[]   Dispute[]

Ví dụ Request:

GET /api/disputes?sellerId=5&status=Open

Ví dụ Response:

{ "success": true, "data": { "items": [ { "id": 7, "orderId": 555, "status": "Open", "description": "Hàng nhận được khác mô tả" } ] } }

PUT /api/disputes/{id}/resolve

Ghi nhận phương án giải quyết khiếu nại.

Xác thực: Bearer token --- [Seller][Verified] hoặc Admin

Request Body:

Trường   Kiểu   Bắt buộc   Mô tả

resolution   string     ✔              Nội dung phương án xử lý

Response --- HTTP 200 OK:

Trường    Kiểu   Mô tả

data.status   string     'Resolved'

Ví dụ Request:

{ "resolution": "Hoàn tiền 100% cho buyer, buyer giữ lại sản phẩm" }

Ví dụ Response:

{ "success": true, "data": { "id": 7, "status": "Resolved" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)    Khi nào xảy ra

409               DISPUTE_ALREADY_RESOLVED

GET /api/returns

Danh sách yêu cầu đổi trả liên quan tới seller.

Xác thực: Bearer token --- [Seller][Verified]

Query Params:

Trường                         Kiểu   Bắt buộc   Mô tả

sellerId, status, page, pageSize   -         -             status: Requested|Accepted|RefundOffered|Declined|RefundedByReturn|Closed

Response --- HTTP 200 OK:

Trường       Kiểu            Mô tả

data.items[]   ReturnRequest[]

Ví dụ Request:

GET /api/returns?sellerId=5&status=Requested

Ví dụ Response:

{ "success": true, "data": { "items": [ { "id": 12, "orderId": 555, "reason": "Sản phẩm bị lỗi màn hình", "status": "Requested", "createdAt": "2026-08-19T12:00:00Z" } ] } }

PUT /api/returns/{id}/status

Seller phản hồi yêu cầu đổi trả trong hạn 3 ngày làm việc (theo Money Back Guarantee).

Xác thực: Bearer token --- [Seller][Verified]

Request Body:

Trường   Kiểu      Bắt buộc   Mô tả

status       string enum   ✔              Accepted|RefundOffered|Declined|RefundedByReturn

Response --- HTTP 200 OK:

Trường    Kiểu   Mô tả

data.status   string

Ví dụ Request:

{ "status": "Accepted" }

Ví dụ Response:

{ "success": true, "data": { "id": 12, "status": "Accepted" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)     Khi nào xảy ra

422               INVALID_STATUS_TRANSITION   Sai thứ tự state machine
409               DECLINE_NOT_ALLOWED         Cố Decline khi lý do là hàng lỗi/không đúng mô tả

Module 6 --- Thành viên 6 {#module-6--thành-viên-6}

Dashboard Báo cáo Doanh số

Bảng dữ liệu phụ trách: Đọc tổng hợp OrderItem, Payment, Product; đề xuất thêm SalesSummary (denormalized)

Tổng quan nghiệp vụ (đối chiếu eBay thực tế)

Đây là màn hình cuối chuỗi nghiệp vụ (không cung cấp API cho module khác) --- tổng hợp dữ liệu bán hàng thành KPI, biểu đồ, top sản phẩm, tương tự Seller Hub Performance/Sales report của eBay. Vì đề bài yêu cầu tải trang < 1 giây, toàn bộ API ở đây đọc từ bảng SalesSummary đã tổng hợp sẵn (denormalized) thay vì JOIN trực tiếp OrderItem+Payment+Product mỗi lần gọi.

Đặc tả chi tiết theo từng tính năng

F6.1. Tổng quan doanh số (KPI card) {#f61-tổng-quan-doanh-số-kpi-card}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Hiển thị tổng doanh thu, tổng số đơn, giá trị đơn trung bình (AOV) theo kỳ tuần/tháng được chọn.

Dữ liệu lấy từ Redis cache (khoá seller:{id}:dashboard:{period}, TTL 5--10 phút) trước, cache miss mới truy vấn SalesSummary.

Quy tắc nghiệp vụ / ràng buộc:

Chỉ tính các đơn có Payment.status = Completed (không tính đơn Pending/Cancelled vào doanh thu).

F6.2. Top sản phẩm bán chạy {#f62-top-sản-phẩm-bán-chạy}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Xếp hạng sản phẩm theo tổng số lượng bán hoặc doanh thu trong kỳ, phục vụ seller quay lại điều chỉnh bước Đăng bán & Tồn kho --- khép kín vòng lặp nghiệp vụ theo Hình 1 của kế hoạch gốc.

Quy tắc nghiệp vụ / ràng buộc:

Giới hạn top N (mặc định 10, tối đa 50).

F6.3. Biểu đồ doanh số theo thời gian {#f63-biểu-đồ-doanh-số-theo-thời-gian}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Biểu đồ đường/cột theo ngày trong tuần hoặc theo tuần trong tháng, phục vụ nhận diện xu hướng bán hàng.

Quy tắc nghiệp vụ / ràng buộc:

Dữ liệu rỗng (chưa có đơn) vẫn phải trả mảng đủ số điểm thời gian với giá trị 0, không bỏ trống, để FE vẽ chart không lỗi.

F6.4 (bổ sung). Chỉ số hiệu suất seller cơ bản (Seller Performance) {#f64-bổ-sung-chỉ-số-hiệu-suất-seller-cơ-bản-seller-performance}

Actor: Seller đã Verified

Luồng nghiệp vụ chính:

Bổ sung nhẹ, mức nice-to-have: tỉ lệ đơn giao trễ (đếm ShippingInfo có ngày giao thực tế trễ hơn estimatedArrival) và tỉ lệ Dispute/Return trên tổng số đơn --- mô phỏng rút gọn khái niệm Late Shipment Rate & Transaction Defect Rate của eBay Seller Level.

Quy tắc nghiệp vụ / ràng buộc:

Không bắt buộc trong phạm vi 6 ngày, đặt độ ưu tiên thấp nhất trong module 6.

API Spec

GET /api/dashboard/summary

KPI tổng quan doanh số theo kỳ.

Xác thực: Bearer token --- [Seller][Verified]

Query Params:

Trường   Kiểu                  Bắt buộc   Mô tả

sellerId     number                    ✔
period       string enum(week,month)   ✔

Response --- HTTP 200 OK:

Trường               Kiểu   Mô tả

data.totalRevenue        decimal
data.totalOrders         number
data.averageOrderValue   decimal

Ví dụ Request:

GET /api/dashboard/summary?sellerId=5&period=week

Ví dụ Response:

{ "success": true, "data": { "totalRevenue": 87500000, "totalOrders": 23, "averageOrderValue": 3804347.8 } }

GET /api/dashboard/top-products

Top sản phẩm bán chạy trong kỳ.

Xác thực: Bearer token --- [Seller][Verified]

Query Params:

Trường                Kiểu   Bắt buộc               Mô tả

sellerId, period, limit   -         sellerId,period bắt buộc   limit mặc định 10

Response --- HTTP 200 OK:

Trường       Kiểu                                          Mô tả

data.items[]   { productId, title, quantitySold, revenue }[]

Ví dụ Request:

GET /api/dashboard/top-products?sellerId=5&period=month&limit=5

Ví dụ Response:

{ "success": true, "data": { "items": [ { "productId": 101, "title": "iPhone 13 Pro Max", "quantitySold": 8, "revenue": 116000000 } ] } }

GET /api/dashboard/revenue-chart

Dữ liệu vẽ biểu đồ doanh số theo thời gian.

Xác thực: Bearer token --- [Seller][Verified]

Query Params:

Trường         Kiểu   Bắt buộc   Mô tả

sellerId, period   -         ✔

Response --- HTTP 200 OK:

Trường        Kiểu                 Mô tả

data.points[]   { label, revenue }[]   Đủ số điểm thời gian, kể cả giá trị 0

Ví dụ Request:

GET /api/dashboard/revenue-chart?sellerId=5&period=week

Ví dụ Response:

{ "success": true, "data": { "points": [ { "label": "T2", "revenue": 12000000 }, { "label": "T3", "revenue": 0 } ] } }

GET /api/dashboard/performance

Chỉ số hiệu suất cơ bản: tỉ lệ giao trễ, tỉ lệ dispute/return.

Xác thực: Bearer token --- [Seller][Verified] --- bổ sung

Query Params:

Trường         Kiểu   Bắt buộc   Mô tả

sellerId, period   -         ✔

Response --- HTTP 200 OK:

Trường                           Kiểu      Mô tả

data.lateShipmentRate, disputeRate   decimal (%)

Ví dụ Request:

GET /api/dashboard/performance?sellerId=5&period=month

Ví dụ Response:

{ "success": true, "data": { "lateShipmentRate": 2.1, "disputeRate": 0.8 } }

Module 7 --- Thành viên 7 {#module-7--thành-viên-7}

Chat / Tin nhắn CSKH

Bảng dữ liệu phụ trách: Message

Tổng quan nghiệp vụ (đối chiếu eBay thực tế)

Mô phỏng kênh 'Ask seller a question' + hộp thư hợp nhất của eBay: buyer và seller trao đổi trực tiếp trong 1 luồng hội thoại. Vì bảng Message trong schema chỉ có senderId/receiverId (không có productId), luồng chat ở đây là theo cặp người dùng --- phần ngữ cảnh sản phẩm được ghi nhận là hạn chế cần mở rộng (xem Ghi chú hiệu chỉnh nghiệp vụ, mục 8).

Đặc tả chi tiết theo từng tính năng

F7.1. Danh sách hội thoại {#f71-danh-sách-hội-thoại}

Actor: Seller đã Verified, Buyer

Luồng nghiệp vụ chính:

Liệt kê tất cả người đã từng nhắn tin qua lại với user hiện tại, sắp xếp theo tin nhắn gần nhất, kèm số tin chưa đọc.

Quy tắc nghiệp vụ / ràng buộc:

Gộp nhóm theo (senderId, receiverId) không phân biệt chiều gửi.

F7.2. Lịch sử tin nhắn 1-1 {#f72-lịch-sử-tin-nhắn-1-1}

Actor: Seller đã Verified, Buyer

Luồng nghiệp vụ chính:

Xem toàn bộ tin nhắn giữa user hiện tại và 1 người cụ thể, phân trang theo thời gian (tin cũ load thêm khi cuộn lên).

F7.3. Gửi tin nhắn (realtime) {#f73-gửi-tin-nhắn-realtime}

Actor: Seller đã Verified, Buyer

Luồng nghiệp vụ chính:

Gửi qua REST để lưu DB, đồng thời đẩy realtime qua WebSocket/SignalR hub cho người nhận đang online; nếu không kịp làm realtime trong 6 ngày, cho phép fallback polling 3--5 giây.

Quy tắc nghiệp vụ / ràng buộc:

content không được rỗng; giới hạn độ dài hợp lý (ví dụ ≤ 2000 ký tự).

F7.4. Badge tin nhắn chưa đọc {#f74-badge-tin-nhắn-chưa-đọc}

Actor: Seller đã Verified, Buyer

Luồng nghiệp vụ chính:

Đánh dấu đã đọc khi user mở đúng hội thoại; số đếm chưa đọc hiển thị trên icon Chat toàn hệ thống.

API Spec

GET /api/messages/conversations

Danh sách hội thoại của user hiện tại.

Xác thực: Bearer token

Query Params:

Trường   Kiểu   Bắt buộc                                       Mô tả

userId       number     ✔ (lấy từ JWT, không cho FE tự truyền user khác)

Response --- HTTP 200 OK:

Trường       Kiểu                                                                    Mô tả

data.items[]   { withUserId, withUsername, lastMessage, lastMessageAt, unreadCount }[]

Ví dụ Request:

GET /api/messages/conversations?userId=5

Ví dụ Response:

{ "success": true, "data": { "items": [ { "withUserId": 20, "withUsername": "buyer01", "lastMessage": "Sản phẩm còn hàng không shop?", "lastMessageAt": "2026-08-20T08:00:00Z", "unreadCount": 2 } ] } }

GET /api/messages/{conversationWith}

Lịch sử tin nhắn giữa user hiện tại và 1 người khác.

Xác thực: Bearer token

Path/Query Params:

Trường         Kiểu   Bắt buộc   Mô tả

conversationWith   number     ✔              userId đối phương

Query Params:

Trường       Kiểu   Bắt buộc   Mô tả

page, pageSize   -         -

Response --- HTTP 200 OK:

Trường       Kiểu      Mô tả

data.items[]   Message[]   { senderId, receiverId, content, timestamp }

Ví dụ Request:

GET /api/messages/20?page=1&pageSize=30

Ví dụ Response:

{ "success": true, "data": { "items": [ { "id": 1001, "senderId": 20, "receiverId": 5, "content": "Sản phẩm còn hàng không shop?", "timestamp": "2026-08-20T08:00:00Z" } ] } }

POST /api/messages

Gửi tin nhắn mới.

Xác thực: Bearer token

Request Body:

Trường   Kiểu   Bắt buộc   Mô tả

receiverId   number     ✔
content      string     ✔              ≤ 2000 ký tự

Response --- HTTP 201 Created:

Trường   Kiểu         Mô tả

data         Message object

Ví dụ Request:

{ "receiverId": 20, "content": "Dạ còn hàng ạ, shop gửi trong hôm nay!" }

Ví dụ Response:

{ "success": true, "data": { "id": 1002, "senderId": 5, "receiverId": 20, "content": "Dạ còn hàng ạ, shop gửi trong hôm nay!", "timestamp": "2026-08-20T08:05:00Z" } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

422               EMPTY_CONTENT             content rỗng

PUT /api/messages/{id}/read

Đánh dấu đã đọc, phục vụ badge tin nhắn chưa đọc.

Xác thực: Bearer token --- bổ sung

Path/Query Params:

Trường   Kiểu   Bắt buộc   Mô tả

id           number     ✔

Response --- HTTP 200 OK:

Trường   Kiểu   Mô tả

data.read    boolean    true

Ví dụ Request:

(không có body)

Ví dụ Response:

{ "success": true, "data": { "id": 1001, "read": true } }

Mã lỗi có thể xảy ra:

HTTP Status   Mã lỗi (error.code)   Khi nào xảy ra

403               NOT_RECEIVER              User không phải người nhận tin nhắn này

WS /hubs/messages

Kênh realtime: server đẩy sự kiện messageReceived tới client đang mở kết nối tương ứng receiverId. Fallback: FE polling GET /api/messages/{conversationWith} mỗi 3--5 giây nếu không kịp triển khai SignalR/WebSocket.

Xác thực: Bearer token (query hoặc header khi kết nối WebSocket)

Response --- HTTP 101 Switching Protocols (khi kết nối):

Trường               Kiểu         Mô tả

event: messageReceived   Message object   Đẩy realtime khi có tin nhắn mới

Ví dụ Request:

(kết nối WebSocket, không phải REST)

Ví dụ Response:

{ "event": "messageReceived", "data": { "id": 1002, "senderId": 5, "receiverId": 20, "content": "...", "timestamp": "..." } }
