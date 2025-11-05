//管理登入資訊
import { createContext, useContext, useState } from "react"

const AuthContext = createContext(null)
export const useAuth = () => useContext(AuthContext)

export function AuthProvider({ children }) {
    const [token, setToken] = useState(localStorage.getItem('token'))
    const [role, setRole] = useState(localStorage.getItem('role'))

    const login = (token, role) => {
        setToken(token)
        setRole(role)
        localStorage.setItem('token', token)
        localStorage.setItem('role', role)
    }

    const logout = () => {
        setToken(null)
        setRole(null)
        localStorage.removeItem('token')
        localStorage.removeItem('role')
    }

    return (
        <AuthContext.Provider value={{ token, role, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}