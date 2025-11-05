// 小工具：專門處理 token 讀寫、解析與過期判斷
export function getToken() {
    return localStorage.getItem("token");
}

export function setToken(token) {
    localStorage.setItem("token", token);
}

export function clearToken() {
    localStorage.removeItem("token");
}

export function parseJwt(token) {
    try {
        const parts = token.split(".");
        if (parts.length !== 3) return null;
        const payload = JSON.parse(atob(parts[1].replace(/-/g, "+").replace(/_/g, "/")));
        return payload;
    } catch {
        return null;
    }
}

export function isTokenExpired(token) {
    const payload = parseJwt(token);
    if (!payload || !payload.exp) return true; // 沒 exp 視為過期
    const now = Math.floor(Date.now() / 1000);
    return payload.exp <= now;
}

export function getUserFromToken() {
    const token = getToken();
    if (!token || isTokenExpired(token)) return null;
    const p = parseJwt(token) || {};
  
    // 角色：涵蓋常見 .NET ClaimType
    const roleClaim =
      p.role ||
      (Array.isArray(p.roles) ? p.roles[0] : p.roles) ||
      p["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
      p["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role"] ||
      "";
  
    const role = Array.isArray(roleClaim) ? roleClaim[0] : roleClaim || "User";
  
    // 名字：優先用 username/unique_name；再來是 name/given_name；再來才是 sub/nameidentifier
    const name =
        p.username ||
        p.preferred_username ||
        p.unique_name ||
        p.name ||
    +   p.displayName ||   // ← 新增這行，對應剛剛自訂 claim
        p["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ||
        p.given_name ||
        p["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"] ||
        p.sub ||
        "使用者";
  
    // 如果拿到的是純數字（多半是 ID），就顯示「使用者」
    if (typeof name === "string" && /^\d+$/.test(name)) {
      name = "使用者";
    }
  
    return { name, role, raw: p };
}
  

export function debugTokenClaims() {
    const token = getToken();
    console.log("[JWT] raw token:", token);
    console.log("[JWT] parsed payload:", parseJwt(token));
}
  