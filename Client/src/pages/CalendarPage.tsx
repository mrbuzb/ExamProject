import React, { useEffect, useState } from 'react';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import { apiService } from '../services/api';
import { TodoItem as Todo } from '../types';
import { Layout } from '../components/layout/Layout';
import { CheckCircle, X, Pencil, Save } from 'lucide-react';

const formatDate = (date: Date) => {
  return date.toLocaleDateString('sv-SE');
};

export const CalendarPage: React.FC = () => {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [selectedDate, setSelectedDate] = useState<Date>(new Date());
  const [selectedTodo, setSelectedTodo] = useState<Todo | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [editMode, setEditMode] = useState(false);

  // Editable fields
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [dueDate, setDueDate] = useState('');

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
    const dateStr = formatDate(date);
    return todos.filter(todo => formatDate(new Date(todo.dueDate)) === dateStr);
  };

  const openModal = (todo: Todo) => {
    setSelectedTodo(todo);
    setTitle(todo.title);
    setDescription(todo.description || '');
    setDueDate(todo.dueDate.slice(0, 10)); // YYYY-MM-DD
    setShowModal(true);
    setEditMode(false);
  };

  const toggleComplete = async (todo: Todo) => {
    try {
      await apiService.markTodoCompleted(todo.toDoItemId);
      await loadTodos();
      setShowModal(false);
    } catch (error) {
      console.error('Failed to complete todo:', error);
    }
  };

  const deleteTodo = async (id: string) => {
    try {
      await apiService.deleteTodo(id);
      await loadTodos();
      setShowModal(false);
    } catch (error) {
      console.error('Failed to delete todo:', error);
    }
  };

  const updateTodo = async () => {
    try {
      if (!selectedTodo) return;
      await apiService.updateTodo({
        toDoItemId: selectedTodo.toDoItemId,
        title,
        description,
        dueDate: new Date(dueDate).toISOString(),
        isCompleted: selectedTodo.isCompleted,
      });
      await loadTodos();
      setShowModal(false);
    } catch (error) {
      console.error('Failed to update todo:', error);
    }
  };

  return (
    <Layout>
      <div className="max-w-5xl mx-auto px-4 py-8 space-y-8">
        {/* Calendar */}
        <div className="bg-white rounded-xl shadow-sm p-6 border">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">Calendar Overview</h1>
          <p className="text-gray-600 mb-6">
            See all your tasks by their due dates. Click a task to view/edit.
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

        {/* Task List */}
        <div className="bg-white rounded-xl shadow-sm p-6 border">
          <h2 className="text-xl font-semibold text-gray-800 mb-3">
            Tasks for {selectedDate.toLocaleDateString()}
          </h2>
          {getTodosForDate(selectedDate).length > 0 ? (
            <ul className="space-y-2">
              {getTodosForDate(selectedDate).map((todo) => (
                <li
                  key={todo.toDoItemId}
                  className="flex justify-between items-center bg-gray-50 rounded-lg px-4 py-2 border hover:shadow transition cursor-pointer"
                  onClick={() => openModal(todo)}
                >
                  <span className={`text-gray-800 ${todo.isCompleted ? 'line-through text-gray-400' : ''}`}>
                    {todo.title}
                  </span>
                  {todo.isCompleted && <CheckCircle className="w-5 h-5 text-green-600" />}
                </li>
              ))}
            </ul>
          ) : (
            <p className="text-gray-500 italic">No tasks due on this date.</p>
          )}
        </div>
      </div>

      {/* Modal */}
      {showModal && selectedTodo && (
        <div className="fixed inset-0 bg-black bg-opacity-40 flex justify-center items-center z-50">
          <div className="bg-white p-6 rounded-lg w-full max-w-md relative">
            <button
              className="absolute top-2 right-2 text-gray-500 hover:text-gray-800"
              onClick={() => setShowModal(false)}
            >
              <X />
            </button>

            {editMode ? (
              <>
                <h3 className="text-xl font-semibold mb-4">Edit Task</h3>
                <input
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                  className="w-full border rounded px-3 py-2 mb-2"
                  placeholder="Title"
                />
                <textarea
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  className="w-full border rounded px-3 py-2 mb-2"
                  placeholder="Description"
                />
                <input
                  type="date"
                  value={dueDate}
                  onChange={(e) => setDueDate(e.target.value)}
                  className="w-full border rounded px-3 py-2 mb-4"
                />
                <div className="flex justify-end gap-2">
                  <button
                    onClick={updateTodo}
                    className="flex items-center gap-1 px-3 py-1 rounded bg-blue-600 text-white hover:bg-blue-700"
                  >
                    Save <Save size={16} />
                  </button>
                  <button
                    onClick={() => setEditMode(false)}
                    className="px-3 py-1 rounded border text-gray-600 hover:bg-gray-100"
                  >
                    Cancel
                  </button>
                </div>
              </>
            ) : (
              <>
                <h3 className="text-xl font-semibold mb-2">{selectedTodo.title}</h3>
                <p className="text-gray-600 mb-2">{selectedTodo.description || 'No description'}</p>
                <p className="text-sm mb-2">
                  <strong>Due:</strong> {new Date(selectedTodo.dueDate).toLocaleDateString()}
                </p>
                <p className="text-sm mb-4">
                  <strong>Status:</strong>{' '}
                  {selectedTodo.isCompleted ? (
                    <span className="text-green-600 font-medium">Completed</span>
                  ) : (
                    <span className="text-yellow-600 font-medium">Pending</span>
                  )}
                </p>
                <div className="flex justify-end gap-2">
                  <button
                    className="flex items-center gap-1 px-3 py-1 rounded bg-green-600 text-white hover:bg-green-700"
                    onClick={() => toggleComplete(selectedTodo)}
                  >
                    {selectedTodo.isCompleted ? 'Undo' : 'Complete'} <CheckCircle size={16} />
                  </button>
                  <button
                    className="flex items-center gap-1 px-3 py-1 rounded bg-blue-600 text-white hover:bg-blue-700"
                    onClick={() => setEditMode(true)}
                  >
                    Edit <Pencil size={16} />
                  </button>
                  <button
                    className="flex items-center gap-1 px-3 py-1 rounded bg-red-600 text-white hover:bg-red-700"
                    onClick={() => deleteTodo(selectedTodo.toDoItemId)}
                  >
                    Delete <X size={16} />
                  </button>
                </div>
              </>
            )}
          </div>
        </div>
      )}
    </Layout>
  );
};
