import { useState } from "react";
import type { Task, TaskCreateDto } from "../../types/projectTask";
import { createTask } from "../../services/projectTaskService"; // your API call
import "./addTaskForm.styles.css";

interface AddTaskFormProps {
  projectId: string;
  setTasks: React.Dispatch<React.SetStateAction<Task[]>>;
}

export default function AddTaskForm({ projectId, setTasks }: AddTaskFormProps) {
  const [title, setTitle] = useState("");
  const [dueDate, setDueDate] = useState<string>("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!title.trim()) {
      setError("Title is required");
      return;
    }

    setError(null);
    setLoading(true);

    const dto: TaskCreateDto = {
      title,
      dueDate: dueDate ? new Date(dueDate).toISOString() : undefined,
      projectId,
    };

    try {
      const response = await createTask(projectId, dto);
      console.log("response got on createTask in addTaskForm: ", response);
      setTitle("");
      setDueDate("");
      setTasks((prev) => [...prev, response]);
    } catch (err) {
      console.error("Failed to create task:", err);
      setError("Failed to add task. Try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form className="add-task-form" onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Task title"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        required
      />
      <input
        type="date"
        value={dueDate || ""}
        placeholder="select deadline"
        onChange={(e) => setDueDate(e.target.value)}
      />
      <button
        type="submit"
        disabled={loading || !title.trim()}
      >
        {loading ? "Adding..." : "Add Task"}
      </button>
      {error && <p className="error">{error}</p>}
    </form>
  );
}
