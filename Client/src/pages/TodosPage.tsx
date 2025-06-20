import React, { useEffect, useState } from 'react';
import { Layout } from '../components/layout/Layout';
import { TodoItem } from '../components/todos/TodoItem';
import { TodoForm } from '../components/todos/TodoForm';
import { Button } from '../components/common/Button';
import { Input } from '../components/common/Input';
import { apiService } from '../services/api';
import { TodoItem as TodoItemType, CreateTodoRequest } from '../types';
import { Plus, Search, Filter, Trash2 } from 'lucide-react';

export const TodosPage: React.FC = () => {
  const [todos, setTodos] = useState<TodoItemType[]>([]);
  const [filteredTodos, setFilteredTodos] = useState<TodoItemType[]>([]);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const [filterStatus, setFilterStatus] = useState<'all' | 'completed' | 'pending'>('all');
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    loadTodos();
  }, []);

  useEffect(() => {
    filterTodos();
  }, [todos, searchQuery, filterStatus]);

  const loadTodos = async () => {
    try {
      const data = await apiService.getAllTodos();
      setTodos(data);
    } catch (error) {
      console.error('Failed to load todos:', error);
    } finally {
      setIsLoading(false);
    }
  };

const handleMarkCompleted = async (id: number) => {
  try {
    await apiService.markTodoCompleted(id);
    // istasangiz: alert("Todo marked as completed")
    // yoki: todo listni yangilang
  } catch (error) {
    console.error("Failed to mark todo as completed:", error);
  }
};




  const filterTodos = () => {
    let filtered = todos;

    // Apply search filter
    if (searchQuery) {
      filtered = filtered.filter(todo =>
        todo.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
        todo.description.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    // Apply status filter
    if (filterStatus === 'completed') {
      filtered = filtered.filter(todo => todo.isCompleted);
    } else if (filterStatus === 'pending') {
      filtered = filtered.filter(todo => !todo.isCompleted);
    }

    setFilteredTodos(filtered);
  };

  const handleCreateTodo = async (todoData: CreateTodoRequest) => {
    try {
      await apiService.createTodo(todoData);
      setIsCreateModalOpen(false);
      loadTodos();
    } catch (error) {
      console.error('Failed to create todo:', error);
    }
  };

  const handleToggleComplete = async (id: number) => {
    try {
      await apiService.markTodoCompleted(id);
      loadTodos();
    } catch (error) {
      console.error('Failed to toggle todo completion:', error);
    }
  };

  const handleUpdateTodo = async (todo: TodoItemType) => {
    try {
      await apiService.updateTodo(todo);
      loadTodos();
    } catch (error) {
      console.error('Failed to update todo:', error);
    }
  };

  const handleDeleteTodo = async (id: number) => {
    try {
      await apiService.deleteTodo(id);
      loadTodos();
    } catch (error) {
      console.error('Failed to delete todo:', error);
    }
  };

  const handleDeleteAllCompleted = async () => {
    try {
      await apiService.deleteAllCompleted();
      loadTodos();
    } catch (error) {
      console.error('Failed to delete completed todos:', error);
    }
  };

  if (isLoading) {
    return (
      <Layout>
        <div className="flex items-center justify-center h-64">
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">My Todos</h1>
            <p className="text-gray-600 mt-1">{todos.length} total tasks</p>
          </div>
          <div className="mt-4 sm:mt-0 flex space-x-3">
            <Button
              variant="danger"
              icon={Trash2}
              onClick={handleDeleteAllCompleted}
              disabled={!todos.some(todo => todo.isCompleted)}
            >
              Clear Completed
            </Button>
            <Button icon={Plus} onClick={() => setIsCreateModalOpen(true)}>
              New Todo
            </Button>
          </div>
        </div>

        {/* Filters */}
        <div className="bg-white rounded-lg border border-gray-200 p-4">
          <div className="flex flex-col sm:flex-row sm:items-center space-y-4 sm:space-y-0 sm:space-x-4">
            <div className="flex-1">
              <Input
                icon={Search}
                placeholder="Search todos..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>
            
            <div className="flex space-x-2">
              <Button
                variant={filterStatus === 'all' ? 'primary' : 'ghost'}
                size="sm"
                onClick={() => setFilterStatus('all')}
              >
                All
              </Button>
              <Button
                variant={filterStatus === 'pending' ? 'primary' : 'ghost'}
                size="sm"
                onClick={() => setFilterStatus('pending')}
              >
                Pending
              </Button>
              <Button
                variant={filterStatus === 'completed' ? 'primary' : 'ghost'}
                size="sm"
                onClick={() => setFilterStatus('completed')}
              >
                Completed
              </Button>
            </div>
          </div>
        </div>

        {/* Todos List */}
        <div className="space-y-4">
          {filteredTodos.length > 0 ? (
            filteredTodos.map((todo) => (
              <TodoItem
                key={todo.toDoItemId}
                todo={todo}
                onToggleComplete={handleToggleComplete}
                onUpdate={handleUpdateTodo}
                onDelete={handleDeleteTodo}
              />
            ))
          ) : (
            <div className="text-center py-12 bg-white rounded-lg border border-gray-200">
              <Filter className="w-12 h-12 text-gray-400 mx-auto mb-4" />
              <h3 className="text-lg font-medium text-gray-900 mb-2">
                {searchQuery || filterStatus !== 'all' ? 'No matching todos' : 'No todos yet'}
              </h3>
              <p className="text-gray-600 mb-4">
                {searchQuery || filterStatus !== 'all' 
                  ? 'Try adjusting your search or filter criteria.'
                  : 'Get started by creating your first todo item.'
                }
              </p>
              {!searchQuery && filterStatus === 'all' && (
                <Button icon={Plus} onClick={() => setIsCreateModalOpen(true)}>
                  Create Todo
                </Button>
              )}
            </div>
          )}
        </div>

        <TodoForm
          isOpen={isCreateModalOpen}
          onClose={() => setIsCreateModalOpen(false)}
          onSubmit={handleCreateTodo}
        />
      </div>
    </Layout>
  );
};