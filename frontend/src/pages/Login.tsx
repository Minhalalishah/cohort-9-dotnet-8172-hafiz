import { FormEvent, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../auth'
import '../css/auth.css'

export default function Login() {
  const [email, setEmail] = useState('admin@tasktool.local')
  const [password, setPassword] = useState('Admin@123')
  const [showPassword, setShowPassword] = useState(false)
  const [error, setError] = useState('')

  const { login } = useAuth()
  const nav = useNavigate()

  const submit = async (e: FormEvent) => {
    e.preventDefault()
    setError('')

    try {
      await login(email, password)
      nav('/')
    } catch (err: any) {
      setError(err.response?.data?.message || 'Invalid email or password')
    }
  }

  return (
    <div className="auth-page">
      <div className="auth-container">

        {/* Left Side */}
        <div className="auth-brand">
          <div className="brand-icon">✓</div>

          <h1>TaskFlow</h1>

          <p>
            Organize your work, manage your tasks,
            and get things done efficiently.
          </p>

          <div className="feature">
            <span>✓</span>
            <div>
              <strong>Manage Tasks</strong>
              <small>Create and organize your daily tasks</small>
            </div>
          </div>

          <div className="feature">
            <span>✓</span>
            <div>
              <strong>Track Progress</strong>
              <small>Stay updated with your productivity</small>
            </div>
          </div>

          <div className="feature">
            <span>✓</span>
            <div>
              <strong>Stay Organized</strong>
              <small>Keep everything in one place</small>
            </div>
          </div>
        </div>

        {/* Login Card */}
        <div className="auth-card">
          <div className="mobile-logo">
            <div className="brand-icon">✓</div>
            <h2>TaskFlow</h2>
          </div>

          <div className="auth-header">
            <h2>Welcome Back 👋</h2>
            <p>Sign in to continue to your account</p>
          </div>

          {error && (
            <div className="error">
              <span>⚠</span>
              {error}
            </div>
          )}

          <form onSubmit={submit}>

            <div className="input-group">
              <label>Email Address</label>

              <div className="input-wrapper">
                <span className="input-icon">✉</span>

                <input
                  type="email"
                  placeholder="Enter your email"
                  value={email}
                  onChange={e => setEmail(e.target.value)}
                  required
                />
              </div>
            </div>

            <div className="input-group">
              <div className="password-label">
                <label>Password</label>
              </div>

              <div className="input-wrapper">
                <span className="input-icon">🔒</span>

                <input
                  type={showPassword ? 'text' : 'password'}
                  placeholder="Enter your password"
                  value={password}
                  onChange={e => setPassword(e.target.value)}
                  required
                />

<button
  type="button"
  className="password-toggle"
  onClick={() => setShowPassword(!showPassword)}
  aria-label={showPassword ? 'Hide password' : 'Show password'}
>
  {showPassword ? (
    <svg
      width="20"
      height="20"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <path d="M3 3l18 18" />
      <path d="M10.58 10.58a2 2 0 002.83 2.83" />
      <path d="M9.88 4.24A10.94 10.94 0 0112 4c5 0 9.27 3.11 11 8a10.9 10.9 0 01-4.04 5.04" />
      <path d="M6.61 6.61A10.9 10.9 0 003 12c1.73 4.89 6 8 11 8a10.94 10.94 0 002.12-.2" />
    </svg>
  ) : (
    <svg
      width="20"
      height="20"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12z" />
      <circle cx="12" cy="12" r="3" />
    </svg>
  )}
</button>
              </div>
            </div>

            <button type="submit" className="login-btn">
              Sign In
              <span>→</span>
            </button>
          </form>

          <div className="divider">
            <span>OR</span>
          </div>

          <p className="register-text">
            Don't have an account?{' '}
            <Link to="/register">Create an account</Link>
          </p>

          <div className="secure">
            🔐 Your data is securely protected
          </div>
        </div>

      </div>
    </div>
  )
}