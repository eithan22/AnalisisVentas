# AnalisisVentas 📊

A **multi-source ETL Sales Analysis System** built with **ASP.NET Core 8** and **Entity Framework Core**. Integrates, processes, and analyzes sales data from multiple sources into a centralized analytical database for reporting and business decision-making.

---

## 📋 Description

AnalisisVentas implements a multi-layer architecture to process sales data via an ETL (Extract, Transform, Load) pipeline. The system exposes REST API endpoints for Customers and Products, backed by a SQL Server database accessed through EF Core and the Repository Pattern.

---

## ✨ Features

- 🔄 **ETL Pipeline** — Extract, transform, and load sales data from multiple sources
- 📦 **Product API** — RESTful endpoints for product data management
- 👥 **Customer API** — RESTful endpoints for customer data queries
- 🗄️ **Entity Framework Core** — Code-first database access with DbContext
- 🔌 **Repository Pattern** — Data access abstracted behind interfaces
- 📄 **Swagger UI** — Interactive API documentation

---

## 🛠️ Technologies

| Category | Technology |
|----------|-----------|
| Language | C# |
| Framework | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Architecture | Repository Pattern, Service Layer |
| API Docs | Swagger / OpenAPI |

---

## 🏗️ Architecture

```
AnalisisVentas/
├── AnalisisVentas.Api/           Presentation Layer
│   ├── Controllers/
│   │   ├── ClientesApiController.cs    GET /api/clientes
│   │   └── ProductosApiController.cs   GET /api/productos
│   ├── Data/
│   │   ├── Context/ApiContext.cs       EF Core DbContext
│   │   ├── Entities/Customer.cs        CustomerID, Name, Email, Phone, City, Country
│   │   ├── Entities/Product.cs         Product entity
│   │   ├── Interface/                  IApiClienteRepository, IApiProductoRepository
│   │   └── Repository/                 Repository implementations
│   └── Program.cs                      DI + EF Core setup
│
├── AnalisisVentas.Application/   Application Layer
│   ├── ETL/                            ETL service classes
│   └── Dtos/                           Data Transfer Objects
│
└── AnalisisVentas.Domain/        Domain Layer
    └── Entities/                       Core domain entities
```

---

## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/ClientesApi/GetClientes | Retrieve all customers |
| GET | /api/ProductosApi/GetProductos | Retrieve all products |

---

## 🚀 Installation

Prerequisites: .NET 8 SDK · SQL Server · Visual Studio 2022

```bash
git clone https://github.com/Eithan22/AnalisisVentas.git
cd AnalisisVentas
dotnet restore
# Set connection string in AnalisisVentas.Api/appsettings.json
dotnet ef database update --project AnalisisVentas.Api
dotnet run --project AnalisisVentas.Api
```

---

## 💡 Skills Demonstrated

- ✅ **ETL Architecture** — Multi-source data integration pipeline design
- ✅ **Entity Framework Core** — Code-first ORM with DbContext
- ✅ **Repository Pattern** — Interface-based data access abstraction
- ✅ **RESTful API Design** — Clean endpoints with proper HTTP semantics
- ✅ **ASP.NET Core 8** — Modern Web API with DI and Swagger
- ✅ **SQL Server** — Relational database design and querying

---

## 🔮 Future Improvements

- [ ] Additional ETL source connectors (CSV, Excel, external APIs)
- [ ] Data transformation pipeline with validation rules
- [ ] Analytical dashboards and reporting endpoints
- [ ] Pagination and filtering on list endpoints
- [ ] Unit tests for ETL service layer

---

## 👨‍💻 Author

**Eithan** — Backend Developer · Santo Domingo, Dominican Republic 🇩🇴
🎓 Software Development @ ITLA · 📧 eithanread1@gmail.com
[LinkedIn](https://linkedin.com/in/eithan-r) · [GitHub](https://github.com/Eithan22)

---

*MIT License*
