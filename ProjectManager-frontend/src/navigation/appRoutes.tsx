import { Routes, Route, Navigate } from "react-router-dom";
import LoginRegisterPage from '../pages/auth/auth.page';
import HomePage from '../pages/home/home.page';
import { useAuth } from "../contexts/AuthContext";
import Layout from "../components/layout/layout";

export default function AppRoutes() {
  const { isLoggedIn } = useAuth();

  return (
    <Routes>
      <Route
        path="/login"
        element={isLoggedIn ? <Navigate to="/" /> : <LoginRegisterPage />}
      />
      <Route
        path="/"
        element={isLoggedIn ?
          (<Layout>
            <HomePage /> 
          </Layout>)
          : <Navigate to="/login" />}
      />
    </Routes>
  );
}