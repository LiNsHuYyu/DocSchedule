import { useState } from "react"
import { useNavigate } from "react-router-dom"
import http from "../api/http"
import { useAuth } from "../context/AuthContext"

//這是什麼？
export default function Login() {
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState('')
    const { login } = useAuth()
    const navigate = useNavigate()

    const submit = async (e) => {
        e.preventDefault()
        setError('')
        setLoading(true)
        try{
            //API路徑 POST/auth/login 
            const { data } = await http.post('/auth/login', { username, password})
            //data裡可能還有token
            login(data.token, data.role)

            //如果還有role,可以根據role導向不同頁面
            if (data.role === 'Admin') {
                navigate('/admin/schedule')
            } else {
                navigate('/doctor/preference')
            }
        } catch (err) {
            //顯示error
            const msg = err?.response?.data?.message || '登入失敗，請檢查帳號密碼'
            setError(msg)
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="container" style={{maxWidth: 420, marginTop: 80}}>
            <h2 className="mb-4">登入</h2>
            <form onSubmit={submit}>
                <div className="mb-3">
                    <label className="form-label">帳號</label>
                    <input
                        className="form-control"
                        value={username}
                        onChange={e => setUsername(e.target.value)}
                        autoComplete="username"
                        required
                    />
                </div>

                <div className="mb-3">
                    <label className="form-label">密碼</label>
                    <input
                        className="form-control"
                        type="password"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        autoComplete="current-password"
                        required
                    />
                </div>

                {error && <div className="alert alert-danger py-2">{error}</div>}

                <button className="btn btn-primary w-100" disabled={loading}>
                    {loading ? '登入中...' : '登入'}
                </button>
            </form>
        </div>
    )
}