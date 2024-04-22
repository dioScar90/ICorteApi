import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../../contexts/auth/AuthContext";

export const useRegisterViewModel = () => {
  const { user, register, loginWithGoogle } = useContext(AuthContext);
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleRegister = async (e) => {
    e.preventDefault();

    console.log("email", email);
    console.log("password", password);

    try {
      const registeredUser = await register(email, password);
      console.log("registeredUser", registeredUser);
      setTimeout(() => navigate("/dashboard"), 500);
    } catch (err) {
      console.log("error", err);
    }
  };

  const handleRegisterWithGoogle = async (e) => {
    e.preventDefault();

    console.log("email", email);
    console.log("password", password);

    try {
      const registeredUser = await loginWithGoogle();
      console.log("registeredUser", registeredUser);
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

  return {
    user,
    navigate,
    setEmail,
    setPassword,
    handleRegister,
    handleRegisterWithGoogle,
    user,
  };
};
