import { useContext, useEffect, useState } from "react";
import { useNavigate, ActionFunctionArgs, redirect } from "react-router-dom";
import { AuthContext } from "../../contexts/auth/AuthContext";
import { AuthContextType } from '../../@types/auth';
import { RegisterType } from './consts';

export const useRegisterViewModel = ({ user, register, loginWithGoogle }: AuthContextType) => {
  // const { user, register, loginWithGoogle } = useContext(AuthContext);
  const navigate = useNavigate();
  
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
