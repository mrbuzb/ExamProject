import React, { useEffect, useState } from 'react';
import { Layout } from '../components/layout/Layout';
import { SummaryCard } from '../components/dashboard/SummaryCard';
import { Button } from '../components/common/Button';
import { TodoForm } from '../components/todos/TodoForm';
import { TodoItem } from '../components/todos/TodoItem';
import { apiService } from '../services/api';
import { TodoItem as TodoItemType, TodoSummary, CreateTodoRequest } from '../types';
import {
  CheckSquare,
  Clock,
  AlertTriangle,
  BarChart3,
  Plus,
  Filter,
  Search,
} from 'lucide-react';

export const DashboardPage: React.FC = () => {
  const [summary, setSummary] = useState<TodoSummary | null>(null);
  const [recentTodos, setRecentTodos] = useState<TodoItemType[]>([]);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      const [summaryData, todosData] = await Promise.all([
        apiService.getTodoSummary(),
        apiService.getAllTodos(),
      ]);
      
      setSummary(summaryData);
      setRecentTodos(todosData.slice(0, 5)); // Show 5 most recent todos
    } catch (error) {
      console.error('Failed to load dashboard data:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCreateTodo = async (todoData: CreateTodoRequest) => {
    try {
      await apiService.createTodo(todoData);
      setIsCreateModalOpen(false);
      loadDashboardData();
    } catch (error) {
      console.error('Failed to create todo:', error);
    }
  };

  const handleToggleComplete = async (id: number) => {
    try {
      await apiService.markTodoCompleted(id);
      loadDashboardData();
    } catch (error) {
      console.error('Failed to toggle todo completion:', error);
    }
  };

  const handleUpdateTodo = async (todo: TodoItemType) => {
    try {
      await apiService.updateTodo(todo);
      loadDashboardData();
    } catch (error) {
      console.error('Failed to update todo:', error);
    }
  };

  const handleDeleteTodo = async (id: number) => {
    try {
      await apiService.deleteTodo(id);
      loadDashboardData();
    } catch (error) {
      console.error('Failed to delete todo:', error);
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
      <div className="space-y-8">
        {/* Header */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
            <p className="text-gray-600 mt-1">Welcome back! Here's your productivity overview.</p>
          </div>
          <div className="mt-4 sm:mt-0 flex space-x-3">
            <Button variant="secondary" icon={Filter}>
              Filter
            </Button>
            <Button variant="secondary" icon={Search}>
              Search
            </Button>
            <Button icon={Plus} onClick={() => setIsCreateModalOpen(true)}>
              New Todo
            </Button>
          </div>
        </div>

        {/* Summary Cards */}
        {summary && (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
            <SummaryCard
              title="Total Tasks"
              value={summary.total}
              icon={BarChart3}
              color="blue"
              trend={{ value: 12, isPositive: true }}
            />
            <SummaryCard
              title="Completed"
              value={summary.completed}
              icon={CheckSquare}
              color="green"
              trend={{ value: 8, isPositive: true }}
            />
            <SummaryCard
              title="In Progress"
              value={summary.incompleted}
              icon={Clock}
              color="yellow"
              trend={{ value: 3, isPositive: false }}
            />
            <SummaryCard
              title="Overdue"
              value={summary.overdue}
              icon={AlertTriangle}
              color="red"
              trend={{ value: 5, isPositive: false }}
            />
          </div>
        )}

        {/* Recent Todos */}
        <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
          <div className="px-6 py-4 border-b border-gray-200">
            <div className="flex items-center justify-between">
              <h2 className="text-lg font-semibold text-gray-900">Recent Todos</h2>
              <Button variant="ghost" size="sm">
                View All
              </Button>
            </div>
          </div>
          
          <div className="p-6">
            {recentTodos.length > 0 ? (
              <div className="space-y-4">
                {recentTodos.map((todo) => (
                  <TodoItem
                    key={todo.toDoItemId}
                    todo={todo}
                    onToggleComplete={handleToggleComplete}
                    onUpdate={handleUpdateTodo}
                    onDelete={handleDeleteTodo}
                  />
                ))}
              </div>
            ) : (
              <div className="text-center py-12">
                <CheckSquare className="w-12 h-12 text-gray-400 mx-auto mb-4" />
                <h3 className="text-lg font-medium text-gray-900 mb-2">No todos yet</h3>
                <p className="text-gray-600 mb-4">Get started by creating your first todo item.</p>
                <Button icon={Plus} onClick={() => setIsCreateModalOpen(true)}>
                  Create Todo
                </Button>
              </div>
            )}
          </div>
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