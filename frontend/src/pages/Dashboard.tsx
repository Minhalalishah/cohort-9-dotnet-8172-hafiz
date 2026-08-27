import { useEffect, useState } from 'react'
import api from '../api'
import '../css/app.css'

export default function Dashboard() {
  const [counts, setCounts] = useState<any>({})

  useEffect(() => {
    api
      .get('/dashboard/counts')
      .then(r => setCounts(r.data))
      .catch(err => {
        console.error('Dashboard error:', err)
      })
  }, [])

  return (
    <div>

      <div className="dashboard-welcome">
        <h1>Welcome to your Dashboard</h1>
        <p>
          Track your tasks, monitor progress and stay organized.
        </p>
      </div>

      <header>
        <h1>Task Overview</h1>
        <p className="muted">
          Here's what's happening with your tasks.
        </p>
      </header>

      <div className="cards">

        <div className="stat">
          <div className="stat-icon">
            ◷
          </div>

          <b>{counts.pending ?? 0}</b>

          <span>Pending Tasks</span>
        </div>

        <div className="stat">
          <div className="stat-icon">
            ↻
          </div>

          <b>{counts.inProgress ?? 0}</b>

          <span>In Progress</span>
        </div>

        <div className="stat">
          <div className="stat-icon">
            ✓
          </div>

          <b>{counts.completed ?? 0}</b>

          <span>Completed</span>
        </div>

        <div className="stat">
          <div className="stat-icon">
            #
          </div>

          <b>{counts.total ?? 0}</b>

          <span>Total Tasks</span>
        </div>

      </div>

    </div>
  )
}