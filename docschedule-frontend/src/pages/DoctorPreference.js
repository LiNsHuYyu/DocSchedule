import React, { useMemo } from "react";
import TopNav from "../components/TopNav";
import { getUserFromToken, clearToken } from "../utils/jwt";
import http from "../api/http";

export default function DoctorPreference() {
  const me = useMemo(() => getUserFromToken(), []);

  const handleLogout = () => {
    clearToken();
    window.location.href = "/login";
  };

  return (
    <>
      <TopNav
        appName="醫院系統"
        centerLabel="選擇日期"
        role={me?.role || "Doctor"}     // 吃token
        userName={me?.name || "使用者"} // 吃token
        onLogout={handleLogout}
      />
      <main style={{maxWidth: 1200, margin: "24px auto", padding: "0 16px"}}>
        <h1>醫師 — 選擇日期</h1>
      </main>
    </>
  );
}