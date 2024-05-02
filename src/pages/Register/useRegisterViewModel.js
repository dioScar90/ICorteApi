import { useEffect, useState } from "react";
import { useNavigate, useActionData } from "react-router-dom";
import { useAuth } from '../../hooks/auth';

export const useRegisterViewModel = () => {
  const { user, isLoading, register, loginWithGoogle } = useAuth()
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const errors = useActionData()
  const navigate = useNavigate()
  
  useEffect(() => {
    if (user) {
      navigate('/dashboard', { replace: true })
    }
  }, [user, navigate])

  return {
    user,
    isLoading,
    email,
    setEmail,
    password,
    setPassword,
    register,
    loginWithGoogle,
    errors,
  }
}
