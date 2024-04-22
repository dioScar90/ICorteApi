import { Navigate, useNavigate } from 'react-router-dom'
import './Register.css'
import { useRegisterViewModel } from './useRegisterViewModel'
import { useState } from 'react'

export const Register = () => {
//  const {  } = useRegisterViewModel()
  const navigate = useNavigate()

  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [isRegistering, setIsRegistering] = useState(false)
  const [errorMessage, setErrorMessage] = useState('')

  const handleSubmit = (e) => {
    e.preventDefault()
  }

  return (
    <>
      {userLoggedIn && <Navigate to="/home" replace={true} />}
      <main>
        <div>
          <div>
            <div><h3>Create a new account</h3></div>
          </div>
        </div>
      </main>
    </>
  )
}
