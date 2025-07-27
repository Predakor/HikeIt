# 🧩 Full-Stack Project: ASP.NET + SPA + Supabase

This is a full-stack project combining:

- **Backend:** ASP.NET Core Web API
- **Frontend:** SPA (React/Vue/etc)
- **Database:** Local Supabase instance (PostgreSQL) via Docker

---

## 📦 Prerequisites

Make sure the following tools are installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js (LTS)](https://nodejs.org/)
- [Docker & Docker Compose](https://docs.docker.com/get-docker/)

---

## 📁 Project Structure

```
/your-repo
  /backend      # ASP.NET Core Web API
  /frontend     # Single Page Application (SPA)
  /supabase     # Docker-based Supabase setup
  .env          # Shared local environment variables
```

---

## ⚙️ 1. Environment Configuration

Create a `.env` file in the root of the project with the following content:

```env
# Shared

# Backend
DB_CONNECTION_STRING=Host=localhost;Port=54322;Database=postgres;Username=postgres;Password=postgres
BACKEND_URL=https://localhost:5001

# Frontend
VITE_BACKEND_URL=https://localhost:5001
VITE_FRONTEND_URL=http://localhost:3000
```

---

## 🐳 2. Start Supabase Locally

```bash
cd supabase
docker compose up -d
```

- **Postgres:** `localhost:54322`
- **Supabase Studio (optional):** `http://localhost:54323`

---

## 🧱 3. Run Backend (ASP.NET Core)

```bash
cd backend
dotnet restore
dotnet watch run
```

- Runs at: `https://localhost:5001`
- Auto-reloads on code changes (`dotnet watch`)
- Config values pulled from `.env` or `appsettings.Development.json`

---

## 🎨 4. Run Frontend (SPA)

```bash
cd frontend
npm install
npm run dev
```

- Runs at: `http://localhost:3000`
- Connects to backend via `VITE_BACKEND_URL`

---

## ✅ Access the App

Once all services are running:

- Frontend: [http://localhost:3000](http://localhost:3000)
- Backend API: [https://localhost:5001](https://localhost:5001)
- Supabase Studio (optional): [http://localhost:54323](http://localhost:54323)

---

## 🧹 Common Scripts

| Action             | Command                        |
|--------------------|--------------------------------|
| Start backend      | `dotnet watch run`             |
| Start frontend     | `npm run dev`                  |
| Start Supabase     | `docker compose up -d`         |
| Stop Supabase      | `docker compose down`          |

---

## 📝 Notes

- Ensure ports `54322`, `54323`, `5001`, and `3000` are available.
- If using HTTPS locally, you may need to trust the ASP.NET dev certificate:

```bash
dotnet dev-certs https --trust
```

---

Happy hacking! 🚀
