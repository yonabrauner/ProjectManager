import { useEffect, useState } from "react";
import { authService } from "../../services/authService";
import { useAuth } from "../../contexts/AuthContext";
import type { LoginDto, RegisterDto, AuthResponseDto } from "../../types/auth";
import { useNavigate } from "react-router-dom";
import './auth.styles.css';

export default function LoginRegisterPage() {
  const [isLogin, setIsLogin] = useState(true);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [message, setMessage] = useState<string | null>(null);
  const navigate = useNavigate();
  const { login, isLoggedIn } = useAuth();

  useEffect(() => {
  if (isLoggedIn) {
    navigate("/");
  }
}, [isLoggedIn]);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      var response: AuthResponseDto;
      if (isLogin) {
        const loginDto: LoginDto = { username, password }
        response = await authService.login(loginDto);
        setMessage(`Welcome back, ${response.username}!`);
      } else {
        const registerDto: RegisterDto = { username, password }
        response = await authService.register(registerDto);
        setMessage(`Account created for ${response.username}. Logged in!`);
      }
      if (response.token) {
        console.log(response.token);
        login(response.token);     // <-- update state and localStorage
      } else {
        console.error("No token returned from register");
      }
    } catch (err: any) {
      setMessage(err.response?.data?.error ?? "Something went wrong." + err);
    }
  }

  return (
    <div className="container">
      <div className="card">
        <h2 className="heading">
          {isLogin ? "Login" : "Register"}
        </h2>

        {message && (<div className="message">{message}</div>)}

        <form onSubmit={handleSubmit}>
          {/* Username field */}
          <div className="input-wrapper">
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder=" "
              className="input-field"
              required
            />
            <label className={"input-label"}>Username</label>
          </div>

          {/* Password field */}
          <div className={"input-wrapper"}>
            <input
              type={showPassword ? "text" : "password"}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder=" "
              className={"input-field"}
              required
            />
            <label className={"input-label"}>Password</label>
            <div
              className={"password-toggle"}
              onClick={() => setShowPassword(!showPassword)}
            >
              <span className="password-icon">{showPassword ? "🙈" : "👁️"}</span>
            </div>
          </div>

          <button
            type="submit"
            className="button"
          >
            {isLogin ? "Login" : "Register"}
          </button>
        </form>

        <p className="toggleText">
          {isLogin ? "Don't have an account?" : "Already have an account?"}{" "}
          <button
            onClick={() => setIsLogin(!isLogin)}
            className="toggle-button"
          >
            {isLogin ? "Register here" : "Login here"}
          </button>
        </p>
      </div>
    </div>
  );
}
