'use client';

import React from 'react';
import { useAuthStore } from '@/store/authStore';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { useRouter } from 'next/navigation';

export default function ProfilePage() {
  const { user, clearAuth } = useAuthStore();
  const router = useRouter();

  if (!user) return null; // Sẽ được Middleware xử lý, nhưng check cho chắc

  const handleLogout = () => {
    clearAuth();
    router.push('/login');
  };

  return (
    <div className="min-h-screen bg-black text-white p-8">
      <div className="max-w-2xl mx-auto space-y-6">
        <h1 className="text-3xl font-bold">Thông tin cá nhân</h1>
        
        <Card className="bg-neutral-900 border-neutral-800">
          <CardHeader>
            <CardTitle className="text-xl">Hồ sơ người dùng</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-3 gap-4 border-b border-neutral-800 pb-4">
              <span className="text-neutral-400">Họ và tên</span>
              <span className="col-span-2 font-medium">{user.fullName}</span>
            </div>
            <div className="grid grid-cols-3 gap-4 border-b border-neutral-800 pb-4">
              <span className="text-neutral-400">Email</span>
              <span className="col-span-2 font-medium">{user.email}</span>
            </div>
            <div className="grid grid-cols-3 gap-4 border-b border-neutral-800 pb-4">
              <span className="text-neutral-400">Quyền hạn</span>
              <span className="col-span-2 capitalize font-medium">{user.role}</span>
            </div>
            
            <div className="pt-6">
              <Button 
                variant="destructive" 
                onClick={handleLogout}
                className="w-full sm:w-auto"
              >
                Đăng xuất tài khoản
              </Button>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
