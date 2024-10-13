import { AuthProvider } from '@/providers/auth'
import { ProxyProvider } from '@/providers/proxy'
import { QueryClientProvider, QueryClient } from '@tanstack/react-query'
import { PropsWithChildren } from 'react'

const client = new QueryClient()

export function MainProviders({ children }: PropsWithChildren) {
  return (
    <QueryClientProvider client={client}>
      <ProxyProvider>
        <AuthProvider>
          {children}
        </AuthProvider>
      </ProxyProvider>
    </QueryClientProvider>
  )
}
