import { Route, RouterProvider, createBrowserRouter, createRoutesFromElements } from 'react-router-dom'
import { Home } from './pages/Home'
import { Login } from './pages/login'
import { BaseLayout } from './components/layouts/BaseLayout'
import { Register } from './pages/register'
import { ClientLayout } from './components/layouts/ClientLayout'
import { BarberShopLayout } from './components/layouts/BarberShopLayout'
import { AdminLayout } from './components/layouts/AdminLayout'
import { BarberScheduleLayout } from './components/layouts/BarberScheduleLayout'

export const ROUTE_ENUM = {
  ROOT: '/',
  REGISTER: '/register',
  LOGIN: '/login',
  LOGOUT: '/logout',
  CLIENT: '/client',
  PROFILE: '/profile',
  BARBER_SHOP: '/barber-shop',
  BARBER_SCHEDULE: '/barber-schedule',
  ADMIN: '/admin',
} as const

export function App() {
  const browerRouter = createBrowserRouter(
    createRoutesFromElements(
      <Route>
        <Route path={ROUTE_ENUM.ROOT} element={<BaseLayout />}>
          <Route index element={<Home />} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
          
          <Route path={ROUTE_ENUM.CLIENT} element={<ClientLayout />}>
            <Route index element={<Home />} />
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
          </Route>

          <Route path={ROUTE_ENUM.PROFILE} element={<ClientLayout />}>
            <Route path=":id" element={<p>Profile</p>} />
            <Route path=":id/edit" element={<p>Edit</p>} />
          </Route>
          
          <Route path={ROUTE_ENUM.BARBER_SHOP} element={<BarberShopLayout />}>
            <Route index element={<Home />} />
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
          </Route>
          
          <Route path={ROUTE_ENUM.BARBER_SCHEDULE} element={<BarberScheduleLayout />}>
            <Route index element={<Home />} />
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
          </Route>
          
          <Route path={ROUTE_ENUM.ADMIN} element={<AdminLayout />}>
            <Route index element={<Home />} />
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
          </Route>
        </Route>
      </Route>
    )
  )

  return <RouterProvider router={browerRouter} />
}
