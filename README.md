Here's the combined version — backend + frontend in one README, trimmed down to what's essential rather than restating everything we discussed in detail.
markdown# 📚 Library Management System

A full-stack library management app — ASP.NET Core Web API backend + Angular frontend, in one monorepo.

## 🛠️ Tech Stack

**Backend**
- C# / ASP.NET Core (Clean Architecture: API / Application / Domain / Infrastructure)
- Custom JWT auth — access token + httpOnly-cookie refresh token, with rotation
- EF Core + PostgreSQL
- Swagger / OpenAPI
- Docker

**Frontend**
- Angular 19 (standalone components, signals)
- Angular Material
- Signal-based `AuthService`, role derived from JWT claims
- Functional route guards & HTTP interceptors
- Docker (Nginx)

## 🚀 Features
- Book management (add/edit/soft-delete) — borrow history is preserved, never destroyed
- Borrow / return flow with per-user history
- Role-based access — `ADMIN`, `LIBRARIAN`, `MEMBER`
- Server-side pagination & search on book listings
- Silent token refresh — expired access tokens renew automatically, no forced re-login
- Global error handling — backend returns RFC 7807 `ProblemDetails`; frontend surfaces them via snackbar
- Consistent loading / empty / error states across all pages (generic `StateView` wrapper component)
- Admin account auto-seeded on startup

## 📁 Project Structure
LibraryManagementSystem/

├── src/                        # Backend (ASP.NET Core)

│   ├── LMS.API/                # Controllers, middleware

│   ├── LMS.Application/        # Services, DTOs, settings

│   ├── LMS.Domain/             # Entities, enums, interfaces

│   └── LMS.Infrastructure/     # EF Core, repositories, JWT/security

│

├── lms-ui/                     # Frontend (Angular)

│   └── src/app/

│       ├── core/                # guards, interceptors, services, models

│       ├── features/            # auth, books, borrow, admin

│       └── shared/components/   # book-card, confirm-dialog, state-view, etc.

│

├── docker-compose.yml          # api + db + ui

└── Dockerfile                  # backend image

## 🔑 API Endpoints
| Method | Endpoint | Description |
|---|---|---|
| POST | /api/User | Register |
| POST | /api/User/login | Login (sets refresh-token cookie) |
| POST | /api/User/refresh | Exchange refresh token for a new access token |
| POST | /api/User/logout | Revoke refresh token & clear cookie |
| PATCH | /api/User/{id}/role | Update role *(Admin)* |
| GET | /api/Book | Paginated, searchable book list |
| GET | /api/Book/{id} | Get a book |
| POST | /api/Book | Add a book *(Admin/Librarian)* |
| PATCH | /api/Book/{id} | Update a book *(Admin/Librarian)* |
| DELETE | /api/Book/{id} | Soft-delete a book *(Admin/Librarian)* |
| POST | /api/borrow-records | Borrow a book |
| GET | /api/borrow-records/{id} | Get a borrow record |
| GET | /api/borrow-records/my | Current user's borrow history |
| POST | /api/borrow-records/return | Return a book |

## ⚙️ Configuration

**Backend** — `appsettings.Development.json` (gitignored):
```json
{
  "ConnectionStrings": { "DefaultConnection": "Host=localhost;Database=lms_db;Username=postgres;Password=your_password" },
  "JwtSettings": {
    "SecretKey": "your_secret_key",
    "Issuer": "LMS.API",
    "Audience": "LMS.API",
    "ExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "AdminSettings": { "Email": "admin@example.com", "Password": "your_password", "Name": "Admin" }
}
```

**Frontend** — `lms-ui/src/environments/environment.ts`:
```typescript
export const environment = { production: false, apiUrl: 'http://localhost:5000/api' };
```

**Docker** — same values supplied via `.env` at the repo root (see `docker-compose.yml`).

## ▶️ Running the Project

**Locally** (two terminals):
```bash
# Backend
dotnet restore
dotnet run --project src/LMS.API/LMS.API.csproj
# → http://localhost:5000  (Swagger at /swagger)

# Frontend
cd lms-ui
npm install
ng serve
# → http://localhost:4200
```

**Docker** (everything at once):
```bash
docker-compose up --build
```
Requires a `.env` file at the repo root with `DB_*`, `JWT_SECRET`, and `ADMIN_*` values.

## 🧪 Testing
Swagger UI (`/swagger`) for backend endpoints. For the auth/borrow flow end-to-end, use the Angular app or a real browser — the cookie-based refresh token won't behave realistically in pure cURL/Postman.

## 📌 Future Improvements
- Input validation (FluentValidation or Data Annotations) — DTOs currently have none
- Unit & integration tests
- HTTPS + `Secure` cookie flag for production
- Standardize controller route casing (`Book`/`User` vs `borrow-records`)
