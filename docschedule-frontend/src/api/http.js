import axios from "axios";
import { getToken, isTokenExpired, clearToken } from "../utils/jwt";

const http = axios.create({
    baseURL: "http://localhost:5175/api",
});

http.interceptors.request.use((config) => {
    const token = getToken();
    if (token && !isTokenExpired(token)) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

http.interceptors.response.use(
    (res) => res,
    (err) => {
        if (err?.response?.status === 401) {
            // Token 失效 → 清除並導回登入
            clearToken();
            window.location.href = "/login";
        }
        return Promise.reject(err);
    }
);

export default http;