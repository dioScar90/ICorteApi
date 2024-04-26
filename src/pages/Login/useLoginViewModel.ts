import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from '../../providers/AuthProvider';

export const useLoginViewModel = () => {
  const { user, login } = useAuth()

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    console.log("email", email);
    console.log("password", password);

    try {
      const loggedUser = await login(email, password);
      console.log("loggedUser", loggedUser);
      setTimeout(() => navigate("/dashboard"), 500);
    } catch (err) {
      console.log("error", err);
    }
  };

  // useEffect(() => {
  //   if (user) {
  //     navigate('/dashboard')
  //   }
  // }, [])

  return { user, setEmail, setPassword, handleLogin };
};
