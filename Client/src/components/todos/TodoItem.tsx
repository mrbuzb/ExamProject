import React, { useState } from 'react';
import { TodoItem as TodoItemType } from '../../types';
import { Button } from '../common/Button';
import { Modal } from '../common/Modal';
import { Input } from '../common/Input';
import { format, isAfter } from 'date-fns';
import {
  Check,
  Edit,
  Trash2,
  Calendar,
  Clock,
  AlertTriangle,
} from 'lucide-react';

interface TodoItemProps {
  todo: TodoItemType;
  onToggleComplete: (id: number) => void;
  onUpdate: (todo: TodoItemType) => void;
  onDelete: (id: number) => void;
}

export const TodoItem: React.FC<TodoItemProps> = ({
  todo,
  onToggleComplete,
  onUpdate,
  onDelete,
}) => {
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [editForm, setEditForm] = useState({
    title: todo.title,
    description: todo.description,
    dueDate: todo.dueDate.split('T')[0],
  });

  const isOverdue = isAfter(new Date(), new Date(todo.dueDate)) && !todo.isCompleted;
  const dueDate = format(new Date(todo.dueDate), 'MMM dd, yyyy');

  const handleUpdate = () => {
    onUpdate({
      ...todo,
      title: editForm.title,
      description: editForm.description,
      dueDate: new Date(editForm.dueDate).toISOString(),
    });
    setIsEditModalOpen(false);
  };

  const handleDelete = () => {
    onDelete(todo.toDoItemId);
    setIsDeleteModalOpen(false);
  };

  return (
    <>
      <div className={`bg-white rounded-lg border-2 p-4 shadow-sm hover:shadow-md transition-all duration-200 ${
        todo.isCompleted ? 'border-emerald-200 bg-emerald-50' : 
        isOverdue ? 'border-red-200 bg-red-50' : 'border-gray-200'
      }`}>
        <div className="flex items-start space-x-3">
          <button
            onClick={() => onToggleComplete(todo.toDoItemId)}
            className={`mt-0.5 w-5 h-5 rounded-full border-2 flex items-center justify-center transition-all duration-200 ${
              todo.isCompleted
                ? 'bg-emerald-500 border-emerald-500 text-white'
                : 'border-gray-300 hover:border-emerald-500'
            }`}
          >
            {todo.isCompleted && <Check className="w-3 h-3" />}
          </button>

          <div className="flex-1">
            <h3 className={`font-semibold ${
              todo.isCompleted ? 'text-gray-500 line-through' : 'text-gray-900'
            }`}>
              {todo.title}
            </h3>
            {todo.description && (
              <p className={`text-sm mt-1 ${
                todo.isCompleted ? 'text-gray-400' : 'text-gray-600'
              }`}>
                {todo.description}
              </p>
            )}

            <div className="flex items-center space-x-4 mt-3">
              <div className={`flex items-center space-x-1 text-xs ${
                isOverdue ? 'text-red-600' : 'text-gray-500'
              }`}>
                {isOverdue ? <AlertTriangle className="w-3 h-3" /> : <Calendar className="w-3 h-3" />}
                <span>{dueDate}</span>
              </div>

              <div className="flex items-center space-x-1 text-xs text-gray-500">
                <Clock className="w-3 h-3" />
                <span>Created {format(new Date(todo.createdAt), 'MMM dd')}</span>
              </div>
            </div>

            {/* ✅ Mark as Completed tugmasi */}
            {!todo.isCompleted && (
              <div className="mt-3">
                <Button
                  onClick={() => onToggleComplete(todo.toDoItemId)}
                  className="text-sm bg-emerald-500 text-white px-3 py-1 rounded hover:bg-emerald-600 transition"
                >
                  Mark as Completed
                </Button>
              </div>
            )}
          </div>

          <div className="flex items-center space-x-1">
            <Button
              variant="ghost"
              size="sm"
              icon={Edit}
              onClick={() => setIsEditModalOpen(true)}
            />
            <Button
              variant="ghost"
              size="sm"
              icon={Trash2}
              onClick={() => setIsDeleteModalOpen(true)}
            />
          </div>
        </div>
      </div>

      <Modal
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        title="Edit Todo"
        maxWidth="lg"
      >
        <div className="space-y-4">
          <Input
            label="Title"
            value={editForm.title}
            onChange={(e) => setEditForm(prev => ({ ...prev, title: e.target.value }))}
            placeholder="Enter todo title"
          />

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Description
            </label>
            <textarea
              value={editForm.description}
              onChange={(e) => setEditForm(prev => ({ ...prev, description: e.target.value }))}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              rows={3}
              placeholder="Enter description (optional)"
            />
          </div>

          <Input
            label="Due Date"
            type="date"
            value={editForm.dueDate}
            onChange={(e) => setEditForm(prev => ({ ...prev, dueDate: e.target.value }))}
          />

          <div className="flex space-x-3 pt-4">
            <Button onClick={handleUpdate} className="flex-1">
              Save Changes
            </Button>
            <Button
              variant="secondary"
              onClick={() => setIsEditModalOpen(false)}
              className="flex-1"
            >
              Cancel
            </Button>
          </div>
        </div>
      </Modal>

      <Modal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        title="Delete Todo"
      >
        <div className="space-y-4">
          <p className="text-gray-600">
            Are you sure you want to delete "{todo.title}"? This action cannot be undone.
          </p>
          <div className="flex space-x-3">
            <Button
              variant="danger"
              onClick={handleDelete}
              className="flex-1"
            >
              Delete
            </Button>
            <Button
              variant="secondary"
              onClick={() => setIsDeleteModalOpen(false)}
              className="flex-1"
            >
              Cancel
            </Button>
          </div>
        </div>
      </Modal>
    </>
  );
};
