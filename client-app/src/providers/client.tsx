import { ProfileRepository } from "@/data/repositories/ProfileRepository"
import { IProfileRepository } from "@/data/repositories/interfaces/IProfileRepository"
import { ProfileService } from "@/data/services/ProfileService"
import { UserMe } from "@/types/user"
import { createContext, PropsWithChildren, useContext } from "react"
import { useAuth } from "./auth"
import { useProxy } from "./proxy"

export type ClientContextType = {
  profile: NonNullable<UserMe['profile']>
  repository: IProfileRepository
}

const ClientContext = createContext<ClientContextType | undefined>(undefined)

export function useClient() {
  const clientContext = useContext(ClientContext)

  if (!clientContext) {
    throw new Error('useClient must be used within an ClientProvider')
  }

  return clientContext
}

export function ClientProvider({ children }: PropsWithChildren) {
  const { httpClient } = useProxy()
  const { authUser } = useAuth()
  
  return (
    <ClientContext.Provider
      value={{
        profile: authUser!.profile!,
        repository: new ProfileRepository(new ProfileService(httpClient)),
      }}
    >
      {children}
    </ClientContext.Provider>
  )
}