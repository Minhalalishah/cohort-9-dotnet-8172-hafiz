import { Link, Outlet, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../auth'
import '../css/sidebar.css'

export default function Layout() {
  const { user, logout } = useAuth()
  const nav = useNavigate()
  const location = useLocation()

  const navItems = [
    { label: 'Dashboard', path: '/' },
    { label: 'Tasks', path: '/tasks' },
    { label: 'Profile', path: '/profile' }
  ]

  const handleLogout = () => {
    logout()
    nav('/login')
  }

  return (
    <div className="layout">
      {/* Sidebar */}
      <aside className="sidebar">
        <div className="sidebar__top">
          {/* Brand & User Info */}
          <div className="sidebar__brand">
            <div className="sidebar__logo">TF</div>
            <div className="sidebar__info">
              <h2 className="sidebar__title">TaskFlow</h2>
              <span className="sidebar__role">{user?.role || 'User'}</span>
            </div>
          </div>

          {/* Navigation Links */}
          <nav className="sidebar__nav">
            {navItems.map((item) => {
              const isActive = location.pathname === item.path
              return (
                <Link
                  key={item.path}
                  to={item.path}
                  className={`sidebar__link ${isActive ? 'sidebar__link--active' : ''}`}
                >
                  {item.label}
                </Link>
              )
            })}
          </nav>
        </div>

        {/* Footer / Logout */}
        <div className="sidebar__footer">
          <button onClick={handleLogout} className="sidebar__logout">
            Logout
          </button>
        </div>
      </aside>

      {/* Main Content Area */}
      <main className="layout__content">
        <Outlet />
      </main>
    </div>
  )
}