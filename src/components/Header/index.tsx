import { NavLink } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../../contexts/auth/AuthContext";

export const Header = () => {
  const { user, logout } = useContext(AuthContext);

  if (!user) {
    return (
      <nav>
        <NavLink to="/login">Login</NavLink>
        <NavLink to="/register">Register new account</NavLink>
      </nav>
    );
  }

  return (
    <nav>
      <button onClick={logout}>Logout</button>
    </nav>
  );
};
