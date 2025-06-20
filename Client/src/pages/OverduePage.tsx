import React, { useEffect, useState } from 'react';
import { Layout } from '../components/layout/Layout';
import { apiService } from '../services/api';
import { TodoItem as Todo } from '../types';
import { Button } from '../components/common/Button';
import { format } from 'date-fns';
import { AlertTriangle, Check, Trash2 } from 'lucide-react';

export const OverduePage: React.FC = () => {
  const [overdueTodos, setOverdueTodos] = useState<Todo[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    loadOverdueTodos();
  }, []);

  const loadOverdueTodos = async () => {
    setIsLoading(true);
    try {
      const data = await apiService.getOverdueTodos(); // ⚠️ Mana shu metod apiService'da bo'lishi kerak
      setOverdueTodos(data);
    } catch (error) {
      console.error('Failed to load overdue todos', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleMarkCompleted = async (id: number) => {
    try {
      await apiService.markTodoCompleted(id);
      loadOverdueTodos();
    } catch (error) {
      console.error('Error marking todo as completed', error);
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await apiService.deleteTodo(id);
      loadOverdueTodos();
    } catch (error) {
      console.error('Error deleting todo', error);
    }
  };

  return (
    <Layout>
      <div className="space-y-6">
        <div className="flex items-center space-x-2">
          <AlertTriangle className="text-red-500 w-6 h-6" />
          <h1 className="text-2xl font-bold text-gray-800">Overdue Tasks</h1>
        </div>

        {isLoading ? (
          <div className="flex justify-center items-center h-40">
            <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-blue-600" />
          </div>
        ) : overdueTodos.length === 0 ? (
          <div className="text-center py-12 bg-white rounded-lg border border-gray-200">
            <p className="text-gray-600 text-lg">You have no overdue tasks 🎉</p>
          </div>
        ) : (
          <div className="space-y-4">
            {overdueTodos.map(todo => (
              <div
                key={todo.toDoItemId}
                className="bg-red-50 border border-red-200 rounded-lg p-4 flex justify-between items-start shadow-sm"
              >
                <div>
                  <h3 className="font-semibold text-gray-900">{todo.title}</h3>
                  {todo.description && (
                    <p className="text-sm text-gray-600 mt-1">{todo.description}</p>
                  )}
                  <p className="text-xs text-red-600 mt-2">
                    Due: {format(new Date(todo.dueDate), 'PPP')}
                  </p>
                </div>
                <div className="flex space-x-2">
                  <Button
                    size="sm"
                    variant="primary"
                    icon={Check}
                    onClick={() => handleMarkCompleted(todo.toDoItemId)}
                  >
                    Complete
                  </Button>
                  <Button
                    size="sm"
                    variant="danger"
                    icon={Trash2}
                    onClick={() => handleDelete(todo.toDoItemId)}
                  >
                    Delete
                  </Button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </Layout>
  );
};
