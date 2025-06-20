import React from 'react';
import { useAuth } from '../../context/AuthContext';
import { Button } from '../common/Button';
import { LogOut, User, CheckSquare, Shield } from 'lucide-react';
import { Link } from 'react-router-dom';

export const Header: React.FC = () => {
  const { user, logout } = useAuth();

  const isAdmin = user?.role === 'Admin' || user?.role === 'SuperAdmin';

  return (
    <header className="bg-white border-b border-gray-200 px-6 py-4 shadow-sm">
      <div className="flex items-center justify-between">
        {/* Logo & App Title */}
        <div className="flex items-center space-x-3">
          <div className="p-2 bg-blue-100 rounded-lg">
            <CheckSquare className="w-6 h-6 text-blue-600" />
          </div>
          <div>
            <h1 className="text-xl font-bold text-gray-900">TodoFlow</h1>
            <p className="text-sm text-gray-500">Stay organized, stay productive</p>
          </div>
        </div>

        {/* Right side */}
        <div className="flex items-center space-x-4">
          {/* Admin Panel button */}
          {isAdmin && (
            <Link
              to="/admin"
              className="inline-flex items-center space-x-2 px-4 py-2 rounded-md bg-red-100 text-red-700 hover:bg-red-200 transition-colors border border-red-200 text-sm font-medium"
            >
              <Shield className="w-4 h-4" />
              <span>Admin Panel</span>
            </Link>
          )}

          {/* User Info */}
          <div className="flex items-center space-x-3">
            <div className="p-2 bg-gray-100 rounded-full">
              <User className="w-4 h-4 text-gray-600" />
            </div>
            <div className="hidden md:block">
              <p className="text-sm font-medium text-gray-900">
                {user?.firstName} {user?.lastName}
              </p>
              <p className="text-xs text-gray-500">{user?.role}</p>
            </div>
          </div>

          {/* Logout */}
          <Button variant="secondary" size="sm" icon={LogOut} onClick={logout}>
            Logout
          </Button>
        </div>
      </div>
    </header>
  );
};
