import { schema } from './consts'
import {
  signInWithEmailAndPassword,
  signInWithPopup,
  GoogleAuthProvider,
} from 'firebase/auth'
import { auth } from '@/firebase/firebase'

export const actionFormLogin = async ({ request }) => {
  const formData = Object.fromEntries(await request.formData())
  
  try {
    let loggedUser

    if (formData?.type === 'google') {
      const provider = new GoogleAuthProvider()
      loggedUser = await signInWithPopup(auth, provider)
    } else {
      const { email, password } = await schema.parseAsync(formData)
      loggedUser = signInWithEmailAndPassword(auth, email, password)
    }
    
    console.log('loggedUser', loggedUser);
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