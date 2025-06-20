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
  const [formData, setFormData] = useState<CreateTodoRequest>({
    title: '',
    description: '',
    dueDate: new Date().toISOString().split('T')[0],
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({
      ...formData,
      dueDate: new Date(formData.dueDate).toISOString(),
    });
    setFormData({
      title: '',
      description: '',
      dueDate: new Date().toISOString().split('T')[0],
    });
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setFormData(prev => ({
      ...prev,
      [e.target.name]: e.target.value,
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

        <Input
          label="Due Date"
          name="dueDate"
          type="date"
          value={formData.dueDate}
          onChange={handleChange}
          required
        />

        <div className="flex space-x-3 pt-4">
          <Button
            type="submit"
            className="flex-1"
            isLoading={isLoading}
          >
            Create Todo
          </Button>
          <Button
            type="button"
            variant="secondary"
            onClick={onClose}
            className="flex-1"
          >
            Cancel
          </Button>
        </div>
      </form>
    </Modal>
  );
};