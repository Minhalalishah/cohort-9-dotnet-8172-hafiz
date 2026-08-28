import { FormEvent, useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import api from '../api'
import TaskFormFields from '../components/TaskFormFields'
import type { Task, Status, Priority } from '../types'
import '../css/app.css'

type TaskFormData = {
  title: string
  description: string
  status: Status
  priority: Priority
  category: string
  dueDate?: string
  assignedToId?: number
}

export default function TaskForm() {
  const { id } = useParams<{ id: string }>()
  const edit = Boolean(id)
  const navigate = useNavigate()

  const [task, setTask] = useState<Partial<Task>>({
    title: '',
    description: '',
    status: 'Pending',
    priority: 'Medium',
    category: 'General',
    dueDate: undefined,
    assignedToId: undefined,
  })

  const [error, setError] = useState('')
  const [loading, setLoading] = useState(edit)
  const [saving, setSaving] = useState(false)

  // --------------------------------------------------
  // Load task when editing
  // --------------------------------------------------
  useEffect(() => {
    if (!id) {
      setLoading(false)
      return
    }

    const loadTask = async () => {
      try {
        setLoading(true)
        setError('')

        const response = await api.get<Task>(`/tasks/${id}`)

        const data = response.data

        setTask({
          title: data.title ?? '',
          description: data.description ?? '',
          status: data.status ?? 'Pending',
          priority: data.priority ?? 'Medium',
          category: data.category ?? 'General',
          dueDate: data.dueDate
            ? data.dueDate.substring(0, 10)
            : undefined,
          assignedToId: data.assignedToId ?? undefined,
        })
      } catch (err: any) {
        console.error('Unable to load task:', err)

        const message = getErrorMessage(
          err,
          'Unable to load task.'
        )

        setError(message)
      } finally {
        setLoading(false)
      }
    }

    loadTask()
  }, [id])

  // --------------------------------------------------
  // Save / Update Task
  // --------------------------------------------------
  const submit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault()

    if (saving) {
      return
    }

    setError('')

    // Basic frontend validation
    if (!task.title?.trim()) {
      setError('Task title is required.')
      return
    }

    if (!task.description?.trim()) {
      setError('Task description is required.')
      return
    }

    if (!task.category?.trim()) {
      setError('Task category is required.')
      return
    }

    setSaving(true)

    try {
      // ------------------------------------------------
      // IMPORTANT:
      // Send only the fields expected by:
      //
      // CreateTaskRequest / UpdateTaskRequest
      // ------------------------------------------------
      const payload: TaskFormData = {
        title: task.title.trim(),
        description: task.description.trim(),
        status: task.status ?? 'Pending',
        priority: task.priority ?? 'Medium',
        category: task.category.trim(),
        dueDate: task.dueDate
          ? task.dueDate.substring(0, 10)
          : undefined,
        assignedToId:
          task.assignedToId !== undefined &&
          task.assignedToId !== null
            ? Number(task.assignedToId)
            : undefined,
      }

      console.log(
        edit ? 'Updating task:' : 'Creating task:',
        payload
      )

      // ------------------------------------------------
      // CREATE
      // ------------------------------------------------
      if (!edit) {
        const response = await api.post('/tasks', payload)

        console.log('Task created successfully:', response.data)

        navigate('/tasks')
        return
      }

      // ------------------------------------------------
      // UPDATE
      // ------------------------------------------------
      await api.put(`/tasks/${id}`, payload)

      console.log('Task updated successfully.')

      navigate(`/tasks/${id}`)
    } catch (err: any) {
      console.error(
        edit
          ? 'Unable to update task:'
          : 'Unable to create task:',
        err
      )

      setError(
        getErrorMessage(
          err,
          edit
            ? 'Unable to update task.'
            : 'Unable to save task.'
        )
      )
    } finally {
      setSaving(false)
    }
  }

  // --------------------------------------------------
  // Loading
  // --------------------------------------------------
  if (loading) {
    return (
      <div className="card">
        <h1>Loading Task...</h1>
        <p>Please wait.</p>
      </div>
    )
  }

  // --------------------------------------------------
  // UI
  // --------------------------------------------------
  return (
    <form className="card" onSubmit={submit}>
      <h1>{edit ? 'Edit Task' : 'New Task'}</h1>

      {error && (
        <div className="error" role="alert">
          {error}
        </div>
      )}

      <TaskFormFields
        task={task}
        setTask={setTask}
      />

      <button
        type="submit"
        disabled={saving}
      >
        {saving
          ? edit
            ? 'Updating...'
            : 'Saving...'
          : 'Save Task'}
      </button>
    </form>
  )
}

// --------------------------------------------------
// Extract useful backend errors
// --------------------------------------------------
function getErrorMessage(
  err: any,
  fallback: string
): string {
  // Network error
  if (!err?.response) {
    return 'Unable to connect to the backend. Make sure the ASP.NET Core API is running on http://localhost:5000.'
  }

  const data = err.response.data

  // ASP.NET validation response
  if (data?.errors) {
    const messages: string[] = []

    Object.values(data.errors).forEach((value: any) => {
      if (Array.isArray(value)) {
        value.forEach((message) => {
          messages.push(String(message))
        })
      } else if (value) {
        messages.push(String(value))
      }
    })

    if (messages.length > 0) {
      return messages.join(' ')
    }
  }

  // Custom backend message
  if (data?.message) {
    return String(data.message)
  }

  // ASP.NET ProblemDetails
  if (data?.detail) {
    return String(data.detail)
  }

  // ASP.NET title
  if (data?.title) {
    return String(data.title)
  }

  // HTTP status
  if (err.response.status) {
    return `${fallback} Server returned ${err.response.status}.`
  }

  return fallback
}