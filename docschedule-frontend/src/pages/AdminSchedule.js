import React, { useMemo } from "react";
import TopNav from "../components/TopNav";
import { getUserFromToken, clearToken, debugTokenClaims } from "../utils/jwt";  //debugTokenClaims之後刪
import http from "../api/http";

export default function AdminSchedule() {
  const me = useMemo(() => getUserFromToken(), []);
  React.useEffect(() => { debugTokenClaims(); }, []);  //debug用 之後刪

  const handleLogout = () => {
    clearToken();
    window.location.href = "/login";
  };

  // 呼叫需要 JWT 的 API（自動夾帶 Bearer）
  // React.useEffect(() => {
  //   http.get("/schedules/draft").then(res => console.log(res.data));
  // }, []);

  return (
    <>
      <TopNav
        appName="醫院系統"
        centerLabel="暫定班表"
        role={me?.role || "Admin"}      // 吃token
        userName={me?.name || "使用者"} // 吃token
        onLogout={handleLogout}
      />
      <main style={{maxWidth: 1200, margin: "24px auto", padding: "0 16px"}}>
        <h1>管理者 — 暫定班表</h1>
      </main>
    </>
  );
}