import type { Dispatch, SetStateAction } from 'react'
import type { Task, Priority, Status } from '../types'
export default function TaskFormFields({task,setTask}:{task:Partial<Task>,setTask:Dispatch<SetStateAction<Partial<Task>>>}){
 return <div className="form-grid">
   <label>Title<input required value={task.title||''} onChange={e=>setTask({...task,title:e.target.value})}/></label>
   <label>Category<input value={task.category||''} onChange={e=>setTask({...task,category:e.target.value})}/></label>
   <label className="wide">Description<textarea value={task.description||''} onChange={e=>setTask({...task,description:e.target.value})}/></label>
   <label>Status<select value={task.status||'Pending'} onChange={e=>setTask({...task,status:e.target.value as Status})}><option>Pending</option><option>InProgress</option><option>Completed</option></select></label>
   <label>Priority<select value={task.priority||'Medium'} onChange={e=>setTask({...task,priority:e.target.value as Priority})}><option>Low</option><option>Medium</option><option>High</option><option>Critical</option></select></label>
   <label>Due date<input type="date" value={task.dueDate?.slice(0,10)||''} onChange={e=>setTask({...task,dueDate:e.target.value||undefined})}/></label>
 </div>
}
