# 🏋️ Gym Management System

A comprehensive **Gym Management System** built with **ASP.NET Core MVC** and **Entity Framework Core**, designed to manage gym operations such as members, trainers, plans, sessions, and related business processes.

The project is implemented using a clean **N-Tier Architecture** to separate responsibilities between the Presentation, Business Logic, and Data Access layers.

---

## 📌 Project Overview

The Gym Management System provides a structured platform for managing the main operations of a gym.

The application focuses not only on implementing CRUD operations, but also on applying software architecture principles and common design patterns to make the system:

* Maintainable
* Scalable
* Testable
* Reusable
* Easy to extend
* Clearly separated by responsibility

---

## 🏗️ Architecture

The project follows an **N-Tier Architecture**, divided into three main layers:

```text
┌───────────────────────────────────────────┐
│           Presentation Layer              │
│          GymManagement (MVC)              │
│                                           │
│  • Controllers   • Views                  │
└─────────────────────┬─────────────────────┘
                      │
                      ▼
┌───────────────────────────────────────────┐
│           Business Logic Layer            │
│              GymManagement.BLL            │
│                                           │
│ Services • Business Rules • ViewModels    │
│ AutoMapper • Result Handling              │
└─────────────────────┬─────────────────────┘
                      │
                      ▼
┌───────────────────────────────────────────┐
│           Data Access Layer               │
│              GymManagement.DAL            │
│                                           │
│ EF Core • DbContext • Repositories        │
│ Generic Repository • Specific Repository  │
│ Unit of Work                              │
└─────────────────────┬─────────────────────┘
                      │
                      ▼
                SQL Database
```

### 🔹 1. Presentation Layer

**Project:** `GymManagement`

Responsible for handling the application's user interface and HTTP requests.

Main responsibilities:

* MVC Controllers
* Razor Views
* Presentation ViewModels
* Model Binding
* Validation
* User interaction
* Authentication/Authorization integration

The Presentation Layer communicates with the **Business Logic Layer** instead of directly accessing the database.

The project contains controllers, views, models, migrations, static files, and application configuration.

---

### 🔹 2. Business Logic Layer

**Project:** `GymManagement.BLL`

This layer contains the application's business rules and application logic.

Main responsibilities:

* Business rules
* Application services
* Validation of business operations
* Coordinating repositories
* Mapping between Entities and ViewModels
* Returning operation results

The BLL contains service classes such as:

```text
Analytics.cs
MemberServices.cs
PlanServices.cs
SessionServices.cs
TrainerServices.cs
```

It also contains:

```text
Interface/
ViewModels/
Common/
MappingProfile.cs
```

This keeps business logic separated from controllers and database implementation.
---

### 🔹 3. Data Access Layer

**Project:** `GymManagement.DAL`

The Data Access Layer is responsible for communication with the database.

Main responsibilities:

* Entity Framework Core
* DbContext
* Database configuration
* Migrations
* Repositories
* Data seeding
* Database operations

The DAL contains:

```text
Data/
DataSessding/
Repositories/
```

The repository structure is further divided into interfaces and implementations.

---

# 🔄 Layer Communication

The application follows a controlled dependency flow:

```text
Presentation
     │
     ▼
    BLL
     │
     ▼
    DAL
     │
     ▼
  Database
```

For example, when updating a gym plan:

```text
Controllers
    ↓
 Services
    ↓
 UnitOfWork 
    ↓
Repositories
    ↓
Entity Framework Core
    ↓
Database
```

The Controller does not contain database logic, and the Repository does not contain business rules.

This separation makes each layer responsible for one specific concern.

---
## 📝 Architectural Note — ViewModels & DTOs

> **Note:** In a larger and more complex application, the recommended approach would be to keep **ViewModels in the Presentation Layer** and use **DTOs in the Business Logic Layer**.

### Recommended Architecture

Normally, the responsibility of each object would be separated as follows:

```text
┌─────────────────────────────────────────────┐
│           Presentation Layer               │
│                                            │
│  Controllers                               │
│  ViewModels                                │
│  Views                                     │
└──────────────────────┬──────────────────────┘
                       │
                       │ DTOs
                       ▼
┌─────────────────────────────────────────────┐
│         Business Logic Layer               │
│                                            │
│  Services                                  │
│  Business Rules                            │
│  DTOs                                      │
└──────────────────────┬──────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────┐
│            Data Access Layer               │
│                                            │
│  Repositories                              |
|  Specific Repository                       |
│  Unit Of Work                              |
│  EF Core                                   │
└─────────────────────────────────────────────┘
```

