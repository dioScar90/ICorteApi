import { useContext, useEffect, useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { AuthContext } from "../../contexts/auth/AuthContext";

export const useLoginViewModel = () => {
  const { user, login } = useContext(AuthContext);

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
