import { schema } from './consts';
import { ZodError } from 'zod';
import {
  createUserWithEmailAndPassword,
  signInWithPopup,
  GoogleAuthProvider,
} from 'firebase/auth'
import { auth } from '@/firebase/firebase'

export const actionFormRegister = async ({ request }) => {
  const formData = Object.fromEntries(await request.formData())
  
  try {
    let registeredUser

    if (formData?.type === 'google') {
      const provider = new GoogleAuthProvider()
      registeredUser = await signInWithPopup(auth, provider)
    } else {
      const { email, password } = await schema.parseAsync(formData)
      registeredUser = await createUserWithEmailAndPassword(auth, email, password)
    }
    
    console.log('registeredUser', registeredUser);
    return redirect('/dashboard')
  } catch (err) {
    if (err instanceof ZodError) {
      console.log('ZodError', err)
      return err.issues.reduce((acc, { message, path }) => ({ ...acc, [path[0]]: { message } }), {})
    }

    console.log('Error', err)
    return null
  }
}