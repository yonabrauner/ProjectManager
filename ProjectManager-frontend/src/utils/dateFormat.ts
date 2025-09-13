export function formatDateToLocal(dueDate?: string) {
  if (!dueDate) return "";
  const d = new Date(dueDate);
  const year = d.getFullYear();
  const month = String(d.getMonth() + 1).padStart(2, "0");
  const day = String(d.getDate()).padStart(2, "0");
  return `${day}-${month}-${year}`;
}

/**
 * Converts a date string from "DD-MM-YYYY" format back to "YYYY-MM-DD"
 * so that it can be parsed safely by `new Date()`.
 */
export function parseDateFromLocal(dateStr: string): string {

  const [day, month, year] = dateStr.split("-");

  return `${year}-${month}-${day}`;
}