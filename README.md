# 🏔️ HikeIt: Full-Stack Hiking App

HikeIt is a full-stack web application for planning, tracking, and analyzing hiking trips.  
It features a modern SPA frontend and a robust ASP.NET Core backend, with PostgreSQL (via Supabase) as the database.

---
## 📑 Table of Contents

1. [Features](#-features)
2. [Demo](#demo)  
3. [Tech Stack](#%EF%B8%8F-tech-stack)  
   - [Backend](#backend)  
   - [Frontend](#frontend)
4. [How To Run](#-how-to-run)  
   - [Requirements](#%EF%B8%8F-1-requirements)  
   - [Download and Initialize the Project](#download-and-initialize-the-project)  
   - [Environment Configuration](#%EF%B8%8F-environment-configuration)  
     - [Backend](#backend)  
     - [Frontend](#frontend)  
   - [Start Application](#start-application)
5. [Access the App](#-access-the-app)  
6. [Notes](#-notes)
   
---
## 🚀 Features

- **Authentication & Authorization**
  - Register, login, logout with ASP.NET Identity
  - Role-based authorization (User, Admin, …)

- **User Management**
  - Profile editing (personal info, avatar, stats)
  - Rank and achievement system

- **Trip Management**
  - Create, update, delete, and list hiking trips
  - File uploads (GPX files for route tracking)
  - Analytics (distance, elevation, time, peaks reached)

- **Data & Exploration**
  - Region and peak browsing with filters
  - Track progress across regions/peaks

- **Developer Experience**
  - RESTful API with Swagger docs
  - Modular architecture (Domain, Application, Infrastructure, API)
  - Environment-based config (development/production)
  - Database seeding with demo/admin users
  - Dockerized Supabase (PostgreSQL)
  - Responsive SPA frontend with React Query state management

---

## 🛠️ Tech Stack

### Backend
- Framework: ASP.NET Core Web API (.NET 8)
- ORM: Entity Framework Core
- Identity: ASP.NET Identity
- Database: PostgreSQL (Supabase, Dockerized locally)

#### Architecture
- Domain Driven Design
- In-memory background events queue
- Result pattern with functional extensions

### Frontend
- Framework: React
- Language: TypeScript
- UI: Chakra UI
- State Management: React Query
- Routing: React Router
- Build Tool: Vite

---

## 🐳 How To Run

### ⚙️ 1. Requirements
 Make sure you have all of the bellow on your machine
- .NET 8 SDK  
- Node.js >= 18  
- pnpm >= 9  
- Supabase CLI (installed globally)  
- Docker & Docker Compose  

### Download and Initialize the project
  //insert required steps here
- pnpx supabase init
- Instal the backend dependencies
- Instal fronted dependencies pnpm install
 
  
### ⚙️ Environment Configuration

#### Backend
Add user secrets in development. In production, provide env variables using `__` instead of `:`:

- `"Cors:AllowedOrigins": "allowedOriginsUrl"`
- `"ConnectionStrings:TripDbCS": "dbConnectionString"`
- `"Storage:Account": "azureBlobStorageName"`
- `"Storage:Key": "azureBlobStorageKey"`
- `"Seeding:Users:Admin": "jsonStringWithAdminCredentials"`

### Frontend
 Create a .env.production file with
- VITE_API_ORIGIN=backendUrl
- VITE_API_PREFIX=api

 Create a  .env file for local 
- VITE_API_ORIGIN=http://localhost:5063
- VITE_API_PREFIX=api
  
---

### Start application

- pnpx supabase start
- Start backend
- Start fronted pnpm run dev
---

## ✅ Access the App

Once all services are running:

- Frontend: [http://localhost:3000](http://localhost:3000)
- Backend API: [https://localhost:5001](https://localhost:5001)
- Supabase Studio (optional): [http://localhost:54323](http://localhost:54323)

---

## 📝 Notes

- Ensure ports `54322`, `54323`, `5001`, and `3000` are available.
- If using HTTPS locally, you may need to trust the ASP.NET dev certificate:

```bash
dotnet dev-certs https --trust
```
