//react 元件檔
import React from "react";
import "./TopNav.css";

export default function TopNav({
  appName = "醫院系統",
  centerLabel = "暫定班表",
  role = "Admin", // "Admin" | "Doctor"
  userName = "admin",
  onLogout = () => {},
}) {
  const rightLabel = role === "Admin" ? `管理者：${userName}` : `醫師：「${userName}」`;

  return (
    <header className="topnav">
      <div className="topnav__inner">
        {/* Left: Brand */}
        <div className="topnav__brand" title={appName}>{appName}</div>

        {/* Center: Page label */}
        <div className="topnav__center" title={centerLabel}>{centerLabel}</div>

        {/* Right: User + Logout */}
        <div className="topnav__right">
          <span className="topnav__user" title={rightLabel}>{rightLabel}</span>
          <button className="topnav__logout" onClick={onLogout} aria-label="登出">登出</button>
        </div>
      </div>
    </header>
  );
}