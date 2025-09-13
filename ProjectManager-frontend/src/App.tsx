import { BrowserRouter as Router } from "react-router-dom";
import AppRoutes from "./navigation/appRoutes";
import { AuthProvider } from "./contexts/AuthContext";
import './App.css'

function App() {


  return (
    <AuthProvider>
      <Router>
        <AppRoutes />
      </Router>
    </AuthProvider>
  );
};

export default App
