"use client";
import Image from "next/image";

export default function LoginPage() {
  return (
    <div
      style={{
        minHeight: "100vh",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        background: "#f7f7fa",
      }}
    >
      <div
        style={{
          display: "flex",
          flexDirection: "row",
          gap: "4rem",
          background: "#fff",
          borderRadius: "1.5rem",
          boxShadow: "0 4px 32px rgba(0,0,0,0.08)",
          padding: "3rem 2.5rem",
          alignItems: "center",
          maxWidth: 700,
          width: "100%",
        }}
      >
        <div style={{ minWidth: 220, textAlign: "center" }}>
          <Image
            src="/bibliokopke_full.png"
            alt="Logo BiblioKopke"
            width={220}
            height={220}
            style={{ objectFit: "contain", width: "100%", height: "auto" }}
            priority
          />
        </div>
        <form
          style={{
            display: "flex",
            flexDirection: "column",
            gap: "1.5rem",
            minWidth: 260,
            width: "100%",
          }}
          onSubmit={e => e.preventDefault()}
        >
          <div>
            <h2 style={{ fontWeight: 700, fontSize: "1.5rem", marginBottom: 8, color: "#1a1a1a" }}>
              Bem-vindo ao BiblioKopke
            </h2>
            <p style={{ color: "#666", fontSize: "1rem" }}>
              Faça login para acessar o sistema
            </p>
          </div>
          <div style={{ display: "flex", flexDirection: "column", gap: 12 }}>
            <label style={{ fontWeight: 500, color: "#333" }}>
              Usuário
              <input
                type="text"
                name="usuario"
                autoComplete="username"
                required
                style={{
                  width: "100%",
                  padding: "0.5rem 0.75rem",
                  border: "1px solid #ccc",
                  borderRadius: 6,
                  marginTop: 4,
                  fontSize: "1rem",
                }}
              />
            </label>
            <label style={{ fontWeight: 500, color: "#333" }}>
              Senha
              <input
                type="password"
                name="senha"
                autoComplete="current-password"
                required
                style={{
                  width: "100%",
                  padding: "0.5rem 0.75rem",
                  border: "1px solid #ccc",
                  borderRadius: 6,
                  marginTop: 4,
                  fontSize: "1rem",
                }}
              />
            </label>
          </div>
          <button
            type="submit"
            style={{
              background: "#1a237e",
              color: "#fff",
              fontWeight: 600,
              border: "none",
              borderRadius: 6,
              padding: "0.75rem",
              fontSize: "1.1rem",
              cursor: "pointer",
              marginTop: 8,
              transition: "background 0.2s",
            }}
          >
            Entrar
          </button>
        </form>
      </div>
    </div>
  );
}
