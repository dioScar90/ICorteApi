import { QueryClientProvider, QueryClient } from '@tanstack/react-query'
import { PropsWithChildren } from 'react'

const client = new QueryClient()

export default function Providers({ children }: PropsWithChildren) {
  return (
    <QueryClientProvider client={client}>
      {children}
    </QueryClientProvider>
  )
}
