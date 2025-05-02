# 📦 Inventory Management System

A robust, modular, and testable Inventory Management System built with **ASP.NET Core** and **Entity Framework Core**. Designed to streamline warehouse operations, manage stock levels, and facilitate seamless inventory tracking.

---

## 🚀 Features

- **Product Management**: Add, update, and remove products with ease.
- **Stock Control**: Monitor stock levels across multiple warehouses.
- **Warehouse Management**: Manage multiple warehouse locations and their inventories.
- **Authentication & Authorization**: Secure access using JWT tokens.
- **Idempotent Operations**: Ensure safe and repeatable API requests.
- **Comprehensive Testing**: Unit and integration tests to ensure reliability.
- **Clean Architecture**: Separation of concerns with a layered architecture.
- **Caching**: Reduce the need to repeatedly query the database.
- **Pagination**: Improve response time and user experience.
- **CQRS and Vertical Slices**: Separate reads from writes and encapsulate each use case in a vertical slice for better scalability and maintainability.

---

## 🛠️ Technologies Used

- **Backend**: ASP.NET Core 7
- **Database**: Entity Framework Core with SQL Server
- **CQRS**: MediatR
- **Validation**: FluentValidation
- **Authentication**: JWT Bearer Tokens
- **Background Jobs**: HangFire
- **Testing**: xUnit, NSubstitute
- **API Documentation**: OpenAPI (Swagger)

---

## ⚙️ Getting Started

### 🧪 Setup Environment Variables

Set the following environment variables using `setx` (Windows):

```bash
setx IMS_Email "your_email@example.com"
setx IMS_Pass "your_google_app_password"
```

### Installation
Clone the repository:
```bash
git clone https://github.com/AbdelrahmanMustafa5227/InventoryManagementSystem.git
```

Update the connection string in appsettings.json:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=InventoryDB;Trusted_Connection=True;"
}
```

