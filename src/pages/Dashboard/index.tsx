// import PropTypes from 'prop-types'
import { useDashboardViewModel } from './useDashboardViewModel'
import { Outlet } from 'react-router-dom'

export const Dashboard = () => {
  const {  } = useDashboardViewModel()

  return (
    <div>
      <h3>Dashboard</h3>
      <p>This is dashboard page</p>
    </div>
  )
}

// Dashboard.propTypes = {}