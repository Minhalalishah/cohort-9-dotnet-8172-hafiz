export type Role = 'Admin' | 'User'
export type Status = 'Pending' | 'InProgress' | 'Completed'
export type Priority = 'Low' | 'Medium' | 'High' | 'Critical'

export interface User { id:number; fullName:string; email:string; role:Role }
export interface AuthResponse { token:string; userId:number; fullName:string; email:string; role:Role }
export interface Task {
  id:number; title:string; description:string; status:Status; priority:Priority;
  category:string; dueDate?:string; createdById:number; assignedToId?:number;
  assignedToName?:string; createdAt:string; updatedAt:string
}
