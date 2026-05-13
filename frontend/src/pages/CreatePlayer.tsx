import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import api from '../services/api'
import type { AxiosError } from 'axios'

type PlayerForm = {
  name: string
  age: number
  position: string
  marketValue: number
  teamId: number
}

const emptyForm: PlayerForm = {
  name: '',
  age: 18,
  position: '',
  marketValue: 0,
  teamId: 1,
}

export const CreatePlayer: React.FC = () => {
  const [form, setForm] = useState<PlayerForm>({ ...emptyForm })
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
      await api.post('/players', form)
      setSuccess('Player created successfully.')
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

      setError('Unable to create player. Check the form values and try again. ' + msg)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Create Player</h2>
        <Link to="/admin/players">Back to Players</Link>
      </div>
      <form onSubmit={submit} className="stack">
        <label htmlFor="create-player-name">Name</label>
        <input
          id="create-player-name"
          value={form.name}
          onChange={(e) => setForm({ ...form, name: e.target.value })}
          placeholder="Name"
        />
        <label htmlFor="create-player-age">Age</label>
        <input
          id="create-player-age"
          type="number"
          value={form.age}
          onChange={(e) => setForm({ ...form, age: Number(e.target.value) })}
        />
        <label htmlFor="create-player-position">Position</label>
        <input
          id="create-player-position"
          value={form.position}
          onChange={(e) => setForm({ ...form, position: e.target.value })}
          placeholder="Position"
        />
        <label htmlFor="create-player-market">Market value</label>
        <input
          id="create-player-market"
          type="number"
          value={form.marketValue}
          onChange={(e) => setForm({ ...form, marketValue: Number(e.target.value) })}
        />
        <label htmlFor="create-player-team">Team ID</label>
        <input
          id="create-player-team"
          type="number"
          value={form.teamId}
          onChange={(e) => setForm({ ...form, teamId: Number(e.target.value) })}
        />
        <div className="button-row">
          <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Create Player'}</button>
          <button type="button" onClick={() => navigate('/admin/players')} disabled={loading}>Cancel</button>
        </div>
        {success && <p className="success" role="status">{success}</p>}
        {error && <p className="error" role="alert">{error}</p>}
      </form>
    </div>
  )
}
