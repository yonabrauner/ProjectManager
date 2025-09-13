import { useAuth } from "../../contexts/AuthContext";
import { useEffect, useState } from "react";
import type { Project } from "../../types/project";
import "./home.styles.css";
import AddProjectForm from "../../components/addProjectForm/addProjectForm.component";
import ProjectsContainer from "../../components/projectsContainer/projectsContainer.component";
import { getProjects } from "../../services/projectService";
import ProjectModal from "../../components/projectModal/projectModal.component";

export default function HomePage() {
  const { isLoggedIn, username } = useAuth();
  const [loading, setLoading] = useState(true);
  const [projects, setProjects] = useState<Project[]>([]);
  const [selectedProject, setSelectedProject] = useState<Project | null>(null);


//   useEffect(() => {
//     const checkAndFetch = async () => {
//       const valid = await validateToken();
//       if (!valid) return;

//       const res = await axiosInstance.get<Project[]>("/api/projects");
//       setProjects(res.data);
//     };

//     checkAndFetch();
//   }, [validateToken]);

  const fetchProjects = async () => {
      try {
        const res = await getProjects();
        setProjects(res);
      } catch (err) {
        console.error("Failed to fetch projects:", err);
      } finally {
        setLoading(false);
      }
    };

  useEffect(() => {
    fetchProjects();
  }, []);

  if (!isLoggedIn) return <p>Please log in</p>;

  return (
    <div className="home-container">
      <h1 className="page-title">Hello {username}!</h1>
      <h2 className="projects-title">Your Projects</h2>
      <AddProjectForm setProjects={setProjects} />
      <ProjectsContainer
        projects={projects}
        loading={loading}
        onProjectClick={setSelectedProject}
      />
      <ProjectModal
        project={selectedProject}
        onClose={() => {setSelectedProject(null); fetchProjects()}}
      />
    </div>
  );
};