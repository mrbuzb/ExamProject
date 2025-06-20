import React, { useState } from 'react';
import { useAuth } from '../../context/AuthContext';
import { Button } from '../common/Button';
import { Input } from '../common/Input';
import { User, Mail, Lock, Phone, AlertCircle, CheckCircle } from 'lucide-react';

interface RegisterFormProps {
  onToggleMode: () => void;
}

export const RegisterForm: React.FC<RegisterFormProps> = ({ onToggleMode }) => {
  const { register } = useAuth();
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    phoneNumber: '',
    password: '',
    confirmPassword: '',
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);

    if (formData.password !== formData.confirmPassword) {
      setError('Passwords do not match');
      setIsLoading(false);
      return;
    }

    try {
      const { confirmPassword, ...registrationData } = formData;
      await register(registrationData);
      setSuccess(true);
    } catch (err) {
      setError('Registration failed. Please try again.');
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

  if (success) {
    return (
      <div className="w-full max-w-md space-y-6 text-center">
        <div className="p-4 bg-green-50 border border-green-200 rounded-lg">
          <CheckCircle className="w-8 h-8 text-green-600 mx-auto mb-2" />
          <h3 className="text-lg font-semibold text-green-900">Registration Successful!</h3>
          <p className="text-green-700 mt-1">Your account has been created successfully.</p>
        </div>
        <Button onClick={onToggleMode} className="w-full">
          Sign In Now
        </Button>
      </div>
    );
  }

  return (
    <div className="w-full max-w-md space-y-6">
      <div className="text-center">
        <h2 className="text-3xl font-bold text-gray-900">Create account</h2>
        <p className="mt-2 text-gray-600">Join us to get started</p>
      </div>

      {error && (
        <div className="flex items-center space-x-2 p-3 bg-red-50 border border-red-200 rounded-lg">
          <AlertCircle className="w-4 h-4 text-red-600" />
          <span className="text-sm text-red-600">{error}</span>
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="grid grid-cols-2 gap-4">
          <Input
            label="First Name"
            name="firstName"
            type="text"
            value={formData.firstName}
            onChange={handleChange}
            placeholder="John"
            required
          />
          <Input
            label="Last Name"
            name="lastName"
            type="text"
            value={formData.lastName}
            onChange={handleChange}
            placeholder="Doe"
            required
          />
        </div>

        <Input
          label="Username"
          name="userName"
          type="text"
          value={formData.userName}
          onChange={handleChange}
          icon={User}
          placeholder="johndoe"
          required
        />

        <Input
          label="Email"
          name="email"
          type="email"
          value={formData.email}
          onChange={handleChange}
          icon={Mail}
          placeholder="john@example.com"
          required
        />

        <Input
          label="Phone Number"
          name="phoneNumber"
          type="tel"
          value={formData.phoneNumber}
          onChange={handleChange}
          icon={Phone}
          placeholder="+1 (555) 000-0000"
          required
        />

        <Input
          label="Password"
          name="password"
          type="password"
          value={formData.password}
          onChange={handleChange}
          icon={Lock}
          placeholder="Create a strong password"
          required
        />

        <Input
          label="Confirm Password"
          name="confirmPassword"
          type="password"
          value={formData.confirmPassword}
          onChange={handleChange}
          icon={Lock}
          placeholder="Confirm your password"
          required
        />

        <Button
          type="submit"
          size="lg"
          className="w-full"
          isLoading={isLoading}
        >
          Create Account
        </Button>
      </form>

      <div className="text-center">
        <p className="text-gray-600">
          Already have an account?{' '}
          <button
            onClick={onToggleMode}
            className="text-blue-600 hover:text-blue-700 font-medium"
          >
            Sign in
          </button>
        </p>
      </div>
    </div>
  );
};