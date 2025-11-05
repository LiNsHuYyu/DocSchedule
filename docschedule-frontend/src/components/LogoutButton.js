//共用登出按鈕
import { useAuth } from '../context/AuthContext'
import { useNavigate } from 'react-router-dom'

export default function LogoutButton() {
  const { logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()            // 清除登入狀態
    navigate('/login')  // 回登入頁
  }

  return (
    <button
      onClick={handleLogout}
      className="btn btn-outline-danger btn-sm"
      style={{ position: 'absolute', top: 20, right: 20 }}
    >
      登出
    </button>
  )
}
