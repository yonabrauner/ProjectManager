import { useState } from "react";
import "./addProjectForm.styles.css";
import { createProject } from "../../services/projectService"; // adjust path if needed
import type { Project, ProjectCreateDto } from "../../types/project";

interface AddProjectFormProps {
    setProjects: React.Dispatch<React.SetStateAction<Project[]>>;
}

export default function AddProjectForm({ setProjects }: AddProjectFormProps) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!title.trim()) {
      setError("Title is required");
      return;
    }

    setError(null);
    setLoading(true);

    
    try {
      const dto: ProjectCreateDto = { title, description }
      const response: Project = await createProject(dto);
      if (!response){
        console.error("no response from server on createProject api call.");
      }
      else{
        setProjects((prev) => [...prev, response]);
      }
      setTitle("");
      setDescription("");
    } catch (err) {
      console.error("Error creating project:", err);
      setError("Failed to create project. Try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form className="add-project-form" onSubmit={handleSubmit}>
      <h2>Add New Project</h2>

      <div className="form-group">
        <label htmlFor="title">Title *</label>
        <input
          id="title"
          type="text"
          placeholder="Enter project title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="description">Description (optional)</label>
        <textarea
          id="description"
          placeholder="Enter a short description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
      </div>

      {error && <p className="error-message">{error}</p>}

      <button type="submit" disabled={loading || title.trim().length < 3 || title.length > 100 || description.length > 500}>
        {loading ? "Adding..." : "Add Project"}
      </button>
    </form>
  );
}
