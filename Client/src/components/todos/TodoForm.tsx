import React, { useState } from 'react';
import { Modal } from '../common/Modal';
import { Button } from '../common/Button';
import { Input } from '../common/Input';
import { CreateTodoRequest } from '../../types';

interface TodoFormProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (todo: CreateTodoRequest) => void;
  isLoading?: boolean;
}

export const TodoForm: React.FC<TodoFormProps> = ({
  isOpen,
  onClose,
  onSubmit,
  isLoading = false,
}) => {
  const today = new Date();
  const defaultDate = today.toISOString().split('T')[0]; // YYYY-MM-DD
  const defaultTime = today.toTimeString().slice(0, 5);   // HH:mm

  const [formData, setFormData] = useState({
    title: '',
    description: '',
    dueDate: defaultDate,
    dueTime: defaultTime,
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const fullDateTime = new Date(`${formData.dueDate}T${formData.dueTime}`);

    // ✅ Local vaqtni to‘g‘ri ISO formatga o‘girish (UTC xatolarni oldini olish uchun)
    const localISOString = new Date(
      fullDateTime.getTime() - fullDateTime.getTimezoneOffset() * 60000
    ).toISOString();

    const payload: CreateTodoRequest = {
      title: formData.title,
      description: formData.description,
      dueDate: localISOString,
      isCompleted: false,
    };

    onSubmit(payload);

    setFormData({
      title: '',
      description: '',
      dueDate: defaultDate,
      dueTime: defaultTime,
    });
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Create New Todo" maxWidth="lg">
      <form onSubmit={handleSubmit} className="space-y-4">
        <Input
          label="Title"
          name="title"
          value={formData.title}
          onChange={handleChange}
          placeholder="Enter todo title"
          required
        />

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Description
          </label>
          <textarea
            name="description"
            value={formData.description}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            rows={3}
            placeholder="Enter description (optional)"
          />
        </div>

        <div className="grid grid-cols-2 gap-4">
          <Input
            label="Due Date"
            name="dueDate"
            type="date"
            value={formData.dueDate}
            onChange={handleChange}
            required
          />
          <Input
            label="Due Time"
            name="dueTime"
            type="time"
            value={formData.dueTime}
            onChange={handleChange}
            required
          />
        </div>

        <div className="flex space-x-3 pt-4">
          <Button type="submit" className="flex-1" isLoading={isLoading}>
            Create Todo
          </Button>
          <Button type="button" variant="secondary" onClick={onClose} className="flex-1">
            Cancel
          </Button>
        </div>
      </form>
    </Modal>
  );
};
