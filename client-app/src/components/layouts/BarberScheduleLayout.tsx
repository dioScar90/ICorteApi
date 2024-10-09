import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "@/providers/auth";
import { ROUTE_ENUM } from "@/App";

export function BarberScheduleLayout() {
  const { authUser } = useAuth()

  if (!authUser?.barberShop) {
    return <Navigate to={ROUTE_ENUM.CLIENT} replace />
  }
  
  return <Outlet />
}
