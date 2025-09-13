import api from "../api/axiosInstance"; // your axios instance
import type { Project, ProjectCreateDto } from "../types/project";

// Get all projects
export async function getProjects(): Promise<Project[]> {
  const response = await api.get<Project[]>("/projects");
  return response.data;
}

// Get project by id
export async function getProjectById(id: string): Promise<Project> {
  const response = await api.get<Project>(`/projects/${id}`);
  return response.data;
}

// Create a new project
export async function createProject(dto: ProjectCreateDto): Promise<Project> {
  console.log("sending POST createProject to api: " + dto.title);
  const response = await api.post<Project>("/projects", dto);
  return response.data;
}

// Delete a project
export async function deleteProject(id: string): Promise<void> {
  await api.delete(`/projects/${id}`);
}
