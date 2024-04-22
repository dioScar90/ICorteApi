import './App.css'
import { RouterProvider, createBrowserRouter, createRoutesFromElements, Route, Navigate } from 'react-router-dom'
import { DefaultLayout } from './components/DefaultLayout'
import { Home } from './pages/Home'
import { Register } from './pages/Register'
import { Login } from './pages/Login'
import { NotFound } from './pages/NotFound'
import { AuthProvider } from './providers/AuthProvider'
import { Dashboard } from './pages/Dashboard'
import { FC, ReactNode, useContext } from 'react'
import { AuthContext } from './contexts/auth/AuthContext'
import { DashboardCard } from './pages/Dashboard/DashboardCard'
import { DashboardLayout } from './pages/Dashboard/components/DashboardLayout'

const RequireAuth: FC<{ children: ReactNode }> | null = ({ children }) => {
  const { user } = useContext(AuthContext)
  
  if (!user) {
    return <Navigate to="/login" replace />
  }

  return children
}

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<DefaultLayout />}>
      <Route index element={<Home />} />
      <Route path="register" element={<Register />} />
      <Route path="login" element={<Login />} />

      <Route
        path="dashboard"
        element={<RequireAuth><DashboardLayout /></RequireAuth>}
      >
        <Route index element={<Dashboard />} />
        <Route path="card" element={<DashboardCard />} />
      </Route>
      
      <Route path="*" element={<NotFound />} />
    </Route>
  )
)

const App = () => {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  )
}

export default App
