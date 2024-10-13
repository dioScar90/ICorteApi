import { BarberShopRepository } from "@/data/repositories/BarberShopRepository"
import { IBarberShopRepository } from "@/data/repositories/interfaces/IBarberShopRepository"
import { BarberShopService } from "@/data/services/BarberShopService"
import { UserMe } from "@/types/user"
import { createContext, PropsWithChildren, useContext } from "react"
import { useAuth } from "./auth"
import { useProxy } from "./proxy"

export type BarberShopContextType = {
  barberShop: NonNullable<UserMe['barberShop']>
  repository: IBarberShopRepository
}

const BarberShopContext = createContext<BarberShopContextType | undefined>(undefined)

export function useBarberShop() {
  const authContext = useContext(BarberShopContext)

  if (!authContext) {
    throw new Error('useBarberShop must be used within an BarberShopProvider')
  }

  return authContext
}

export function BarberShopProvider({ children }: PropsWithChildren) {
  const { httpClient } = useProxy()
  const { authUser } = useAuth()
  
  return (
    <BarberShopContext.Provider
      value={{
        barberShop: authUser!.barberShop!,
        repository: new BarberShopRepository(new BarberShopService(httpClient)),
      }}
    >
      {children}
    </BarberShopContext.Provider>
  )
}