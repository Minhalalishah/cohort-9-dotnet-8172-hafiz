import { useEffect,useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../api'
import type { Task,Status,Priority } from '../types'
import '../css/app.css'

export default function Tasks(){const [tasks,setTasks]=useState<Task[]>([]);const [search,setSearch]=useState('');const [status,setStatus]=useState('');const [priority,setPriority]=useState('')
 const load=()=>api.get<Task[]>('/tasks',{params:{search:search||undefined,status:status||undefined,priority:priority||undefined}}).then(r=>setTasks(r.data))
 useEffect(()=>{load()},[])
 return <><header className="row">
 <div>
   <h1>Tasks</h1>
   <p className="muted">
     Manage assigned and created tasks
   </p>
 </div>

 <Link
   className="button"
   to="/tasks/new"
 >
   + New Task
 </Link>
</header>
 <div className="filters"><input placeholder="Search..." value={search} onChange={e=>setSearch(e.target.value)}/><select value={status} onChange={e=>setStatus(e.target.value)}><option value="">All status</option><option value="Pending">Pending</option><option value="InProgress">In Progress</option><option value="Completed">Completed</option></select><select value={priority} onChange={e=>setPriority(e.target.value)}><option value="">All priority</option><option>Low</option><option>Medium</option><option>High</option><option>Critical</option></select><button onClick={load}>Filter</button></div>
 <div className="table"><div className="tr head"><span>Title</span><span>Status</span><span>Priority</span><span>Category</span><span>Due</span></div>{tasks.map(t=><Link className="tr" key={t.id} to={`/tasks/${t.id}`}><span>{t.title}</span><span>{t.status}</span><span>{t.priority}</span><span>{t.category}</span><span>{t.dueDate?.slice(0,10)||'—'}</span></Link>)}{tasks.length===0&&<p className="empty">No tasks found.</p>}</div></>}
