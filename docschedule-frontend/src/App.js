import { Routes, Route, Navigate } from 'react-router-dom'
import Login from './pages/Login'
import AdminSchedule from './pages/AdminSchedule'
import DoctorPreference from './pages/DoctorPreference'
import ProtectedRoute from './components/ProtectedRoute'

export default function App(){
  return (
    <Routes>
      <Route path = "/" element = {<Navigate to = "/login" replace />} />
      <Route path = "/login" element = {<Login />} />
      <Route
        path="/admin/schedule"
        element={
          <ProtectedRoute roles={['Admin']}>
            <AdminSchedule />
          </ProtectedRoute>
        }
      />
      <Route
        path="/doctor/preference"
        element={
          <ProtectedRoute roles={['Doctor']}>
            <DoctorPreference />
          </ProtectedRoute>
        }
      />
    </Routes>
  )
}