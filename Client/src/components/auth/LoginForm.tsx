import React, { useState } from 'react';
import { useAuth } from '../../context/AuthContext';
import { Button } from '../common/Button';
import { Input } from '../common/Input';
import { User, Lock, AlertCircle } from 'lucide-react';

interface LoginFormProps {
  onToggleMode: () => void;
}

export const LoginForm: React.FC<LoginFormProps> = ({ onToggleMode }) => {
  const { login } = useAuth();
  const [formData, setFormData] = useState({
    userName: '',
    password: '',
  });
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);

    try {
      await login(formData);
    } catch (err) {
      setError('Invalid username or password');
    } finally {
      setIsLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData(prev => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  return (
    <div className="w-full max-w-md space-y-6">
      <div className="text-center">
        <h2 className="text-3xl font-bold text-gray-900">Welcome back</h2>
        <p className="mt-2 text-gray-600">Sign in to your account</p>
      </div>

      {error && (
        <div className="flex items-center space-x-2 p-3 bg-red-50 border border-red-200 rounded-lg">
          <AlertCircle className="w-4 h-4 text-red-600" />
          <span className="text-sm text-red-600">{error}</span>
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4">
        <Input
          label="Username"
          name="userName"
          type="text"
          value={formData.userName}
          onChange={handleChange}
          icon={User}
          placeholder="Enter your username"
          required
        />

        <Input
          label="Password"
          name="password"
          type="password"
          value={formData.password}
          onChange={handleChange}
          icon={Lock}
          placeholder="Enter your password"
          required
        />

        <Button
          type="submit"
          size="lg"
          className="w-full"
          isLoading={isLoading}
        >
          Sign In
        </Button>
      </form>

      <div className="text-center">
        <p className="text-gray-600">
          Don't have an account?{' '}
          <button
            onClick={onToggleMode}
            className="text-blue-600 hover:text-blue-700 font-medium"
          >
            Sign up
          </button>
        </p>
      </div>
    </div>
  );
};