import React from 'react';
import { NavLink } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import {
  Home,
  CheckSquare,
  AlertCircle,
  Calendar,
  Users,
  Shield,
  Search,
  BarChart3,
} from 'lucide-react';

const navigationItems = [
  {
    name: 'Dashboard',
    href: '/dashboard',
    icon: Home,
    roles: ['User', 'Admin', 'SuperAdmin'],
  },
  {
    name: 'My Todos',
    href: '/todos',
    icon: CheckSquare,
    roles: ['User', 'Admin', 'SuperAdmin'],
  },
  {
    name: 'Overdue',
    href: '/todos/overdue',
    icon: AlertCircle,
    roles: ['User', 'Admin', 'SuperAdmin'],
  },
  {
    name: 'Calendar',
    href: '/calendar',
    icon: Calendar,
    roles: ['User', 'Admin', 'SuperAdmin'],
  },
  {
    name: 'Search',
    href: '/search',
    icon: Search,
    roles: ['User', 'Admin', 'SuperAdmin'],
  },
  {
    name: 'Analytics',
    href: '/analytics',
    icon: BarChart3,
    roles: ['Admin', 'SuperAdmin'],
  },
  {
    name: 'User Management',
    href: '/admin/users',
    icon: Users,
    roles: ['Admin', 'SuperAdmin'],
  },
  {
    name: 'Role Management',
    href: '/admin/roles',
    icon: Shield,
    roles: ['SuperAdmin'],
  },
];

export const Sidebar: React.FC = () => {
  const { user } = useAuth();

  const filteredItems = navigationItems.filter(item =>
    item.roles.includes(user?.role || 'User')
  );

  return (
    <div className="w-64 bg-gray-50 border-r border-gray-200 h-full">
      <nav className="p-4 space-y-2">
        {filteredItems.map((item) => (
          <NavLink
            key={item.name}
            to={item.href}
            className={({ isActive }) =>
              `flex items-center space-x-3 px-3 py-2 rounded-lg text-sm font-medium transition-all duration-200 ${
                isActive
                  ? 'bg-blue-100 text-blue-700 shadow-sm'
                  : 'text-gray-700 hover:bg-gray-100 hover:text-gray-900'
              }`
            }
          >
            <item.icon className="w-5 h-5" />
            <span>{item.name}</span>
          </NavLink>
        ))}
      </nav>
    </div>
  );
};