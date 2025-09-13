import React from "react";
import "./projectItem.styles.css";

interface ProjectItemProps {
  title: string;
  description?: string;
  createdAt: string; // ISO string from backend
  onClick: () => void;
}

const ProjectItem: React.FC<ProjectItemProps> = ({ title, description, createdAt, onClick }) => {
  const formattedDate = new Date(createdAt).toLocaleDateString();

  return (
    <div className="project-card" onClick={onClick}>
      <h3 className="project-title">{title}</h3>
      {description && <p className="project-description">{description}</p>}
      <div className="project-footer">
        <small className="project-date">Created: {formattedDate}</small>
      </div>
    </div>
  );
};

export default ProjectItem;
