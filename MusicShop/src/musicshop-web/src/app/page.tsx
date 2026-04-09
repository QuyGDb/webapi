'use client';

import React from 'react';
import Link from 'next/link';
import { useAuthStore } from '@/store/authStore';
import { Button, buttonVariants } from '@/components/ui/button';
import { Music, LogOut, User as UserIcon, ShoppingBag } from 'lucide-react';
import { useRouter } from 'next/navigation';
import { cn } from '@/lib/utils';

export default function Home() {
  const { user, isAuthenticated, clearAuth } = useAuthStore();
  const router = useRouter();

  const handleLogout = () => {
    clearAuth();
    router.refresh(); // Refresh the page to trigger middleware check
  };

  return (
    <div className="min-h-screen bg-black text-white flex flex-col">
      {/* Header/Navbar */}
      <header className="border-b border-neutral-800 bg-black/50 backdrop-blur-md sticky top-0 z-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <Music className="h-8 w-8 text-blue-500" />
            <span className="text-xl font-bold tracking-tight">MusicShop</span>
          </div>

          <div className="flex items-center gap-4">
            {isAuthenticated ? (
              <>
                <Link href="/profile" className="text-sm font-medium hover:text-blue-400 transition-colors flex items-center gap-1">
                  <UserIcon className="h-4 w-4" />
                  {user?.fullName}
                </Link>
                <Button variant="ghost" size="sm" onClick={handleLogout} className="text-neutral-400 hover:text-white">
                  <LogOut className="h-4 w-4 mr-1" />
                  Logout
                </Button>
              </>
            ) : (
              <Link 
                href="/login" 
                className={cn(buttonVariants({ variant: 'default' }), "bg-blue-600 hover:bg-blue-700")}
              >
                Sign In
              </Link>
            )}
          </div>
        </div>
      </header>

      {/* Hero Section */}
      <main className="flex-1 flex flex-col items-center justify-center text-center px-4 relative overflow-hidden">
        {/* Abstract background decorations */}
        <div className="absolute top-1/4 left-1/4 w-64 h-64 bg-blue-600/10 rounded-full blur-3xl -z-10" />
        <div className="absolute bottom-1/4 right-1/4 w-96 h-96 bg-purple-600/10 rounded-full blur-3xl -z-10" />

        <div className="max-w-3xl space-y-8">
          <h1 className="text-5xl md:text-7xl font-extrabold tracking-tighter leading-tight bg-clip-text text-transparent bg-gradient-to-b from-white to-neutral-500">
            Where Your Soul <br /> 
            Meets the Melody
          </h1>
          
          <p className="text-xl md:text-2xl text-neutral-400 max-w-2xl mx-auto leading-relaxed">
            {isAuthenticated 
              ? `Welcome back, ${user?.fullName}! Are you ready to explore our latest collection?`
              : "Explore thousands of high-quality vinyl records and CDs. Sign up now to start your musical journey."
            }
          </p>

          <div className="flex flex-col sm:flex-row items-center justify-center gap-4 pt-4">
            <Button size="lg" className="w-full sm:w-auto px-8 h-14 text-lg bg-white text-black hover:bg-neutral-200">
              <ShoppingBag className="mr-2 h-5 w-5" />
              Explore Shop
            </Button>
            
            {!isAuthenticated && (
              <Link 
                href="/register" 
                className={cn(
                  buttonVariants({ variant: 'outline', size: 'lg' }), 
                  "w-full sm:w-auto px-8 h-14 text-lg border-neutral-700 hover:bg-neutral-900"
                )}
              >
                Create Account
              </Link>
            )}
          </div>
        </div>
      </main>

      {/* Footer */}
      <footer className="py-8 border-t border-neutral-800 text-center text-neutral-600 text-sm">
        &copy; {new Date().getFullYear()} MusicShop. All rights reserved.
      </footer>
    </div>
  );
}
