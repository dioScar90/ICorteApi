import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from '../../hooks/auth';

export const useLoginViewModel = () => {
  const { user, isLoading, login, loginWithGoogle } = useAuth()
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
    login,
    loginWithGoogle,
    errors,
  }
};
