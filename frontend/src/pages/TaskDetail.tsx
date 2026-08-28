import { useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import api from '../api'
import type { Task } from '../types'
import '../css/app.css'

export default function TaskDetail() {
  const { id } = useParams()
  const [task, setTask] = useState<Task | null>(null)
  const nav = useNavigate()

  useEffect(() => {
    api
      .get<Task>(`/tasks/${id}`)
      .then(r => setTask(r.data))
      .catch(err => {
        console.error('Unable to load task:', err)
      })
  }, [id])

  if (!task) {
    return (
      <div className="card loading-card">
        <div className="loading-spinner" />
        <p className="muted">Loading task...</p>
      </div>
    )
  }

  const del = async () => {
    if (confirm('Delete this task?')) {
      await api.delete(`/tasks/${id}`)
      nav('/tasks')
    }
  }

  return (
    <div className="card task-detail">

      <div className="task-detail-header">

        <div>
          <h1>{task.title}</h1>

          <p className="muted">
            Task Details
          </p>
        </div>

        <div className="task-actions">

          <Link
            className="button"
            to={`/tasks/${id}/edit`}
          >
            Edit
          </Link>

          <button
            className="danger"
            onClick={del}
          >
            Delete
          </button>

        </div>

      </div>

      <div className="task-detail-description">
        {task.description || 'No description provided.'}
      </div>

      <hr />

      <div className="task-meta">

        <div className="task-meta-item">
          <label>Status</label>
          <span>{task.status}</span>
        </div>

        <div className="task-meta-item">
          <label>Priority</label>
          <span>{task.priority}</span>
        </div>

        <div className="task-meta-item">
          <label>Category</label>
          <span>{task.category}</span>
        </div>

        <div className="task-meta-item">
          <label>Assigned To</label>
          <span>
            {task.assignedToName || 'Unassigned'}
          </span>
        </div>

        <div className="task-meta-item">
          <label>Due Date</label>
          <span>
            {task.dueDate?.slice(0, 10) || 'No due date'}
          </span>
        </div>

      </div>

    </div>
  )
}