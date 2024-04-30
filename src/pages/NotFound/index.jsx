import { useNotFoundViewModel } from './useNotFoundViewModel'

export const NotFound = () => {
  const vv = useNotFoundViewModel()

  return (
    <div { ...vv }>NotFound</div>
  )
}
