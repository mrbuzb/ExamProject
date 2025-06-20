import React, { useState } from 'react';
import { LoginForm } from '../components/auth/LoginForm';
import { RegisterForm } from '../components/auth/RegisterForm';
import { CheckSquare } from 'lucide-react';

export const AuthPage: React.FC = () => {
  const [isLoginMode, setIsLoginMode] = useState(true);

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50 flex items-center justify-center p-4">
      <div className="w-full max-w-4xl flex bg-white rounded-2xl shadow-xl overflow-hidden">
        {/* Left Panel - Branding */}
        <div className="hidden lg:flex lg:w-1/2 bg-gradient-to-br from-blue-600 to-purple-700 p-12 flex-col justify-center">
          <div className="text-white">
            <div className="flex items-center space-x-3 mb-8">
              <div className="p-3 bg-white/20 rounded-lg">
                <CheckSquare className="w-8 h-8" />
              </div>
              <h1 className="text-3xl font-bold">TodoFlow</h1>
            </div>
            
            <h2 className="text-4xl font-bold mb-6">
              Stay organized,<br />stay productive
            </h2>
            
            <p className="text-xl text-blue-100 mb-8">
              Manage your tasks efficiently with our intuitive todo application. 
              Track progress, set deadlines, and boost your productivity.
            </p>
            
            <div className="space-y-4">
              <div className="flex items-center space-x-3">
                <div className="w-2 h-2 bg-white rounded-full"></div>
                <span className="text-blue-100">Smart task management</span>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-2 h-2 bg-white rounded-full"></div>
                <span className="text-blue-100">Deadline tracking</span>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-2 h-2 bg-white rounded-full"></div>
                <span className="text-blue-100">Team collaboration</span>
              </div>
            </div>
          </div>
        </div>

        {/* Right Panel - Auth Forms */}
        <div className="w-full lg:w-1/2 p-8 lg:p-12 flex items-center justify-center">
          {isLoginMode ? (
            <LoginForm onToggleMode={() => setIsLoginMode(false)} />
          ) : (
            <RegisterForm onToggleMode={() => setIsLoginMode(true)} />
          )}
        </div>
      </div>
    </div>
  );
};