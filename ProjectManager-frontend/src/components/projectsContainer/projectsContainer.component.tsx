import ProjectItem from "../projectItem/projectItem.component";
import type { Project } from "../../types/project";
import "./projectsContainer.styles.css";

interface ProjectsContainerProps {
    projects: Project[];
    loading: boolean;
    onProjectClick: (project: Project) => void;
}

export default function ProjectsContainer({ projects, loading, onProjectClick }: ProjectsContainerProps) {

  if (loading) return <p className="loading">Loading projects...</p>;
  if (projects.length === 0) return <p className="empty">You have no projects yet.</p>;

  return (
    <div className="projects-container">
      {projects.map((p) => (
        <ProjectItem
          key={p.id}
          {...p}
          onClick={() => onProjectClick(p)}
        />
      ))}
    </div>
  );
}
