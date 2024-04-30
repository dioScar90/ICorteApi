import { Form, redirect } from 'react-router-dom'
import { useRegisterViewModel } from './useRegisterViewModel'
import { schema } from './consts';

export const action = async ({ request }) => {
  const formData = await request.formData()
  const type = formData.get('type')
  const email = formData.get('email')
  const password = formData.get('password')

  if (type === 'google') {
    // do loginWithGoogle
    return
  }
  
  const errors = {}
  
  console.log('email', email)
  console.log('password', password)

  await schema.parseAsync({ email, password })

  if (Object.keys(errors).length) {
    return errors
  }

  try {
    const registeredUser = await register(email, password);
    console.log('registeredUser', registeredUser);
    // setTimeout(() => navigate('/dashboard'), 500);
    return redirect('/dashboard')
  } catch (err) {
    console.log('error', err);
    return null
  }
};

export const Register = () => {
  const { email, password } = useRegisterViewModel()
  
  return (
    <div>
      <div>
        <h1>This is the register page</h1>
        <Form method="post">
          <input type="email" name="email" placeholder="Email" value={email} />
          <input type="password" name="password" placeholder="Password" value={password} />
          <input type="hidden" name="type" value="usual" />
          <button type="submit">Sign Up</button>
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
