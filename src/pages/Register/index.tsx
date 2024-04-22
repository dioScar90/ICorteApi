import { Navigate } from 'react-router-dom'
import { useRegisterViewModel } from './useRegisterViewModel'

export const Register = () => {
  const { user, setEmail, setPassword, handleRegister, handleRegisterWithGoogle } = useRegisterViewModel()
  
  if (user) {
    return <Navigate to="/dashboard" replace />
  }
  
  return (
    <div>
      <div>
        <h1>This is the register page</h1>
        <form onSubmit={handleRegister}>
          <input onChange={(e) => setEmail(e.target.value)} type="email" name="email" placeholder="Email" />
          <input onChange={(e) => setPassword(e.target.value)} type="password" name="password" placeholder="Password" />
          <button type="submit">Sign Up</button>
        </form>
      </div>
      <div>
        <h3>Register with Google</h3>
        <form onSubmit={handleRegisterWithGoogle}>
          <button type="submit">Sign Up With Google</button>
        </form>
      </div>
    </div>
  )
}
