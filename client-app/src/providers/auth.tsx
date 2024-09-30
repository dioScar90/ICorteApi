import { UserService } from "@/services/user"
import { BarberShop, Profile, UserMe, UserRole } from "@/types/user"
import { createContext, PropsWithChildren, useContext, useEffect, useLayoutEffect, useState } from "react"

type UserType = Omit<UserMe, 'profile' | 'barberShop' | 'roles'>

type User = {
  user?: UserType
  profile?: Profile
  barberShop?: BarberShop
  roles: UserRole[]
}

const AuthContext = createContext<User | null>(null)

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
    UserService.getMe()
      .then(data => setToken(data.accessToken))
      .catch(err => setToken(null))
  }, [])

  useLayoutEffect(() => {
    //
  }, [token])

  const value = {
    //
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}