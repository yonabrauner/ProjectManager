import type { Task } from "../../types/projectTask"; // adjust path
import TaskRow from "../taskRow/taskRow.component"; // row component for each task
import "./tasksContainer.styles.css";

interface TasksContainerProps {
  projectId: string;
  tasks: Task[];
  loading: boolean;
  onDelete: (taskId: string) => void;
  refreshTasks: () => void;
}

export default function TasksContainer({ tasks, loading, onDelete, refreshTasks }: TasksContainerProps) { 
    console.log(tasks);
  if (loading) return <p className="loading">Loading tasks...</p>;
  
  return (
    <div className="tasks-container">
      {tasks.length ? tasks.map((task) => (
        <TaskRow key={task.id} {...task} onDelete={onDelete} refreshTasks={refreshTasks}/>
      )) : <p className="empty">No tasks yet.</p>}
    </div>
  );
}
