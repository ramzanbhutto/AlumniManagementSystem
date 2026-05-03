# Alumni Management System

A full-stack web application for managing alumni records, events, donations and job
opportunities. Built with .NET 10, Blazor Web App (Blazor Server + Blazor WASM) and
MySQL, following **CLEAN ARCHITECTURE** principles for strict separation of concerns.

---

## Table of Contents

- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Features](#features)
- [Prerequisites](#prerequisites)
  - [Install .NET 10](#install-net-10)
  - [Install EF Core CLI](#install-ef-core-cli)
  - [Install MySQL](#install-mysql)
- [Getting Started](#getting-started)
  - [Clone & Restore](#clone--restore)
  - [Configure the Database](#configure-the-database)
  - [Migrations](#migrations)
  - [Build & Run](#build--run)
- [Running the Application](#running-the-application)
- [Accessing the App](#accessing-the-app)
- [User Registration Rules](#user-registration-rules)
- [Role-Based Access](#role-based-access)
- [API Overview](#api-overview)
- [Known Quirks & Fixes](#known-quirks--fixes)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor WebAssembly (.NET 10) |
| UI Components | MudBlazor 9.4.0 |
| Backend | ASP.NET Core Web API (.NET 10) |
| ORM | Entity Framework Core (Pomelo MySQL provider) |
| Database | MySQL 8+ |
| Auth | JWT Bearer Tokens (60-minutes expiry, HS256) |
| Password Hashing | BCrypt.Net-Next (work factor 12) |
| Architecture | Clean Architecture |

---

## Architecture

The solution follows Clean Architecture with strict dependency rules — outer layers depend
on inner layers, never the reverse. Domain has zero dependencies.

```
AlumniManagementSystem/
├── AlumniManagementSystem.Domain/              # Layer 1 — innermost, no dependencies
│   └── ...                                     # Pure C# entity classes (16 entities) + Enums
│
├── AlumniManagementSystem.Application/         # Layer 2 — depends only on Domain
│   ├── Interfaces/                             # Repository & service contracts (IAlumniRepository, IAuthService, etc.)
│   └── Services/                               # Business logic orchestration (AlumniService, EventService, etc.)
│
├── AlumniManagementSystem.Infrastructure/      # Layer 3 — depends on Application
│   ├── Repositories/                           # Concrete EF Core + LINQ implementations
│   ├── Services/                               # AuthService (BCrypt hashing + JWT generation)
│   └── Migrations/                             # EF Core migration files
│
├── AlumniManagementSystem.Shared/              # No dependencies — pure DTOs
│   └── ...                                     # AlumniDto, LoginDto, EventDto, etc.
│                                               # Referenced by both API and Web.Client
│
├── AlumniManagementSystem.Api/                 # Layer 4 — ASP.NET Core Web API
│   ├── Controllers/                            # 14 REST controllers
│   └── Properties/                             # launchSettings.json
│
└── AlumniManagementSystem.Web/                 # Presentation layer
    ├── AlumniManagementSystem.Web/             # Blazor Server host (serves WASM to browser)
    │   ├── Components/                         # App.razor, layout host
    │   └── wwwroot/                            # Static assets
    └── AlumniManagementSystem.Web.Client/      # Blazor WASM SPA (runs in browser)
        ├── Pages/                              # All UI pages (Dashboard, Events, Jobs, etc.)
        │   └── Admin/                          # Admin-only pages (Departments, Newsletter, Reports)
        ├── Services/                           # HTTP client services (call the API)
        ├── Layout/                             # MainLayout, NavMenu
        ├── Shared/                             # Reusable Blazor components
        └── wwwroot/                            # WASM static assets
```

**Dependency flow (outermost → innermost):**
```
Web.Client      →  Shared
Web             →  Web.Client, Shared
API             →  Application, Infrastructure, Shared
Infrastructure  →  Application
Application     →  Domain
Domain          →  (nothing)
```

---

## Features

**Admin**
- Full user management (view, delete, role assignment)
- View all alumni profiles and activity
- Manage events, donations, job postings
- Department & program management
- Newsletters (draft → send to target audience)
- Dashboard with system-wide statistics and reports

**Alumni**
- Register and manage personal profile
- Browse and RSVP to events
- Track donation history
- Browse and post job listings
- Inbox messaging system
- Take surveys

**Guest**
- View public alumni directory (limited fields)
- View upcoming public events
- Register for an account

---

## Prerequisites

### Install .NET 10

**Arch Linux**
```bash
sudo pacman -S dotnet-sdk
# verify
dotnet --version
```

If the pacman version is behind .NET 10, use the AUR:
```bash
yay -S dotnet-sdk-bin
```

**Ubuntu / Debian**
```bash
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-10.0
dotnet --version
```

**Windows**

Download the .NET 10 SDK installer from https://dotnet.microsoft.com/download and run it. After installation open a new terminal and run:
```powershell
dotnet --version
```

**macOS**
```bash
brew install dotnet
dotnet --version
```

---

### Install EF Core CLI

After .NET is installed, install the EF Core global tool:
```bash
dotnet tool install --global dotnet-ef
```

If it's already installed and you need to update:
```bash
dotnet tool update --global dotnet-ef
```

Verify:
```bash
dotnet ef --version
```

> Make sure `~/.dotnet/tools` is in your `PATH`. On Arch/Linux add this to `~/.zshrc`:
> ```bash
> export PATH="$HOME/.dotnet/tools:$PATH"
> source ~/.zshrc
> ```

---

### Install MySQL

**Arch Linux**
```bash
sudo pacman -S mysql
sudo mysqld --initialize --user=mysql
sudo systemctl enable --now mysqld
sudo grep 'temporary password' /var/log/mysql/mysqld.log
mysql_secure_installation
```

**Ubuntu**
```bash
sudo apt install mysql-server
sudo systemctl enable --now mysql
sudo mysql_secure_installation
```

**Windows** — download MySQL Community Server from https://dev.mysql.com/downloads/installer/ and follow the installer.

**macOS**
```bash
brew install mysql
brew services start mysql
mysql_secure_installation
```

---

## Getting Started

### Clone & Restore

```bash
git clone https://github.com/ramzanbhutto/AlumniManagementSystem.git
cd AlumniManagementSystem

# Restore all NuGet packages
dotnet restore
```

---

### Configure the Database

1. Create the database in MySQL:
```sql
CREATE DATABASE AlumniDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

2. Copy `appsettings.example.json` to `appsettings.json` inside `AlumniManagementSystem.Api/`:
```bash
cp AlumniManagementSystem.Api/appsettings.example.json AlumniManagementSystem.Api/appsettings.json
```

3. Edit `appsettings.json` and fill in your MySQL password:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=AlumniDB;User=root;Password=YOUR_PASSWORD;"
  },
  "Jwt": {
    "Key": "YourSecretKeyAtLeast32CharsLong!!",
    "Issuer": "AlumniManagementSystem",
    "Audience": "AlumniManagementSystemUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> Default MySQL port is 3306. Never commit real credentials — `appsettings.json` is gitignored.

---

### Migrations

All EF Core commands are run from the solution root. Only the first two are needed to get the project running.

**Apply existing migrations to the database (creates all tables):**
```bash
dotnet ef database update \
  --project AlumniManagementSystem.Infrastructure \
  --startup-project AlumniManagementSystem.Api
```

**Add a new migration (only if you changed an entity):**
```bash
dotnet ef migrations add MigrationName \
  --project AlumniManagementSystem.Infrastructure \
  --startup-project AlumniManagementSystem.Api
```

**List all migrations:**
```bash
dotnet ef migrations list \
  --project AlumniManagementSystem.Infrastructure \
  --startup-project AlumniManagementSystem.Api
```

**Remove the last migration (if not yet applied):**
```bash
dotnet ef migrations remove \
  --project AlumniManagementSystem.Infrastructure \
  --startup-project AlumniManagementSystem.Api
```

**Drop the database entirely (dev only):**
```bash
dotnet ef database drop \
  --project AlumniManagementSystem.Infrastructure \
  --startup-project AlumniManagementSystem.Api
```

---

### Build & Run

**Clean build artifacts:**
```bash
dotnet clean
```

**Build the entire solution:**
```bash
dotnet build
```

---

## Running the Application

The application requires two terminals running simultaneously.

**Terminal 1 — Start the API:**
```bash
cd AlumniManagementSystem.Api
dotnet run
```

**Terminal 2 — Start the Blazor frontend:**
```bash
cd AlumniManagementSystem.Web/AlumniManagementSystem.Web
dotnet run
```

> Both must be running at the same time. The frontend makes HTTP requests to the API — if the API is not running, all data operations will fail.

---

## Accessing the App

| Service | URL |
|---|---|
| Frontend(App) | http://localhost:5193 |
| API | http://localhost:5016 |
| Swagger UI | http://localhost:5016/swagger |

---

## User Registration Rules

| Rule | Requirement |
|---|---|
| Minimum length | At least 6 characters |
| Uppercase letter | At least 1 (A–Z) |
| Lowercase letter | At least 1 (a–z) |
| Number | At least 1 (0–9) |
| Special character | At least 1 (`!@#$%^&*`) |

**Other rules:**
- Email must be unique and in valid format (e.g. `user@example.com`)
- First name and last name are required
- Graduation year must be a valid 4-digit year and not in the future

---

## Role-Based Access

| Role | What they can do |
|---|---|
| **Admin** | Full access — manage users, view all data, assign roles, manage events/donations/jobs |
| **Alumni** | Manage own profile, RSVP events, track donations, browse job board, messaging, surveys |
| **Guest** | Read-only access to public directory and public events; can register |

New accounts are assigned the **Alumni** role by default. An Admin must manually promote a user via the admin panel or directly in the database:

```sql
UPDATE Users SET Role = 'Admin' WHERE Email = 'your@email.com';
```

---

## API Overview

The API exposes **14 REST controllers** under `http://localhost:5016/api/`:

| Controller | Base Route | Description |
|---|---|---|
| Auth | `/api/auth` | Login, register |
| Users | `/api/users` | User account management (Admin only) |
| Alumni | `/api/alumni` | Alumni profile CRUD |
| Events | `/api/events` | Event listing, creation, RSVP |
| Donations | `/api/donations` | Donation records and campaign tracking |
| Jobs | `/api/jobs` | Job postings board |
| Messages | `/api/messages` | Inbox, sent, mark as read |
| Surveys | `/api/surveys` | Survey creation and responses |
| Newsletter | `/api/newsletter` | Draft and send newsletters |
| Departments | `/api/departments` | Department management (Admin) |
| Programs | `/api/programs` | Program management (Admin) |
| Dashboard | `/api/dashboard` | Aggregated stats (Admin) |
| Search | `/api/search` | Cross-entity search |
| Reports | `/api/reports` | Reports and aggregate data |

Swagger UI available at `http://localhost:5016/swagger` (Development mode only).

---

## Known Quirks & Fixes

**MudSelect, MudDialog, MudDatePicker broken in Blazor WASM**

MudBlazor 9.x has known issues with certain components under Blazor WASM. The following workarounds are used:

- `NativeSelect.razor` — replaces `MudSelect` with a native HTML `<select>`
- Native `<input type="date">` — replaces `MudDatePicker`
- Inline overlay divs — replace `MudDialog` for modal interactions

If you update MudBlazor and those components break, this is why.

**Rate limiting**

Three policies are configured in `Program.cs` to protect the API:

| Policy | Type | Limit | Applied To |
|---|---|---|---|
| `GlobalPolicy` | Fixed Window | 100 req/min | All endpoints |
| `AuthPolicy` | Fixed Window | 10 req/min | Login, Register (brute-force protection) |
| `ReportsPolicy` | Sliding Window | 20 req/min | Reports, Dashboard (heavy queries) |

If you hit a limit, the API returns `429 Too Many Requests`. During development, wait a moment or temporarily comment out `app.UseRateLimiter()` in `Program.cs`.

**JWT expiry**

Tokens expire after **60 minutes**. If the UI stops loading data, log out and log back in.

---

## License

This project is for academic and portfolio purposes.
