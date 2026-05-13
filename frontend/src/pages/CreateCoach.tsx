import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import api from '../services/api'
import type { AxiosError } from 'axios'

type CoachForm = {
  name: string
  age: number
  experienceYrs: number
  teamId: number
}

const emptyForm: CoachForm = {
  name: '',
  age: 30,
  experienceYrs: 0,
  teamId: 1,
}

export const CreateCoach: React.FC = () => {
  const [form, setForm] = useState<CoachForm>({ ...emptyForm })
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)
  const navigate = useNavigate()

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError(null)
    setSuccess(null)
    try {
      await api.post('/coaches', form)
      setSuccess('Coach created successfully.')
      setForm({ ...emptyForm })
    } catch (err) {
      const axiosErr = err as AxiosError<any>
      let msg = ''

      const data = axiosErr.response?.data
      if (typeof data === 'string') {
        msg = data
      } else if (data?.errors) {
        msg = Object.values(data.errors).flat().join(', ')
      } else if (data?.title) {
        msg = data.title
      }

      setError('Unable to create coach. Check the form values and try again. ' + msg)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Create Coach</h2>
        <Link to="/admin/coaches">Back to Coaches</Link>
      </div>
      <form onSubmit={submit} className="stack">
        <label htmlFor="create-coach-name">Name</label>
        <input
          id="create-coach-name"
          value={form.name}
          onChange={(e) => setForm({ ...form, name: e.target.value })}
          placeholder="Name"
        />
        <label htmlFor="create-coach-age">Age</label>
        <input
          id="create-coach-age"
          type="number"
          value={form.age}
          onChange={(e) => setForm({ ...form, age: Number(e.target.value) })}
        />
        <label htmlFor="create-coach-exp">Experience (years)</label>
        <input
          id="create-coach-exp"
          type="number"
          value={form.experienceYrs}
          onChange={(e) => setForm({ ...form, experienceYrs: Number(e.target.value) })}
        />
        <label htmlFor="create-coach-team">Team ID</label>
        <input
          id="create-coach-team"
          type="number"
          value={form.teamId}
          onChange={(e) => setForm({ ...form, teamId: Number(e.target.value) })}
        />
        <div className="button-row">
          <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Create Coach'}</button>
          <button type="button" onClick={() => navigate('/admin/coaches')} disabled={loading}>Cancel</button>
        </div>
        {success && <p className="success" role="status">{success}</p>}
        {error && <p className="error" role="alert">{error}</p>}
      </form>
    </div>
  )
}
