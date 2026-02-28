# DCAVIA Padel

<div align="center">

![Padel](https://img.shields.io/badge/🏸_PADEL-Court_Booking_System-blue?style=for-the-badge)
![DDD](https://img.shields.io/badge/Architecture-Domain_Driven_Design-purple?style=for-the-badge)
![VIA](https://img.shields.io/badge/VIA_University-DCA1_Course-red?style=for-the-badge)

---

### CI/CD Pipeline Status

| Pipeline | Status |
|----------|--------|
| 🧪 **Tests & Coverage** | ![.NET Tests](https://github.com/mixs1234/DCAVIA_Padel/actions/workflows/dotnet_tests.yml/badge.svg) |
| 🏗️ **Build** | ![Build](https://github.com/mixs1234/DCAVIA_Padel/actions/workflows/build.yml/badge.svg) |
| 🧹 **Code Quality** | ![Code Quality](https://github.com/mixs1234/DCAVIA_Padel/actions/workflows/code_quality.yml/badge.svg) |
| 🔒 **Security Audit** | ![Security](https://github.com/mixs1234/DCAVIA_Padel/actions/workflows/security_audit.yml/badge.svg) |
| 📊 **Repo Stats** | ![Stats](https://github.com/mixs1234/DCAVIA_Padel/actions/workflows/repo_stats.yml/badge.svg) |

---

### Tech Stack

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C# 12](https://img.shields.io/badge/C%23-12-239120?style=flat-square&logo=csharp&logoColor=white)
![xUnit](https://img.shields.io/badge/xUnit-Testing-512BD4?style=flat-square)
![GitHub Actions](https://img.shields.io/badge/CI-GitHub_Actions-2088FF?style=flat-square&logo=githubactions&logoColor=white)

![Repo Size](https://img.shields.io/github/repo-size/mixs1234/DCAVIA_Padel?style=flat-square&label=Repo%20Size)
![Last Commit](https://img.shields.io/github/last-commit/mixs1234/DCAVIA_Padel?style=flat-square&label=Last%20Commit)
![Commit Activity](https://img.shields.io/github/commit-activity/w/mixs1234/DCAVIA_Padel?style=flat-square&label=Commits%2FWeek)
![Top Language](https://img.shields.io/github/languages/top/mixs1234/DCAVIA_Padel?style=flat-square)
![Languages](https://img.shields.io/github/languages/count/mixs1234/DCAVIA_Padel?style=flat-square&label=Languages)

</div>

---

## About

DCAVIA Padel is a domain-centric application for managing padel court bookings, daily schedules, player accounts, and VIP reservations. The project is an assignment for the DCA1 course from VIA University College.

---

## Solution Architecture

```
DCAVIA_Padel.sln
│
├── 📦 src/Core/
│   ├── DCAVIA_Padel.Core.Domain                          # Aggregates, Entities, Value Objects
│   └── Tools/
│       └── DCAVIA_Padel.Core.Tools.OperationResult       # Result pattern & typed error hierarchy
│
└── 🧪 Tests/
    ├── UnitTests                                          # xUnit domain logic tests
    └── TestUtils                                          # Shared test data & helpers
```

## Domain Aggregates

```mermaid
graph TB
    subgraph DailySchedule Aggregate
        DS[🏟️ DailySchedule]
        CT[CourtType]
        DS --> CT
    end

    subgraph Player Aggregate
        P[👤 Player]
        E[Email]
        VID[VIAID]
        P --> E
        P --> VID
    end

    subgraph Booking Aggregate
        B[📋 Booking]
        D[Duration]
        B --> D
    end

    subgraph ReservationQueue Aggregate
        RQ[📬 ReservationQueue]
    end

    B -.->|references| P
    B -.->|references| DS
    RQ -.->|references| B
```

## Error Hierarchy

```mermaid
classDiagram
    class ResultError {
        <<abstract>>
        +string ErrorCode
        +string Message
        +ToString() string
    }

    class ValidationError {
        +IReadOnlyList~ValidationDetail~ Details
    }

    class NotFoundError {
        +string EntityName
        +object Id
    }

    class ConflictError
    class UnauthorizedError
    class CompositeError {
        +IReadOnlyList~ResultError~ InnerErrors
    }

    ResultError <|-- ValidationError
    ResultError <|-- NotFoundError
    ResultError <|-- ConflictError
    ResultError <|-- UnauthorizedError
    ResultError <|-- CompositeError
```

## CI/CD Pipelines

This project runs **5 automated pipelines** on every push:

| Pipeline | What it does | Trigger |
|----------|-------------|---------|
| **🧪 Tests & Coverage** | Runs all xUnit tests, generates code coverage with Cobertura, publishes test results as check annotations, posts coverage summary as PR comment | Push & PR |
| **🏗️ Build** | Compiles Debug (with warnings-as-errors) and Release configurations | Push & PR |
| **🧹 Code Quality** | Runs `dotnet format --verify-no-changes` and builds with `EnforceCodeStyleInBuild` | Push & PR |
| **🔒 Security Audit** | Scans all NuGet dependencies for known vulnerabilities (also runs weekly on Monday) | Push, PR & Scheduled |
| **📊 Repo Stats** | Counts lines of code across all projects with `cloc` | Push to main |

## Getting Started

```bash
# Clone
git clone https://github.com/mixs1234/DCAVIA_Padel.git

# Restore & Build
dotnet restore DCAVIA_Padel.sln
dotnet build DCAVIA_Padel.sln

# Run all tests
dotnet test DCAVIA_Padel.sln

# Run tests with coverage
dotnet test DCAVIA_Padel.sln --collect:"XPlat Code Coverage"
```

---

<div align="center">

*Built with frustration, caffeine, and Domain-Driven Design*

![.NET](https://img.shields.io/badge/Powered_by-.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

</div>