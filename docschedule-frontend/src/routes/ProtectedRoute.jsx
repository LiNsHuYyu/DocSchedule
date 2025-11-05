//沒 token 或已過期 → 轉回 /login
import React from "react";
import { Navigate } from "react-router-dom";
import { getToken, isTokenExpired, getUserFromToken } from "../utils/jwt";

export default function ProtectedRoute({ children, requireRole }) {
  const token = getToken();
  if (!token || isTokenExpired(token)) {
    return <Navigate to="/login" replace />;
  }
  if (requireRole) {
    const user = getUserFromToken();
    const role = user?.role;
    if (Array.isArray(requireRole)) {
      if (!requireRole.includes(role)) return <Navigate to="/login" replace />;
    } else {
      if (role !== requireRole) return <Navigate to="/login" replace />;
    }
  }
  return children;
}