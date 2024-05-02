import { NavLink } from "react-router-dom";
import { useAuth } from '../../hooks/auth';

export const Header = () => {
  const { user, logout } = useAuth()

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
