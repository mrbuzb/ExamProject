export interface User {
  userId: number;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phoneNumber: string;
  role: string;
}

export interface TodoItem {
  toDoItemId: number;
  userId: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string;
  dueDate: string;
}

export interface TodoSummary {
  total: number;
  completed: number;
  incompleted: number;
  overdue: number;
}

export interface Role {
  id: number;
  name: string;
  description: string;
}

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  userName: string;
  password: string;
  email: string;
  phoneNumber: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  tokenType: string;
  expires: number;
}

export interface CreateTodoRequest {
  title: string;
  description: string;
  dueDate: string;
}

export interface UpdateTodoRequest {
  toDoItemId: number;
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate: string;
}