In this approach:

* **DTOs** belong to the Business Logic/Application layer and are used to transfer data between the Business Logic Layer and the Presentation Layer.
* **ViewModels** belong to the Presentation Layer because they are specifically designed to prepare and shape data for the UI/View.
* The Presentation Layer receives the required data through DTOs and then maps or transforms that data into ViewModels according to what the View needs.

For example:

```text
Entity
   ↓
Repository
   ↓
Service
   ↓
DTO
   ↓
Presentation Layer
   ↓
ViewModel
   ↓
View
```

This separation becomes more valuable as the application grows because the DTOs represent the application's data contract, while ViewModels remain specific to the UI.

---

### 📌 Why Is It Different in This Project?

In this project, **ViewModels are located inside the Business Logic Layer** rather than having separate DTOs in the BLL and ViewModels in the Presentation Layer.

This was an **intentional architectural decision based on the scope and size of the project**.

For a small application like this one, introducing both:

```text
BLL → DTOs → Presentation → ViewModels → Views
```

would add another layer of mapping and additional classes without providing enough practical benefit.

In other words, implementing a complete DTO/ViewModel separation for this project would be considered **over-engineering relative to its current scope**.

Therefore, the project uses:

```text
Business Logic Layer
        │
        └── ViewModels
              │
              ▼
       Presentation Layer
              │
              ▼
             Views
```

Because the **Presentation Layer references the Business Logic Layer**, the Presentation Layer can consume these ViewModels directly.

At the same time, the Business Logic Layer can use the same ViewModels when receiving input from or returning data to the Presentation Layer.

### ⚖️ Architectural Trade-off

This approach provides a simpler implementation for the current project:

* Less code
* Fewer mapping operations
* Fewer classes
* Easier development
* Appropriate for the project's current size and scope

However, for a **larger enterprise-level application**, separating DTOs from ViewModels would be preferable:

```text
BLL
 │
 └── DTOs
       │
       ▼
Presentation
 │
 └── ViewModels
       │
       ▼
     Views
```

This would provide stronger separation of concerns and prevent the Business Logic Layer from becoming coupled to UI-specific models.

> **Conclusion:** The current implementation is a deliberate simplification appropriate for this project's small scope. It should not be considered the only or universally recommended way to structure ViewModels and DTOs.


# 🧩 Design Patterns

The project uses several important design patterns to improve maintainability and reduce code duplication.

---

## 1. Generic Repository Pattern

The project implements a **Generic Repository Pattern** through:

```text
IGenaricRepository<TEntity>
        ↓
GenaricRepository<TEntity>
```

The repository provides reusable database operations that can work with different entities.

Typical operations include:

```text
GetAllAsync()
GetByIdAsync()
Add()
Update()
Delete()
```

Instead of creating the same CRUD implementation for every entity, the generic repository provides common functionality that can be reused across the application.

### Example

Instead of implementing:

```text
MemberRepository
PlanRepository
TrainerRepository
SessionRepository
```

for every basic CRUD operation, the generic repository can provide the common operations once.

### Benefits

* Reduces duplicated code
* Provides reusable data-access functionality
* Centralizes common CRUD operations
* Makes repositories easier to maintain
* Keeps services independent from EF Core implementation details

The repository interfaces and implementations are explicitly separated inside the DAL.

---

# 2. Specific Repository Pattern

When an entity requires operations that are **specific to its business/data requirements**, a specific repository is used.

The project contains:

```text
ISessionRepository
        ↓
SessionRepository
```

This allows specialized database operations to exist without adding entity-specific logic to the generic repository.

### Why use a Specific Repository?

The Generic Repository handles common operations:

```text
Get
Add
Update
Delete
```

But sometimes an entity needs specialized queries.

For example:

```text
GetAllSessionWithTrainerAndCategory(...)
GetSessionWithTrainerAndCategory(...)
GetCountOfSlotsAsync(...)
```

These operations belong to the specific repository because they are related specifically to the `Session` entity.

The project currently contains `ISessionRepository` and `SessionRepository` alongside the generic repository implementation.

---

# 3. Unit of Work Pattern

The project also implements the **Unit of Work Pattern**:

```text
IUnitOfWork
     ↓
UnitOfWork
```

The Unit of Work coordinates multiple repositories and provides a atomic transaction for committing changes.

The DAL repository structure contains both `IUnitOfWork` and `UnitOfWork`.

### Why Unit of Work?

