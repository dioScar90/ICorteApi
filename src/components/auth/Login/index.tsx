import { Navigate } from 'react-router-dom'
import './Login.css'
import { useLoginViewModel } from './useLoginViewModel'

export const Login = () => {
 const {  } = useLoginViewModel()
//  const userLoggedIn = true

 return (
   <div>
    {userLoggedIn && <Navigate to="/home" replace={true} />}
    <main>
      <div>
        <div>

        </div>
      </div>
    </main>
   </div>
 )
}
