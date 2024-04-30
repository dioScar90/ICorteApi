import { useState } from "react"
import { doSignInWithEmailAndPassword, doSignInWithGoogle } from "../../../firebase/auth"

export const useLoginViewModel = () => {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [errorMessage, setErrorMessage] = useState('')

  const handleSubmit = async (e) => {
    e.preventDefault()

    if (!isAuthenticated) {
      setIsAuthenticated(true)
      await doSignInWithEmailAndPassword(email, password)
    }
  }

  const onGoogleSignIn = (e) => {
    e.preventDefault()

    if (!isAuthenticated) {
      setIsAuthenticated(true)
      doSignInWithGoogle()
        .catch(err => {
          setIsAuthenticated(false)
        })
    }
  }

 return {  }
}