Imagine a business operation requires changing:

```text
Member
Membership
Payment
```

These operations should ideally be committed together.

The Unit of Work provides a centralized mechanism for coordinating these operations and saving changes as one unit.

Conceptually:

```text
Service
   │
   ├── Member Repository
   ├── Plan Repository
   └── Session Repository
             │
             ▼
        Unit of Work
             │
             ▼
        Save Changes
```

This helps maintain consistency when multiple database operations belong to the same business operation.

---

# 4. Result Pattern

The application uses the **Result Pattern** to represent the outcome of business operations in a structured way.

Instead of relying only on exceptions or primitive return values such as:

```csharp
bool
null
```

the Result Pattern can provide a consistent representation of:

```text
Success
Failure
Error Message
Returned Data
Not Found
Validation
```

Conceptually:

```text
Result
 ├── Ok
 ├── Faile
 ├── Notfound
 ├── Validation
 └── Value
```

This provides a cleaner communication mechanism between the Business Logic Layer and the Presentation Layer.

For example:

```text
Service
   ↓
Result<T>
   ↓
Controller
   ↓
Success / Error Response
```

### Benefits

* Consistent error handling
* Clear success/failure communication
* Reduces unnecessary exception handling
* Makes service responses easier to consume
* Separates business operation results from HTTP concerns

---

# 🔀 AutoMapper

The project uses **AutoMapper** to simplify object-to-object mapping.

The BLL contains:

```text
MappingProfile.cs
```

and the BLL project references:

```text
AutoMapper
```

with version `16.2.0`.

### Why AutoMapper?

The application works with different types of objects, such as:

```text
Entity
   ↓
ViewModel
```

and:

```text
ViewModel
   ↓
Entity
```

Instead of manually mapping every property:

```csharp
member.Name = model.Name;
member.Email = model.Email;
member.Phone = model.Phone;
```

AutoMapper allows these mappings to be configured centrally.

### Benefits

* Reduces repetitive mapping code
* Keeps controllers/services cleaner
* Centralizes mapping configuration
* Makes DTO/ViewModel transformations easier to maintain

---

# 🔐 ASP.NET Core Identity

The application uses **ASP.NET Core Identity** for authentication and authorization.

Identity provides the infrastructure required for managing application users and access control.

It can handle responsibilities such as:

* User authentication
* Password management
* User accounts
* Roles
* Authorization
* Login/logout
* Secure credential management

This allows authentication and authorization concerns to be handled by the framework instead of implementing a custom authentication system from scratch.

---

# 🛠️ Technologies & Packages

## Backend

* **ASP.NET Core MVC**
* **.NET 9**
* **C#**
* **Entity Framework Core**
* **ASP.NET Core Identity**

The projects target **.NET 9.0**.

## Libraries / Packages

### AutoMapper

Used for mapping between:

```text
Entities ↔ ViewModels
```

Version:

```text
AutoMapper 16.2.0
```

### ASP.NET Core Identity

Used for:

```text
Authentication
Authorization
User Management
Roles
```

### Entity Framework Core

Used as the ORM responsible for communication between the application and the relational database.

---

# 📂 Project Structure

```text
GymManagementSystem/
│
├── GymManagement.PL/
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── Pages/
│   ├── Migrations/
│   ├── MemberPhotos/
│   ├── wwwroot/
│   ├── Program.cs
│   ├── ProgramExtention.cs
│   └── GymManagement.PL.csproj
│
├── GymManagement.BLL/
│   ├── Class/
│   │   ├── Analytics.cs
│   │   ├── MemberServices.cs
│   │   ├── PlanServices.cs
│   │   ├── SessionServices.cs
│   │   └── TrainerServices.cs
│   │
│   ├── Interface/
│   ├── ViewModels/
│   ├── Common/
│   ├── AttachMemnt/
│   ├── MappingProfile.cs
│   └── GymManagement.BLL.csproj
│
├── GymManagement.DAL/
│   ├── Data/
│   ├── DataSessding/
│   │
│   ├── Repositories/
│   │   ├── Classes/
│   │   │   ├── GenaricRepository.cs
│   │   │   ├── SessionRepository.cs
│   │   │   └── UnitOfWork.cs
│   │   │
│   │   └── Interfaces/
│   │       ├── IGenaricRepository.cs
│   │       ├── ISessionRepository.cs
│   │       └── IUnitOfWork.cs
│   │
│   └── GymManagement.DAL.csproj
│
└── GymManagement.sln
```

