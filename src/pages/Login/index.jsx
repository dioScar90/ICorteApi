import { Navigate } from 'react-router-dom'
import { useLoginViewModel } from './useLoginViewModel'
import { useAuth } from '../../providers/AuthProvider';

export const action = async ({ request }) => {
  const formData = await request.formData()
  const { email, password } = Object.fromEntries(formData)

  console.log("email", email);
  console.log("password", password);

  try {
    const registeredUser = await register(email, password);
    console.log("registeredUser", registeredUser);
    // setTimeout(() => navigate("/dashboard"), 500);
    return redirect('/dashboard')
  } catch (err) {
    console.log("error", err);
    return null
  }

  // return redirect('/dashboard')
};

export const Login = () => {
  const auth = useAuth()
  
  if (auth.user) {
    return <Navigate to="/dashboard" replace />
  }

  const { user, setEmail, setPassword, handleLogin } = useLoginViewModel(auth)
  
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
