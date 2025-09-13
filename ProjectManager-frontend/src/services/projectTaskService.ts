import api from "../api/axiosInstance";
import type { Task, TaskCreateDto, TaskUpdateDto } from "../types/projectTask";

// Get all tasks in a project (with optional filters/sorting)
export async function getTasksByProject(
  projectId: string,
  completed?: boolean,
  sort?: string
): Promise<Task[]> {
  const response = await api.get<Task[]>(
    `/projects/${projectId}/tasks`,
    { params: { completed, sort } }
  );
  return response.data;
}

// Get a single task
export async function getTaskById(
  projectId: string,
  taskId: number
): Promise<Task> {
  const response = await api.get<Task>(`/projects/${projectId}/tasks/${taskId}`);
  return response.data;
}

// Create a new task
export async function createTask(
  projectId: string,
  dto: TaskCreateDto
): Promise<Task> {
  const response = await api.post<Task>(`/projects/${projectId}/tasks`, dto);
  return response.data;
}

// Update a task
export async function updateTask(
  projectId: string,
  taskId: string,
  dto: TaskUpdateDto
): Promise<Task> {
  const response = await api.put<Task>(`/projects/${projectId}/tasks/${taskId}`, dto);
  return response.data;
}

// Delete a task
export async function deleteTask(
  projectId: string,
  taskId: string
): Promise<void> {
  await api.delete(`/projects/${projectId}/tasks/${taskId}`);
}
