import "./taskRow.styles.css";
import type { Task, TaskUpdateDto } from "../../types/projectTask";
import { useState } from "react";
import { updateTask } from "../../services/projectTaskService";
import { formatDateToLocal, parseDateFromLocal } from "../../utils/dateFormat";

type TaskRowProps = Task & {
    onDelete: (taskId: string) => void;
    refreshTasks: () => void;
}

export default function TaskRow({ title, dueDate, isCompleted: initialCompleted, id, onDelete, projectId, refreshTasks}: TaskRowProps) {
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [editTitle, setEditTitle] = useState<string>(title);
  const [editDueDate, setEditDueDate] = useState<string>(formatDateToLocal(dueDate));
  const [isCompleted, setIsCompleted] = useState<boolean>(initialCompleted);


  const handleConfirm = async () => {
    if (!editTitle.trim()) return;

    const dto: TaskUpdateDto = {
        title: editTitle,
        dueDate: editDueDate ? new Date(editDueDate).toISOString() : undefined
    };
    const response: Task = await updateTask(projectId, id, dto);
    setEditTitle(response.title);
    setEditDueDate(formatDateToLocal(response.dueDate));

    setIsEditing(false);
  };

  const handleToggleComplete = async () => {
    const dto: TaskUpdateDto = {
        title: editTitle,
        dueDate: editDueDate ? new Date(parseDateFromLocal(editDueDate)).toISOString() : undefined,
        isCompleted: !isCompleted
    };
    const response: Task = await updateTask(projectId, id, dto);

    setIsCompleted(response.isCompleted);

    refreshTasks();
  }

  return (
    <div className="task-row">
      {isEditing ? (
        <>
          <input
            type="text"
            value={editTitle}
            onChange={(e) => setEditTitle(e.target.value)}
            className="task-edit-input"
          />
          <input
            type="date"
            value={editDueDate}
            onChange={(e) => setEditDueDate(e.target.value)}
            className="task-edit-input"
          />
        </>
      ) : (
        <>
          <div
            className="task-status clickable"
            onClick={handleToggleComplete}
            style={{ cursor: "pointer" }}
          >
            {isCompleted ? "✅" : "🔲"}
          </div>
          <div className="task-title">{editTitle}</div>
          {dueDate && (
            <div className="task-due">
              {editDueDate}
            </div>
          )}
        </>
      )}

      <div className="task-actions">
        {isEditing ? (
          <button
            className="task-confirm"
            onClick={handleConfirm}
            aria-label="Confirm update"
            type="button"
          >
            <span className="confirm-icon" aria-hidden="true">✅</span>
          </button>
        ) : (
          <button
            className="task-update"
            onClick={() => setIsEditing(true)}
            aria-label="Edit task"
            type="button"
          >
            <span className="update-icon" aria-hidden="true">✏️</span>
          </button>
        )}

        <button
          className="task-delete"
          onClick={() => onDelete?.(id)}
          aria-label="Delete task"
          type="button"
        >
          <span className="trash-icon" aria-hidden="true">🗑️</span>
        </button>
      </div>
    </div>
  );
}
