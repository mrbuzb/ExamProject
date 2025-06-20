import React, { createContext, useContext, useEffect, useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import { User, LoginResponse } from '../types';
import { apiService } from '../services/api';

interface DecodedToken {
  UserId: string;
  FirstName: string;
  LastName: string;
  PhoneNumber: string;
  unique_name: string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
}

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (credentials: { userName: string; password: string }) => Promise<void>;
  register: (userData: any) => Promise<void>;
  logout: () => void;
  isLoading: boolean;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedToken = localStorage.getItem('accessToken');
    const storedUser = localStorage.getItem('user');

    if (storedToken && storedUser) {
      setToken(storedToken);
      setUser(JSON.parse(storedUser));
    }
    setIsLoading(false);
  }, []);

  const login = async (credentials: { userName: string; password: string }) => {
    try {
      const response: LoginResponse = await apiService.login(credentials);

      const decoded: DecodedToken = jwtDecode(response.accessToken);

      console.log("Decoded token:", decoded);
console.log("Extracted role:", decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]);


      const userFromToken: User = {
  userId: parseInt(decoded.UserId),
  firstName: decoded.FirstName,
  lastName: decoded.LastName,
  userName: decoded.unique_name,
  email: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
  phoneNumber: decoded.PhoneNumber,
  role: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"], // ← SHU
};

      

      setToken(response.accessToken);
      setUser(userFromToken);

      localStorage.setItem('accessToken', response.accessToken);
      localStorage.setItem('refreshToken', response.refreshToken);
      localStorage.setItem('user', JSON.stringify(userFromToken));
    } catch (error) {
      console.error('Login failed:', error);
      throw error;
    }
  };

  const register = async (userData: any) => {
    try {
      await apiService.register(userData);
    } catch (error) {
      console.error('Registration failed:', error);
      throw error;
    }
  };

  const logout = async () => {
    try {
      const refreshToken = localStorage.getItem('refreshToken');
      if (refreshToken) {
        await apiService.logout(refreshToken);
      }
    } catch (error) {
      console.error('Logout failed:', error);
    } finally {
      setUser(null);
      setToken(null);
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
    }
  };

  const value = {
    user,
    token,
    login,
    register,
    logout,
    isLoading,
    isAuthenticated: !!token,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
