import { Navigate, Route, Routes } from 'react-router-dom'
import { AuthProvider, useAuth } from './auth'
import Login from './pages/Login'
import Register from './pages/Register'
import Layout from './components/Layout'
import Dashboard from './pages/Dashboard'
import Profile from './pages/Profile'

function Protected(){
  const {user}=useAuth()
  return user ? <Layout/> : <Navigate to="/login" replace/>
}
export default function App(){
  return <AuthProvider><Routes>
    <Route path="/login" element={<Login/>}/>
    <Route path="/register" element={<Register/>}/>
    <Route element={<Protected/>}>
      <Route path="/" element={<Dashboard/>}/>
      <Route path="/profile" element={<Profile/>}/>
    </Route>
    <Route path="*" element={<Navigate to="/" replace/>}/>
  </Routes></AuthProvider>
}
