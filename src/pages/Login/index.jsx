import { useLoginViewModel } from './useLoginViewModel'
import { actionFormLogin } from './actionFormLogin'

export const action = actionFormLogin

export const Login = () => {
  const { email, password, errors, setEmail, setPassword, isLoading } = useLoginViewModel()
  
  return (
    <div>
      <div>
        <h1>This is the login page</h1>
        <Form method="post">
          <input type="email" name="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.value)} />
          {errors?.email && <span>{errors.email.message}</span>}

          <input type="password" name="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.value)} />
          {errors?.password && <span>{errors.password.message}</span>}

          <button type="submit" disabled={isLoading}>Sign In</button>
        </Form>
      </div>
      <div>
        <h3>Login with Google</h3>
        <Form method="post">
          <input type="hidden" name="type" value="google" />
          <button type="submit">Sign In With Google</button>
        </Form>
      </div>
    </div>
  )
}
