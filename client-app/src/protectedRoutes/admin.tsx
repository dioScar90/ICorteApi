import { PropsWithChildren } from "react"
import { Navigate } from "react-router-dom"
import { useAuth } from "@/providers/auth"
import { ROUTE_ENUM } from "@/App"

export function ProtectedAdminRoute({ children }: PropsWithChildren) {
  const { isAdmin } = useAuth()
  
  if (!isAdmin) {
    return <Navigate to={ROUTE_ENUM.BARBER_SHOP} />
  }

  return children
}
