import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import api from '../services/api'
import type { AxiosError } from 'axios'

type MatchForm = {
  date: string
  location: string
  homeTeamId: number
  awayTeamId: number
  homeTeamScore: number
  awayTeamScore: number
}

const emptyForm: MatchForm = {
  date: new Date().toISOString().slice(0, 10),
  location: '',
  homeTeamId: 1,
  awayTeamId: 2,
  homeTeamScore: 0,
  awayTeamScore: 0,
}

export const CreateMatch: React.FC = () => {
  const [form, setForm] = useState<MatchForm>({ ...emptyForm })
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
      const payload = { ...form, date: new Date(form.date) }
      await api.post('/matches', payload)
      setSuccess('Match created successfully.')
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

      setError('Unable to create match. Check the form values and try again. ' + msg)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Create Match</h2>
        <Link to="/admin/matches">Back to Matches</Link>
      </div>
      <form onSubmit={submit} className="stack">
        <label htmlFor="create-match-date">Date</label>
        <input
          id="create-match-date"
          type="date"
          value={form.date}
          onChange={(e) => setForm({ ...form, date: e.target.value })}
        />
        <label htmlFor="create-match-location">Location</label>
        <input
          id="create-match-location"
          value={form.location}
          onChange={(e) => setForm({ ...form, location: e.target.value })}
          placeholder="Location"
        />
        <label htmlFor="create-match-home-team">Home team ID</label>
        <input
          id="create-match-home-team"
          type="number"
          value={form.homeTeamId}
          onChange={(e) => setForm({ ...form, homeTeamId: Number(e.target.value) })}
        />
        <label htmlFor="create-match-away-team">Away team ID</label>
        <input
          id="create-match-away-team"
          type="number"
          value={form.awayTeamId}
          onChange={(e) => setForm({ ...form, awayTeamId: Number(e.target.value) })}
        />
        <label htmlFor="create-match-home-score">Home team score</label>
        <input
          id="create-match-home-score"
          type="number"
          value={form.homeTeamScore}
          onChange={(e) => setForm({ ...form, homeTeamScore: Number(e.target.value) })}
        />
        <label htmlFor="create-match-away-score">Away team score</label>
        <input
          id="create-match-away-score"
          type="number"
          value={form.awayTeamScore}
          onChange={(e) => setForm({ ...form, awayTeamScore: Number(e.target.value) })}
        />
        <div className="button-row">
          <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Create Match'}</button>
          <button type="button" onClick={() => navigate('/admin/matches')} disabled={loading}>Cancel</button>
        </div>
        {success && <p className="success" role="status">{success}</p>}
        {error && <p className="error" role="alert">{error}</p>}
      </form>
    </div>
  )
}
