import { httpClient } from "@/data/httpClient"
import { UserService } from "@/data/services/UserService"
import { UserMe } from "@/types/user"
import { createContext, PropsWithChildren, useContext, useEffect, useLayoutEffect, useState } from "react"

// type UserType = Omit<UserMe, 'profile' | 'barberShop' | 'roles'>

// type User = {
//   user?: UserType
//   profile?: Profile
//   barberShop?: BarberShop
//   roles: UserRole[]
// }

const AuthContext = createContext<UserMe | null>(null)

export function useAuth() {
  const authContext = useContext(AuthContext)

  if (!authContext) {
    throw new Error('useAuth must be used within an AuthProvider')
  }

  return authContext
}

function AuthProvider({ children }: PropsWithChildren) {
  const service = new UserService(httpClient)
  const [user, setUser] = useState<UserMe | null>()

  useEffect(() => {
    service.getMe()
      .then(data => setUser(data.data))
      .catch((_) => setUser(null))
  }, [])

  useLayoutEffect(() => {
    //
  }, [token])

  const value = {
    //
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}