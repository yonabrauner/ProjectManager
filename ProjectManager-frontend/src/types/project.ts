export interface Project {
  id: string;
  title: string;
  description?: string;
  createdAt: string;
}

export interface ProjectCreateDto {
  title: string;
  description?: string;
}
