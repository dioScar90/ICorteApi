import PropTypes from 'prop-types'
import { Spinner } from '../../Spinner'

const getBtnVariant = (variant) => ({
  normal: 'bg-slate-900 text-white hover:bg-slate-800',
  ghost: 'bg-transparent hover:text-slate-900 hover:bg-slate-200',
})[variant]

const getBtnSize = (size) => ({
  sm: 'h-9 px-2',
  md: 'h-10 py-2 px-4',
  lg: 'h-11 px-8',
})[size]

const getBtnClassName = (variant, size, className) => {
  const classVariant = getBtnVariant(variant ?? 'normal')
  const classSize = getBtnSize(size ?? 'md')
  const others = className ?? ''
  
  const btnClassName = `
    active:scale-95 inline-flex items-center justify-center rounded-md text-sm font-medium
    transition-color focus:outline-none focus:ring-2 focus:ring-slate-400 focus:ring-offset-2
    disabled:opacity-50 disabled:pointer-events-none ${classVariant} ${classSize} ${others}
  `

  return btnClassName.replace(/\s+/g, ' ').trim()
}

export const Button = ({ children, isLoading, variant, size, type, className, ...props }) => {
  const btnClassName = getBtnClassName(variant, size, className)

  return (
    <button
      {...props}
      className={btnClassName}
      type={type ?? 'button'}
      disabled={!!isLoading}
    >
      {!!isLoading && <Spinner />}
      {children}
    </button>
  )
}

getBtnVariant.propTypes = {
  variant: PropTypes.oneOf(['normal', 'ghost']),
}

getBtnSize.propTypes = {
  size: PropTypes.oneOf(['md', 'sm', 'lg']),
}

getBtnClassName.propTypes = {
  variant: PropTypes.oneOf(['normal', 'ghost']),
  size: PropTypes.oneOf(['md', 'sm', 'lg']),
  className: PropTypes.string,
}

Button.propTypes = {
  children: PropTypes.node,
  isLoading: PropTypes.bool,
  variant: PropTypes.oneOf(['normal', 'ghost']),
  size: PropTypes.oneOf(['md', 'sm', 'lg']),
  type: PropTypes.oneOf(['button', 'submit', 'reset']),
  className: PropTypes.string,
  props: PropTypes.objectOf(PropTypes.any),
}