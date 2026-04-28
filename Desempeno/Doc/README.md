#  Sports Space Reservation System

A C# console application for managing users, sports spaces, and reservations — built with .NET 10, Entity Framework Core, and MySQL.

---

##  Features

###  User Management
- Register users with name, email, phone, and document number
- Edit user information
- Unique document validation (no duplicates allowed)
- List all registered users

###  Sports Space Management
- Register spaces with name, type (football, basketball, pool, etc.), and capacity
- Edit space information
- Duplicate validation by ID or name
- List all spaces
- Filter spaces by type

###  Reservation Management
- Create reservations linked to a user and a sports space
- Validates overlapping time ranges per space and per user
- Validates that end time is after start time
- Prevents reservations on past dates or times
- Reservation statuses: `Programada`, `Cancelada`, `Finalizada`
- Cancel a reservation (changes status to `Cancelada`)
- List all reservations
- List reservations by user
- List reservations by sports space

### Email Notifications
- Sends a confirmation email to the user when a reservation is successfully created (via SMTP / Mailtrap)

---

##  Tech Stack

| Technology | Version |
|---|---|
| .NET | 10.0 |
| Entity Framework Core | 9.0 |
| Pomelo MySQL Provider | 9.0 |
| MySQL | 8.x |
| Microsoft.Extensions.DependencyInjection | 10.0 |

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL Server running (local or remote)
- Git

---

## Setup & Run

### 1. Clone the repository

```bash
git clone https://github.com/Mk1-mid/Prueba_cs.git
cd Prueba_cs/Desempeno
```

### 2. Configure the database connection

Edit `appsettings.json` with your MySQL credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=YOUR_HOST;port=3306;database=YOUR_DB;user=YOUR_USER;password=YOUR_PASSWORD;"
  }
}
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

---

##  Project Structure

```
Desempeno/
├── Data/
│   ├── AppDbContext.cs           # EF Core DbContext
│   └── AppDbContextFactory.cs   # Design-time factory for migrations
├── models/
│   ├── Users.cs                  # User entity
│   ├── SportSpace.cs             # Sports space entity
│   └── Reserv.cs                 # Reservation entity
├── repositories/
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   ├── ISportSpaceRepository.cs
│   ├── SportSpaceRepository.cs
│   ├── IReservationRepository.cs
│   └── ReservationRepository.cs
├── services/
│   ├── UserServices.cs
│   ├── SportSpaceServices.cs
│   ├── ReservationServices.cs
│   └── ServiceResponse.cs        # Generic response wrapper
├── Migrations/                   # EF Core auto-generated migrations
├── Program.cs                    # Entry point & console menu
└── appsettings.json              # Configuration file
```

---

## Architecture

The project follows a **3-layer architecture**:

```
Program.cs (UI / Console Menu)
      ↓
  Services (Business Logic + Validations)
      ↓
  Repositories (Data Access via EF Core)
      ↓
  MySQL Database
```

- **Repository pattern** with interfaces for each entity
- **Dependency Injection** via `Microsoft.Extensions.DependencyInjection`
- **ServiceResponse\<T\>** as a generic wrapper for all operation results
- **LINQ** queries for filtering and overlap detection

---

## Business Rules

- A user's document number must be unique
- A sports space name must be unique
- A space cannot have two overlapping reservations on the same date
- A user cannot have two overlapping reservations on the same date
- Reservations cannot be created on past dates or times
- Cancelled reservations cannot be edited

---

##  Author

**Miguel** — Software Engineering Student  
RIWI / Tecnológico de Antioquia  
GitHub: [@Mk1-mid](https://github.com/Mk1-mid)  
LinkedIn: [miguelcallemk1](https://linkedin.com/in/miguelcallemk1)
