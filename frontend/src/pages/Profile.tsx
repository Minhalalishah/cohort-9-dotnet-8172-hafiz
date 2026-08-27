import { useEffect, useState } from 'react'
import api from '../api'
import type { User } from '../types'
import '../css/app.css'

export default function Profile() {
  const [user, setUser] = useState<User | null>(null)

  useEffect(() => {
    api
      .get<User>('/users/me')
      .then(r => setUser(r.data))
      .catch(err => {
        console.error('Unable to load profile:', err)
      })
  }, [])

  if (!user) {
    return (
      <div className="card loading-card">
        <div className="loading-spinner" />
        <p className="muted">Loading profile...</p>
      </div>
    )
  }

  const initials = user.fullName
    ?.split(' ')
    .map(name => name[0])
    .join('')
    .slice(0, 2)
    .toUpperCase()

  return (
    <div className="card profile-card">

      <div className="profile-header">

        <div className="profile-avatar">
          {initials}
        </div>

        <div className="profile-info">
          <h2>{user.fullName}</h2>
          <span>Account Profile</span>
        </div>

      </div>

      <div className="profile-details">

        <div className="profile-item">
          <label>Full Name</label>
          <span>{user.fullName}</span>
        </div>

        <div className="profile-item">
          <label>Email Address</label>
          <span>{user.email}</span>
        </div>

        <div className="profile-item">
          <label>Role</label>
          <span>{user.role}</span>
        </div>

      </div>

    </div>
  )
}