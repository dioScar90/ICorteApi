import { Route, RouterProvider, createBrowserRouter, createRoutesFromElements } from 'react-router-dom'
import { Home } from './pages/Home'
import { Login } from './pages/login'
import { BaseLayout } from './components/layouts/BaseLayout'
import { Register } from './pages/register'
import { ClientLayout } from './components/layouts/ClientLayout'
import { BarberShopLayout } from './components/layouts/BarberShopLayout'
import { AdminLayout } from './components/layouts/AdminLayout'

export const baseRoute = {
  root: '/',
  client: '/client',
  barberShop: '/barber-shop',
  admin: '/admin',
} as const

export function App() {
  const browerRouter = createBrowserRouter(
    createRoutesFromElements(
      <Route>
        <Route path={baseRoute.root} element={<BaseLayout />}>
          <Route index element={<Home />} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
          
          <Route path={baseRoute.client} element={<ClientLayout />}>
            <Route index element={<Home />} />
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
          </Route>
          
          <Route path={baseRoute.barberShop} element={<BarberShopLayout />}>
            <Route index element={<Home />} />
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
          </Route>
          
          <Route path={baseRoute.admin} element={<AdminLayout />}>
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
