import React, { useState } from 'react';
import { Input } from '../components/common/Input';
import { Button } from '../components/common/Button';
import { Layout } from '../components/layout/Layout';
import { TodoItem as TodoItemType } from '../types';
import { apiService } from '../services/api';
import { Search } from 'lucide-react';
import { TodoItem } from '../components/todos/TodoItem';

export const SearchPage: React.FC = () => {
  const [keyword, setKeyword] = useState('');
  const [results, setResults] = useState<TodoItemType[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [searched, setSearched] = useState(false);

  const handleSearch = async () => {
    if (!keyword.trim()) return;

    setIsLoading(true);
    setSearched(true);
    try {
      const data = await apiService.searchTodos(keyword);
      setResults(data);
    } catch (error) {
      console.error('Search failed:', error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Layout>
      <div className="max-w-2xl mx-auto space-y-6">
        <h1 className="text-3xl font-bold text-center text-gray-900">Search Todos</h1>

        <div className="flex space-x-3">
          <Input
            icon={Search}
            placeholder="Enter keyword..."
            value={keyword}
            onChange={(e) => setKeyword(e.target.value)}
            onKeyDown={(e) => e.key === 'Enter' && handleSearch()}
          />
          <Button onClick={handleSearch}>Search</Button>
        </div>

        {isLoading ? (
          <div className="text-center mt-10">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600 mx-auto"></div>
          </div>
        ) : (
          <>
            {searched && results.length === 0 ? (
              <div className="text-center text-gray-500 mt-6">No todos found.</div>
            ) : (
              <div className="space-y-4">
                {results.map((todo) => (
                  <TodoItem
                    key={todo.toDoItemId}
                    todo={todo}
                    onToggleComplete={() => {}}
                    onUpdate={() => {}}
                    onDelete={() => {}}
                  />
                ))}
              </div>
            )}
          </>
        )}
      </div>
    </Layout>
  );
};
