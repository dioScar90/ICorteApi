// import PropTypes from 'prop-types'
import { useDashboardLayoutViewModel } from './useDashboardLayoutViewModel'
import { Outlet } from 'react-router-dom'

export const DashboardLayout = () => {
  const {  } = useDashboardLayoutViewModel()

  return (
    <div>
      <h1>DashboardLayout</h1>
      <Outlet />
    </div>
  )
}

// DashboardLayout.propTypes = {}