The current GitHub repository contains the three main projects and the solution file shown above.

---

# 🧠 Separation of Responsibilities

One of the main goals of the architecture is to prevent different responsibilities from being mixed together.

| Layer        | Responsibility                       |
| ------------ | ------------------------------------ |
| Presentation | HTTP Requests, Controllers, Views    |
| BLL          | Business Rules and Application Logic |
| DAL          | Database Access                      |
| Repository   | Encapsulates Data Access             |
| Unit of Work | Coordinates Repository Operations    |
| AutoMapper   | Object Mapping                       |
| Identity     | Authentication & Authorization       |

---

# 🔗 Example Request Flow

A typical request can flow through the application like this:

```text
User
 │
 ▼
MVC Controller
 │
 ▼
Business Service
 │
 ▼
Unit of Work
 │
 ▼
Generic / Specific Repository
 │
 ▼
Entity Framework Core
 │
 ▼
SQL Database
```

After processing:

```text
SQL Database
     │
     ▼
Repository
     │
     ▼
Business Service
     │
     ▼
AutoMapper
     │
     ▼
ViewModel
     │
     ▼
Controller
     │
     ▼
Razor View
     │
     ▼
User
```

---

# 🎯 Main Architectural Goals

The architecture was designed around the following principles:

### Separation of Concerns

Each layer has a clear responsibility.

### Reusability

Generic Repository provides reusable database operations.

### Maintainability

Business logic is separated from controllers and database code.

### Scalability

New entities and business features can be added without significantly changing existing layers.

### Testability

Business logic and data access are separated through interfaces, making individual components easier to test and mock.

### Clean Dependency Flow

The Presentation Layer depends on the BLL, while the BLL depends on the DAL abstractions/implementation required by the application.

---

# 🚀 Getting Started

## Prerequisites

Make sure you have installed:

* .NET 9 SDK
* Visual Studio 2022 or later
* SQL Server
* SQL Server Management Studio (optional)

---

## Clone the Repository

```bash
git clone https://github.com/joo877/GymManagementSystem.git
```

Then:

```bash
cd GymManagementSystem
```

---

## Database Configuration

Configure your SQL Server connection string inside:

```text
GymManagement/appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  }
}
```

> Replace the connection string with your local SQL Server configuration.

---

## Apply Migrations

From the solution directory:

```bash
dotnet ef database update
```

Or use the Package Manager Console in Visual Studio:

```powershell
Update-Database
```

---

## Run the Application

```bash
dotnet run
```

Or open:

```text
GymManagement.sln
```

with Visual Studio and run the `GymManagement` project.

---

# 📚 Architectural Summary

The project can be summarized as:

```text
                   Gym Management System
                           │
                           ▼
                  ┌─────────────────┐
                  │ Presentation    │
                  │ ASP.NET MVC     │
                  └────────┬────────┘
                           │
                           ▼
                  ┌─────────────────┐
                  │      BLL        │
                  │ Services        │
                  │ Business Rules  │
                  │ AutoMapper      │
                  │ Result Pattern  │
                  └────────┬────────┘
                           │
                           ▼
                  ┌─────────────────┐
                  │      DAL        │
                  │ EF Core         │
                  │ Generic Repo    │
                  │ Specific Repo   │
                  │ Unit of Work    │
                  └────────┬────────┘
                           │
                           ▼
                     SQL Database
```

---

# 💡 Key Patterns & Technologies

| Category                 | Used Technology / Pattern |
| ------------------------ | ------------------------- |
| Architecture             | N-Tier Architecture       |
| Web Framework            | ASP.NET Core MVC          |
| ORM                      | Entity Framework Core     |
| Authentication           | ASP.NET Core Identity     |
| Mapping                  | AutoMapper                |
| Data Access              | Repository Pattern        |
| Generic Data Access      | Generic Repository        |
| Specialized Data Access  | Specific Repository       |
| Transaction Coordination | Unit of Work              |
| Operation Handling       | Result Pattern            |
| Database                 | SQL Server                |
| Framework                | .NET 9                    |
| Language                 | C#                        |

---

## 👨‍💻 Author

**Youssef Said**

GitHub:

[Gym Management System Repository](https://github.com/joo877/GymManagementSystem?utm_source=chatgpt.com)

---

## ⭐ Project Purpose

This project was developed to demonstrate practical experience with **ASP.NET Core MVC**, **Entity Framework Core**, layered architecture, repository-based data access, business-service design, authentication, authorization, and reusable software design patterns.
