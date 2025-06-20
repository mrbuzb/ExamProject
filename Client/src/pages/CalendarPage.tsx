import React, { useEffect, useState } from 'react';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import { apiService } from '../services/api';
import { TodoItem as Todo } from '../types';
import { Layout } from '../components/layout/Layout';

export const CalendarPage: React.FC = () => {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [selectedDate, setSelectedDate] = useState<Date>(new Date());

  useEffect(() => {
    loadTodos();
  }, []);

  const loadTodos = async () => {
    try {
      const data = await apiService.getAllTodos();
      setTodos(data);
    } catch (error) {
      console.error('Failed to load todos:', error);
    }
  };

  const getTodosForDate = (date: Date) => {
    const dateStr = date.toISOString().split('T')[0];
    return todos.filter(todo => todo.dueDate.startsWith(dateStr));
  };

  return (
    <Layout>
      <div className="max-w-5xl mx-auto px-4 py-8 space-y-8">
        <div className="bg-white rounded-xl shadow-sm p-6 border">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">Calendar Overview</h1>
          <p className="text-gray-600 mb-6">
            See all your tasks by their due dates. Click a date to view detailed tasks.
          </p>
          <Calendar
            value={selectedDate}
            onChange={(date) => setSelectedDate(date as Date)}
            tileContent={({ date }) => {
              const dayTodos = getTodosForDate(date);
              return dayTodos.length > 0 ? (
                <div className="mt-1 text-xs text-emerald-600 font-semibold text-center">
                  • {dayTodos.length}
                </div>
              ) : null;
            }}
            className="mx-auto"
          />
        </div>

        <div className="bg-white rounded-xl shadow-sm p-6 border">
          <h2 className="text-xl font-semibold text-gray-800 mb-3">
            Tasks for {selectedDate.toLocaleDateString()}
          </h2>
          {getTodosForDate(selectedDate).length > 0 ? (
            <ul className="space-y-2">
              {getTodosForDate(selectedDate).map((todo) => (
                <li
                  key={todo.toDoItemId}
                  className="flex justify-between items-center bg-gray-50 rounded-lg px-4 py-2 border hover:shadow transition"
                >
                  <span className={`text-gray-800 ${todo.isCompleted ? 'line-through text-gray-400' : ''}`}>
                    {todo.title}
                  </span>
                  {todo.isCompleted && <span className="text-emerald-600 text-sm">✔️</span>}
                </li>
              ))}
            </ul>
          ) : (
            <p className="text-gray-500 italic">No tasks due on this date.</p>
          )}
        </div>
      </div>
    </Layout>
  );
};
