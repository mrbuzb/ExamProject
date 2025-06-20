const API_BASE_URL = 'https://localhost:7166';

class ApiService {
  private getAuthHeaders(): HeadersInit {
    const token = localStorage.getItem('accessToken');
    return {
      'Content-Type': 'application/json',
      ...(token && { Authorization: `Bearer ${token}` }),
    };
  }

  private async request<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
    const url = `${API_BASE_URL}${endpoint}`;
    const response = await fetch(url, {
      ...options,
      headers: {
        ...this.getAuthHeaders(),
        ...options.headers,
      },
    });

    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'API request failed');
    }

    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
      return response.json();
    }
    
    return response.text() as T;
  }

  // Auth endpoints
  async login(credentials: { userName: string; password: string }) {
    return this.request<any>('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify(credentials),
    });
  }

  async register(userData: any) {
    return this.request<any>('/api/auth/sign-up', {
      method: 'POST',
      body: JSON.stringify(userData),
    });
  }

  async refreshToken(refreshToken: string, accessToken: string) {
    return this.request<any>('/api/auth/refresh-token', {
      method: 'PUT',
      body: JSON.stringify({ refreshToken, accessToken }),
    });
  }

  async logout(refreshToken: string) {
    return this.request<any>(`/api/auth/log-out?refreshToken=${refreshToken}`, {
      method: 'DELETE',
    });
  }

  // Todo endpoints
  async getAllTodos() {
    return this.request<any[]>('/get-all');
  }

  async getTodoById(id: number) {
    return this.request<any>(`/select-by-id?id=${id}`);
  }

  async createTodo(todo: any) {
    return this.request<any>('/create', {
      method: 'POST',
      body: JSON.stringify(todo),
    });
  }

  async updateTodo(todo: any) {
    return this.request<any>('/update', {
      method: 'PUT',
      body: JSON.stringify(todo),
    });
  }

  async deleteTodo(id: number) {
    return this.request<any>(`/delete?id=${id}`, {
      method: 'DELETE',
    });
  }

  async markTodoCompleted(id: number) {
    return this.request<any>(`/mark-as-compleated?id=${id}`, {
      method: 'PATCH',
    });
  }

  async searchTodos(keyword: string) {
    return this.request<any[]>(`/search?keyword=${encodeURIComponent(keyword)}`);
  }

  async filterTodosByDate(dueDate: string) {
    return this.request<any[]>(`/filter-by-date?dueDate=${dueDate}`);
  }

  async getOverdueTodos() {
    return this.request<any[]>('/to-do-item/overdue');
  }

  async getTodoSummary() {
    return this.request<any>('/get-summary');
  }

  async getTodoCount() {
    return this.request<number>('/count');
  }

  async deleteAllCompleted() {
    return this.request<any>('/delete-all-completed', {
      method: 'DELETE',
    });
  }

  async setTodoDueDate(id: number, dueDate: string) {
    return this.request<any>(`/set-due-date?id=${id}&dueDate=${dueDate}`, {
      method: 'PUT',
    });
  }

  // Admin endpoints
  async getAllUsersByRole(role: string) {
    return this.request<any[]>(`/api/admin/get-all-users-by-role?role=${role}`);
  }

  async deleteUser(userId: number) {
    return this.request<any>(`/api/admin/delete-user-by-id?userId=${userId}`, {
      method: 'DELETE',
    });
  }

  async updateUserRole(userId: number, userRole: string) {
    return this.request<any>(`/api/admin/update-user-role?userId=${userId}&userRole=${userRole}`, {
      method: 'PATCH',
    });
  }

  // Role endpoints
  async getAllRoles() {
    return this.request<any[]>('/api/role/get-all-roles');
  }
}

export const apiService = new ApiService();