import { Form } from 'react-router-dom'
import { useRegisterViewModel } from './useRegisterViewModel'
import { actionFormRegister } from './actionFormRegister';

export const action = actionFormRegister

export const Register = () => {
  const { email, password, errors, setEmail, setPassword, isLoading } = useRegisterViewModel()
  
  return (
    <div>
      <div>
        <h1>This is the register page</h1>
        <Form method="post">
          <input type="email" name="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.value)} />
          {errors?.email && <span>{errors.email.message}</span>}

          <input type="password" name="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.value)} />
          {errors?.password && <span>{errors.password.message}</span>}

          <button type="submit" disabled={isLoading}>Sign Up</button>
        </Form>
      </div>
      <div>
        <h3>Register with Google</h3>
        <Form method="post">
          <input type="hidden" name="type" value="google" />
          <button type="submit">Sign Up With Google</button>
        </Form>
      </div>
    </div>
  )
}
