import { FormEvent, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../auth'

export default function Register() {
  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [showPassword, setShowPassword] = useState(false)
  const [error, setError] = useState('')

  const { register } = useAuth()
  const nav = useNavigate()

  const submit = async (e: FormEvent) => {
    e.preventDefault()
    setError('')

    if (password !== confirmPassword) {
      setError('Passwords do not match')
      return
    }

    try {
      await register(name, email, password)
      nav('/login')
    } catch (err: any) {
      setError(err.response?.data?.message || 'Registration failed')
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
            Start managing your tasks smarter
            and make every day more productive.
          </p>

          <div className="feature">
            <span>✓</span>
            <div>
              <strong>Simple & Powerful</strong>
              <small>Everything you need to manage tasks</small>
            </div>
          </div>

          <div className="feature">
            <span>✓</span>
            <div>
              <strong>Stay Productive</strong>
              <small>Focus on what matters most</small>
            </div>
          </div>

          <div className="feature">
            <span>✓</span>
            <div>
              <strong>Work Smarter</strong>
              <small>Organize your workflow effortlessly</small>
            </div>
          </div>
        </div>

        {/* Register Card */}
        <div className="auth-card register-card">

          <div className="mobile-logo">
            <div className="brand-icon">✓</div>
            <h2>TaskFlow</h2>
          </div>

          <div className="auth-header">
            <h2>Create Account 🚀</h2>
            <p>Join TaskFlow and start organizing your work</p>
          </div>

          {error && (
            <div className="error">
              <span>⚠</span>
              {error}
            </div>
          )}

          <form onSubmit={submit}>

            <div className="input-group">
              <label>Full Name</label>

              <div className="input-wrapper">
                <span className="input-icon">👤</span>

                <input
                  type="text"
                  placeholder="Enter your full name"
                  value={name}
                  onChange={e => setName(e.target.value)}
                  required
                />
              </div>
            </div>

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
              <label>Password</label>

              <div className="input-wrapper">
                <span className="input-icon">🔒</span>

                <input
                  type={showPassword ? 'text' : 'password'}
                  placeholder="Create a password"
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

            <div className="input-group">
              <label>Confirm Password</label>

              <div className="input-wrapper">
                <span className="input-icon">🔒</span>

                <input
                  type={showPassword ? 'text' : 'password'}
                  placeholder="Confirm your password"
                  value={confirmPassword}
                  onChange={e => setConfirmPassword(e.target.value)}
                  required
                />
              </div>
            </div>

            <button type="submit" className="login-btn">
              Create Account
              <span>→</span>
            </button>

          </form>

          <div className="divider">
            <span>OR</span>
          </div>

          <p className="register-text">
            Already have an account?{' '}
            <Link to="/login">Sign in</Link>
          </p>

          <div className="secure">
            🔐 Your information is securely protected
          </div>

        </div>

      </div>
    </div>
  )
}