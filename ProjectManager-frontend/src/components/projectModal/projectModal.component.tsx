import { useEffect, useState } from "react";
import type { Project } from "../../types/project";
import "./projectModal.styles.css";
import AddTaskForm from "../addTaskForm/addTaskForm.component";
import { getTasksByProject } from "../../services/projectTaskService"; 
import type { Task } from "../../types/projectTask";
import TasksContainer from "../tasksContainer/tasksContainer.component";
import { deleteTask } from "../../services/projectTaskService";
import { deleteProject } from "../../services/projectService";

interface ProjectModalProps {
  project: Project | null; // null means closed
  onClose: () => void;
}

export default function ProjectModal({ project, onClose }: ProjectModalProps) {
  if (!project) return null; // modal hidden
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [filterCompleted, setFilterCompleted] = useState<boolean | null>(null);
  const [sortBy, setSortBy] = useState<string | null>(null);
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);


  const fetchTasks = async () => {
    setLoading(true);
    try {
      const res = await getTasksByProject(project.id, filterCompleted ?? undefined, sortBy ?? undefined);
      setTasks(res);
    } catch (err) {
      console.error("Failed to fetch tasks:", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTasks();
  }, [project.id, filterCompleted, sortBy]); // refetch on filter/sort change

  const handleFilterToggle = () => {
    // cycle between null -> true -> false -> null
    setFilterCompleted(prev => prev === null ? true : prev === true ? false : null);
  };

  const onTaskDelete: (taskId: number) => void = async (taskId) => {
      await deleteTask(project.id, taskId);
      setTasks((prev) => prev.filter((t) => t.id !== taskId));
    }

  const handleDeleteProject = async () => {
    if (!project) return;
    await deleteProject(project.id);
    onClose(); // close modal after delete
  };


  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        {/* Delete Project Button */}
        <div 
          className="delete-project-btn" 
          onClick={() => setShowDeleteConfirm(true)}
        >
          🗑️ Delete Project
        </div>

        <h2 className="title">{project.title}</h2>
        <p>{project.description || "No description provided."}</p>

        <div>
          <strong>Created at:</strong>{" "}
          {new Date(project.createdAt).toLocaleString()}

          <AddTaskForm projectId={project.id} setTasks={setTasks} />

          {/* Filter & Sort Buttons */}
          <div className="task-controls">

            <button onClick={handleFilterToggle}>
              Filter: {filterCompleted === null ? "All" : filterCompleted ? "Completed" : "Uncompleted"}
            </button>

            <select
              value={sortBy ?? ""}
              onChange={(e) => setSortBy(e.target.value || null)}
            >
              <option value="">Sorting</option>
              <option value="title_asc">Title ↑</option>
              <option value="title_desc">Title ↓</option>
              <option value="duedate_asc">Date ↑</option>
              <option value="duedate_desc">Date ↓</option>
            </select>
          </div>

          <TasksContainer tasks={tasks} projectId={project.id} loading={loading} onDelete={onTaskDelete} refreshTasks={fetchTasks}/>

          {/* Confirm Delete Modal */}
            {showDeleteConfirm && (
            <div className="confirm-modal-overlay" onClick={() => setShowDeleteConfirm(false)}>
                <div className="confirm-modal" onClick={e => e.stopPropagation()}>
                <p>Are you sure you want to delete this project?</p>
                <div className="confirm-buttons">
                    <button onClick={handleDeleteProject} className="confirm-yes">Yes</button>
                    <button onClick={() => setShowDeleteConfirm(false)} className="confirm-no">No</button>
                </div>
                </div>
            </div>
            )}
        </div>
      </div>
    </div>
  );
}
