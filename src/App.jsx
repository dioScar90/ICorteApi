import {
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  Navigate,
  Outlet,
} from "react-router-dom";
import { DefaultLayout } from "./components/DefaultLayout";
import { Home } from "./pages/Home";
import { Register, action as registerAction } from "./pages/Register";
import { Login, action as loginAction } from "./pages/Login";
import { About } from './pages/About';
import { NotFound } from "./pages/NotFound";
import { AuthProvider } from "./providers/AuthProvider";
import { useAuth } from './hooks/auth';
import { Dashboard } from "./pages/Dashboard";
import { DashboardCard } from "./pages/Dashboard/DashboardCard";
import { DashboardLayout } from "./pages/Dashboard/components/DashboardLayout";

const RedirectIfHasUser = () => {
  const { user } = useAuth()
  return user ? <Navigate to="/dashboard" replace /> : <Outlet />
}

const Protected = () => {
  const { user } = useAuth()
  return !user ? <Navigate to="/login" replace /> : <Outlet />
}

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<DefaultLayout />}>
      <Route index element={<Home />} />
      <Route path="about" element={<Login />} />

      <Route element={<RedirectIfHasUser />}>
        <Route path="register" element={<Register />} action={registerAction} />
        <Route path="login" element={<Login />} action={loginAction} />
      </Route>

      <Route element={<Protected />}>
        <Route path="dashboard" element={<DashboardLayout />}>
          <Route index element={<Dashboard />} />
          <Route path="card" element={<DashboardCard />} />
        </Route>
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
  );
}

export default App
