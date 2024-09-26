import { cn } from "@/lib/utils"
import { PropsWithChildren } from 'react'

type MaxWidthType = PropsWithChildren & {
  className?: string
}

export function MaxWidthWrapper({ className, children }: MaxWidthType) {
  return (
    <div className={cn('h-full mx-auto w-full max-w-screen-xl px-2.5 md:px-20', className)}>
      {children}
    </div>
  )
}
