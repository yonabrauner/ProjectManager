export interface Task {
  id: string;
  title: string;
  isCompleted: boolean;
  dueDate: string;
  projectId: string;
}

export interface TaskCreateDto {
  title: string;
  dueDate?: string;
  projectId: string;
}

export interface TaskUpdateDto {
  title?: string;
  isCompleted?: boolean;
  dueDate?: string;
}
