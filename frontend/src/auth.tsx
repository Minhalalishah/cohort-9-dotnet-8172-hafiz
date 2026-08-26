import { createContext, useContext, useMemo, useState, type ReactNode } from 'react'
import type { AuthResponse, User } from './types'
import api from './api'

type AuthContextType = { user: User|null; login:(email:string,password:string)=>Promise<void>; register:(fullName:string,email:string,password:string)=>Promise<void>; logout:()=>void }
const AuthContext = createContext<AuthContextType>(null!)

function savedUser(): User|null {
  const raw = localStorage.getItem('user'); return raw ? JSON.parse(raw) : null
}
export function AuthProvider({children}:{children:ReactNode}) {
  const [user,setUser] = useState<User|null>(savedUser())
  const save = (r:AuthResponse) => {
    const u={id:r.userId,fullName:r.fullName,email:r.email,role:r.role}
    localStorage.setItem('token',r.token); localStorage.setItem('user',JSON.stringify(u)); setUser(u)
  }
  const value=useMemo(()=>({
    user,
    login:async(email:string,password:string)=>save((await api.post<AuthResponse>('/auth/login',{email,password})).data),
    register:async(fullName:string,email:string,password:string)=>save((await api.post<AuthResponse>('/auth/register',{fullName,email,password})).data),
    logout:()=>{localStorage.clear();setUser(null)}
  }),[user])
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
export const useAuth=()=>useContext(AuthContext)
