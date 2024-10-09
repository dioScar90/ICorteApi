import { httpClient } from "@/data/httpClient"
import { AuthRepository } from "@/data/repositories/AuthRepository"
import { UserRepository } from "@/data/repositories/UserRepository"
import { AuthService } from "@/data/services/AuthService"
import { UserService } from "@/data/services/UserService"
import { UserRegisterType } from "@/schemas/user"
import { UserMe } from "@/types/user"
import { createContext, PropsWithChildren, useContext, useEffect, useMemo, useReducer } from "react"

export type AuthUser = {
  user: Omit<UserMe, 'roles' | 'profile' | 'barberShop'>
  roles: UserMe['roles']
  profile?: UserMe['profile']
  barberShop?: UserMe['barberShop']
}

export type AuthContextType = {
  authUser: AuthUser | null
  isLoading: boolean
  isAuthenticated: boolean
  isClient: boolean
  isBarberShop: boolean
  isAdmin: boolean
  register: (data: UserRegisterType) => Promise<void>
  login: (data: UserRegisterType) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function useAuth() {
  const authContext = useContext(AuthContext)

  if (!authContext) {
    throw new Error('useAuth must be used within an AuthProvider')
  }

  return authContext
}

export type AuthState = {
  isLoading: boolean
  isAuthenticated: false
  authUser: null
} | {
  isLoading: boolean
  isAuthenticated: true
  authUser: AuthUser
}

export type AuthAction =
  | { type: 'LOGIN_SUCCESS'; payload: AuthUser }
  | { type: 'LOGIN_FAILURE' }
  | { type: 'LOGOUT' }
  | { type: 'SET_LOADING'; payload: boolean }
  
function authReducer(state: AuthState, action: AuthAction): AuthState {
  switch (action.type) {
    case 'LOGIN_SUCCESS':
      return {
        ...state,
        authUser: action.payload,
        isAuthenticated: true,
        isLoading: false,
      }
    case 'LOGIN_FAILURE':
      return {
        ...state,
        authUser: null,
        isAuthenticated: false,
        isLoading: false,
      }
    case 'LOGOUT':
      return {
        ...state,
        authUser: null,
        isAuthenticated: false,
      }
    case 'SET_LOADING':
      return {
        ...state,
        isLoading: action.payload,
      }
    default:
      return state
  }
}

const initialAuthState: AuthState = {
  authUser: null,
  isLoading: true,
  isAuthenticated: false,
}

export function AuthProvider({ children }: PropsWithChildren) {
  const authRepository = useMemo(() => new AuthRepository(new AuthService(httpClient)), [])
  const userRepository = useMemo(() => new UserRepository(new UserService(httpClient)), [])
  const [state, dispatch] = useReducer(authReducer, initialAuthState)

  const register = async (data: UserRegisterType) => {
    dispatch({ type: 'SET_LOADING', payload: true })

    const userResult = await authRepository.register(data)

    if (!userResult.isSuccess) {
      console.error('Failed to fetch user data:', userResult.error)
      dispatch({ type: 'LOGIN_FAILURE' })
      return
    }
    
    const userData: UserMe = userResult.value

    const user: AuthUser = {
      user: {
        id: userData.id,
        email: userData.email,
        phoneNumber: userData.phoneNumber,
      },
      roles: userData.roles,
      profile: userData.profile,
      barberShop: userData.barberShop,
    }

    dispatch({ type: 'LOGIN_SUCCESS', payload: user })
  }

  const login = async () => {
    dispatch({ type: 'SET_LOADING', payload: true })

    const userResult = await userRepository.getMe()

    if (!userResult.isSuccess) {
      console.error('Failed to fetch user data:', userResult.error)
      dispatch({ type: 'LOGIN_FAILURE' })
      return
    }
    
    const userData: UserMe = userResult.value

    const user: AuthUser = {
      user: {
        id: userData.id,
        email: userData.email,
        phoneNumber: userData.phoneNumber,
      },
      roles: userData.roles,
      profile: userData.profile,
      barberShop: userData.barberShop,
    }

    dispatch({ type: 'LOGIN_SUCCESS', payload: user })
  }

  const logout = () => {
    authRepository.logout()
    dispatch({ type: 'LOGOUT' })
  }
  
  useEffect(() => {
    const checkAuthStatus = async () => await login()
    checkAuthStatus()
  }, [])
  
  return (
    <AuthContext.Provider
      value={{
        authUser: state.authUser,
        isLoading: state.isLoading,
        isAuthenticated: state.isAuthenticated,
        isClient: !!state.authUser?.roles?.includes('Client'),
        isBarberShop: !!state.authUser?.roles?.includes('BarberShop'),
        isAdmin: !!state.authUser?.roles?.includes('Admin'),
        register,
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}