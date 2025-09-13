import React, { createContext, useContext, useState, useEffect, useCallback } from "react";
import { jwtDecode } from "jwt-decode";
import axiosInstance from "../api/axiosInstance";

interface JwtPayload {
  sub: string;
  exp: number;
}

interface AuthContextType {
  token: string | null;
  userId?: string;
  login: (token: string) => void;
  logout: () => void;
  isLoggedIn: boolean;
  validateToken: () => Promise<boolean>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [token, setToken] = useState<string | null>(localStorage.getItem("jwtToken"));
  const [userId, setUserId] = useState<string | undefined>();
  const [initialized, setInitialized] = useState(false); // ✅ here

  useEffect(() => {
    const storedToken = localStorage.getItem("jwtToken");
    if (storedToken) {
      try {
        const payload: JwtPayload = jwtDecode(storedToken);
        if (payload.exp * 1000 > Date.now()) {
          setToken(storedToken);
          setUserId(payload.sub);
        } else {
          localStorage.removeItem("jwtToken");
        }
      } catch {
        localStorage.removeItem("jwtToken");
      }
    }
    setInitialized(true); // finished checking token
  }, []);

  if (!initialized) return null; // don't render children until auth state is ready

  const login = useCallback((newToken: string) => {
    localStorage.setItem("jwtToken", newToken);
    setToken(newToken);
    const payload: JwtPayload = jwtDecode(newToken);
    setUserId(payload.sub);
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem("jwtToken");
    setToken(null);
    setUserId(undefined);
  }, []);

  const validateToken = useCallback(async (): Promise<boolean> => {
    if (!token) return false;

    try {
      const payload: JwtPayload = jwtDecode(token);
      if (payload.exp * 1000 < Date.now()) {
        logout();
        return false;
      }

      // optional backend validation
      await axiosInstance.get("/auth/validate");
      return true;
    } catch (err) {
      logout();
      return false;
    }
  }, [token, logout]);

  // initialize userId from token on mount
  useEffect(() => {
    if (token) {
      const payload: JwtPayload = jwtDecode(token);
      setUserId(payload.sub);
    }
  }, [token]);

  const isLoggedIn = !!token;

  return (
    <AuthContext.Provider value={{ token, userId, login, logout, isLoggedIn, validateToken }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used inside AuthProvider");
  return context;
};
