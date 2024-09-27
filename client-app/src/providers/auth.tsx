import { createContext, PropsWithChildren, useContext, useEffect, useLayoutEffect, useState } from "react"



const AuthContext = createContext(undefined)

export function useAuth() {
  const authContext = useContext(AuthContext)

  if (!authContext) {
    throw new Error('useAuth must be used within an AuthProvider')
  }

  return authContext
}

function AuthProvider({ children }: PropsWithChildren) {
  const [token, setToken] = useState<boolean | null>()

  useEffect(() => {
    const authPromise = new Promise<{ accessToken: boolean }>()
    authPromise
      .then(data => setToken(data.accessToken))
      .catch(err => setToken(null))
  }, [])

  useLayoutEffect(() => {
    //
  }, [token])
}