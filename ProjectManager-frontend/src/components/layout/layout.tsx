import type { ReactNode } from "react";
import { useAuth } from "../../contexts/AuthContext";
import "./Layout.css";

interface LayoutProps {
  children: ReactNode;
}

export default function Layout({ children }: LayoutProps) {
  const { logout, isLoggedIn } = useAuth();

  return (
    <div className="layout">
      {isLoggedIn && (
        <header className="layout-header">
          {/* <span className="layout-username">Hello, {username}!</span> */}
          <button className="layout-logout" onClick={logout}>
            Logout
          </button>
        </header>
      )}
      <main className="layout-main">{children}</main>
    </div>
  );
}
