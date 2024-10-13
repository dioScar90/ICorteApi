import { PropsWithChildren } from "react"
import { Navigate } from "react-router-dom"
import { ROUTE_ENUM } from "@/App"
import { BarberShopProvider } from "@/providers/barberShop"
import { useAuth } from "@/providers/auth"

export function ProtectedBarberShopRoute({ children }: PropsWithChildren) {
  const { isBarberShop } = useAuth()
  
  if (!isBarberShop) {
    return <Navigate to={ROUTE_ENUM.CLIENT} />
  }

  return (
    <BarberShopProvider>
      {children}
    </BarberShopProvider>
  )
}
