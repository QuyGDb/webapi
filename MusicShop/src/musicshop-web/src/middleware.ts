import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

// Định nghĩa các đường dẫn cần bảo vệ (cần đăng nhập mới vào được)
const protectedRoutes = ['/profile', '/dashboard', '/orders'];
// Định nghĩa các đường dẫn dành cho khách (đã đăng nhập thì không vào lại nữa)
const authRoutes = ['/login', '/register'];

export function middleware(request: NextRequest) {
  const token = request.cookies.get('accessToken')?.value;
  const { pathname } = request.nextUrl;

  // 1. Kiểm tra nếu vào trang bảo vệ mà chưa có token
  const isProtectedRoute = protectedRoutes.some((route) => pathname.startsWith(route));
  if (isProtectedRoute && !token) {
    const loginUrl = new URL('/login', request.url);
    // Lưu lại trang đang định vào để sau khi login xong quay lại (tùy chọn)
    // loginUrl.searchParams.set('callbackUrl', pathname);
    return NextResponse.redirect(loginUrl);
  }

  // 2. Kiểm tra nếu đã có token mà cố tình vào trang login/register
  const isAuthRoute = authRoutes.some((route) => pathname.startsWith(route));
  if (isAuthRoute && token) {
    return NextResponse.redirect(new URL('/', request.url));
  }

  return NextResponse.next();
}

// Chỉ chạy middleware cho các đường dẫn cụ thể để tối ưu hiệu năng
export const config = {
  matcher: ['/profile/:path*', '/dashboard/:path*', '/login', '/register'],
};
