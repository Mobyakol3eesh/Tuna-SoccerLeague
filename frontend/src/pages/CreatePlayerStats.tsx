import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import api from '../services/api'
import type { AxiosError } from 'axios'

type PlayerStatsForm = {
  playerId: number
  matchId: number
  goals: number
  assists: number
  shotsOnTarget: number
  touches: number
  passesCompleted: number
  score: number
}

const emptyForm: PlayerStatsForm = {
  playerId: 1,
  matchId: 1,
  goals: 0,
  assists: 0,
  shotsOnTarget: 0,
  touches: 0,
  passesCompleted: 0,
  score: 0,
}

export const CreatePlayerStats: React.FC = () => {
  const [form, setForm] = useState<PlayerStatsForm>({ ...emptyForm })
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
      await api.post('/players/player-stats', form)
      setSuccess('Player stats created successfully.')
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

      setError('Unable to create player stats. Check the form values and try again. ' + msg)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Create Player Stats</h2>
        <Link to="/admin/player-stats">Back to Player Stats</Link>
      </div>
      <form onSubmit={submit} className="stack">
        <label htmlFor="create-stats-player">Player ID</label>
        <input
          id="create-stats-player"
          type="number"
          value={form.playerId}
          onChange={(e) => setForm({ ...form, playerId: Number(e.target.value) })}
        />
        <label htmlFor="create-stats-match">Match ID</label>
        <input
          id="create-stats-match"
          type="number"
          value={form.matchId}
          onChange={(e) => setForm({ ...form, matchId: Number(e.target.value) })}
        />
        <label htmlFor="create-stats-goals">Goals</label>
        <input
          id="create-stats-goals"
          type="number"
          value={form.goals}
          onChange={(e) => setForm({ ...form, goals: Number(e.target.value) })}
        />
        <label htmlFor="create-stats-assists">Assists</label>
        <input
          id="create-stats-assists"
          type="number"
          value={form.assists}
          onChange={(e) => setForm({ ...form, assists: Number(e.target.value) })}
        />
        <label htmlFor="create-stats-sot">Shots on target</label>
        <input
          id="create-stats-sot"
          type="number"
          value={form.shotsOnTarget}
          onChange={(e) => setForm({ ...form, shotsOnTarget: Number(e.target.value) })}
        />
        <label htmlFor="create-stats-touches">Touches</label>
        <input
          id="create-stats-touches"
          type="number"
          value={form.touches}
          onChange={(e) => setForm({ ...form, touches: Number(e.target.value) })}
        />
        <label htmlFor="create-stats-passes">Passes completed</label>
        <input
          id="create-stats-passes"
          type="number"
          value={form.passesCompleted}
          onChange={(e) => setForm({ ...form, passesCompleted: Number(e.target.value) })}
        />
        <label htmlFor="create-stats-score">Score</label>
        <input
          id="create-stats-score"
          type="number"
          value={form.score}
          onChange={(e) => setForm({ ...form, score: Number(e.target.value) })}
        />
        <div className="button-row">
          <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Create Stats'}</button>
          <button type="button" onClick={() => navigate('/admin/player-stats')} disabled={loading}>Cancel</button>
        </div>
        {success && <p className="success" role="status">{success}</p>}
        {error && <p className="error" role="alert">{error}</p>}
      </form>
    </div>
  )
}
