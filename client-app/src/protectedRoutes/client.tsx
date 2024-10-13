import { PropsWithChildren } from "react"
import { Navigate } from "react-router-dom"
import { ROUTE_ENUM } from "@/App"
import { ClientProvider } from "@/providers/client"
import { useAuth } from "@/providers/auth"

export function ProtectedClientRoute({ children }: PropsWithChildren) {
  const { isClient } = useAuth()
  
  if (!isClient) {
    return <Navigate to={ROUTE_ENUM.LOGIN} />
  }

  return (
    <ClientProvider>
      {children}
    </ClientProvider>
  )
}
