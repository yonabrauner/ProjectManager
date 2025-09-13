import React, { createContext, useContext, useState, useEffect, useCallback } from "react";
import { jwtDecode } from "jwt-decode";

interface JwtPayload { sub: string; username?: string; exp: number; }

interface AuthContextType {
  token: string | null;
  userId?: string;
  username?: string
  isLoggedIn: boolean;
  login: (token: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [token, setToken] = useState<string | null>(null);
  const [userId, setUserId] = useState<string | undefined>();
  const [username, setUsername] = useState<string | undefined>();
  
  // Initialize token from localStorage
  useEffect(() => {
    const storedToken = localStorage.getItem("jwtToken");
    if (storedToken) {
      try {
        const payload: JwtPayload = jwtDecode(storedToken);
        if (payload.exp * 1000 > Date.now()) {
          setToken(storedToken);
          setUserId(payload.sub);
          setUsername(payload.username);
        } else {
          localStorage.removeItem("jwtToken");
        }
      } catch {
        localStorage.removeItem("jwtToken");
      }
    }
  }, []);

  const login = useCallback((newToken: string) => {
    localStorage.setItem("jwtToken", newToken);
    setToken(newToken);
    const payload: JwtPayload = jwtDecode(newToken);
    setUserId(payload.sub);
    setUsername(payload.username);
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem("jwtToken");
    setToken(null);
    setUserId(undefined);
  }, []);

  const isLoggedIn = !!token;

  return (
    <AuthContext.Provider value={{ token, userId, username, isLoggedIn, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used inside AuthProvider");
  return context;
};