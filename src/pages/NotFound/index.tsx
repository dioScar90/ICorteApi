import { FC } from 'react'
import { useNotFoundViewModel } from './useNotFoundViewModel'

interface NotFoundProps {}

export const NotFound: FC<NotFoundProps> = () => {
  const vv = useNotFoundViewModel()

  return (
    <div { ...vv }>NotFound</div>
  )
}
