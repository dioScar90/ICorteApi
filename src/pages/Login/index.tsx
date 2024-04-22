import { Navigate } from 'react-router-dom'
import { useLoginViewModel } from './useLoginViewModel'

export const Login = () => {
  const { user, setEmail, setPassword, handleLogin } = useLoginViewModel()
  
  if (user) {
    return <Navigate to="/dashboard" replace />
  }

  return (
    <div>
      <h1>This is the login page</h1>
      <form onSubmit={handleLogin}>
        <input onChange={(e) => setEmail(e.target.value)} type="email" name="email" placeholder="Email" />
        <input onChange={(e) => setPassword(e.target.value)} type="password" name="password" placeholder="Password" />
        <button type="submit">Sign On</button>
      </form>
    </div>
  )
}
