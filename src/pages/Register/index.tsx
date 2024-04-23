import { ActionFunctionArgs, Form, Navigate, redirect } from 'react-router-dom'
import { useRegisterViewModel } from './useRegisterViewModel'
import { useContext } from 'react';
import { AuthContext } from '../../contexts/auth/AuthContext';
import { RegisterType, schema } from './consts';

export const action = async ({ request }: ActionFunctionArgs) => {
  const { register } = useContext(AuthContext)
  const formData = await request.formData()
  const { email, password } = Object.fromEntries(formData) as RegisterType

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
};

export const Register = () => {
  const auth = useContext(AuthContext)
  
  if (auth.user) {
    return <Navigate to="/dashboard" replace />
  }

  const { user, handleRegister, handleRegisterWithGoogle } = useRegisterViewModel(auth!)
  
  
  return (
    <div>
      <div>
        <h1>This is the register page</h1>
        <Form method="post">
          <input type="email" name="email" placeholder="Email" />
          <input type="password" name="password" placeholder="Password" />
          <button type="submit">Sign Up</button>
        </Form>
      </div>
      <div>
        <h3>Register with Google</h3>
        <Form method="post">
          <button type="submit">Sign Up With Google</button>
        </Form>
      </div>
    </div>
  )
}
