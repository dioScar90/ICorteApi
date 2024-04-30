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
import { NotFound } from "./pages/NotFound";
import { AuthProvider, useAuth } from "./providers/AuthProvider";
import { Dashboard } from "./pages/Dashboard";
import { DashboardCard } from "./pages/Dashboard/DashboardCard";
import { DashboardLayout } from "./pages/Dashboard/components/DashboardLayout";

const Protected = () => {
  const { user } = useAuth();

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
};

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<DefaultLayout />}>
      <Route path="register" element={<Register />} action={registerAction} />
      <Route path="login" element={<Login />} action={loginAction} />
      <Route path="about" element={<Login />} />

      <Route element={<Protected />}>
        <Route index element={<Home />} />
        <Route path="dashboard" element={<DashboardLayout />}>
          <Route index element={<Dashboard />} />
          <Route path="card" element={<DashboardCard />} />
        </Route>
      </Route>

      <Route path="*" element={<NotFound />} />
    </Route>
  )
);

const App = () => {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  );
};

export default App;
