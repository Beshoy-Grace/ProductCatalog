
# 🛒 Product Catalog - ASP.NET Core Solution

## Overview

This is a full-stack ASP.NET Core solution split into two major projects:

- **ProductCatalog.API** - Backend API built with ASP.NET Core Web API.
- **ProductCatalog.MVC** - Frontend client built with ASP.NET Core MVC consuming the API.

The solution demonstrates clean architecture principles, Domain-Driven Design, and includes robust features like authentication, repository pattern, logging, pagination, and standardized API responses.

## 🔧 Technologies Used

- ASP.NET Core 8
- Entity Framework Core
- ASP.NET Identity
- SQL Server
- AutoMapper
- Swagger / OpenAPI
- Bootstrap (for UI)
- Repository + Unit of Work Pattern
- HttpClient with Token Authorization

## 📁 Project Structure

ProductCatalog.sln  
│  
├── ProductCatalog.API # ASP.NET Core Web API  
│ ├── Controllers  
│ ├── Middleware (logging, error handling)  
│ ├── Logs # Request/response logs  
│ ├── AutoMapper Profiles  
│ └── Program.cs / Startup.cs  
│  
├── ProductCatalog.MVC # ASP.NET Core MVC Client  
│ ├── Controllers  
│ ├── Views (Razor Pages)  
│ ├── wwwroot (Bootstrap UI)  
│ └── Startup.cs  
│  
├── ProductCatalog.Infrastructure # DBContext, Identity, Migrations  
├── ProductCatalog.Domain # Domain Models and Enums  
├── ProductCatalog.Application # Services, DTOs, Interfaces  
├── ProductCatalog.Framework # Core logic: UnitOfWork, Repository, Pagination, Response Models  
└── ProductCatalog.Service # HttpClient-based API Consumers used in MVC


## ✅ Features

### Authentication
- ASP.NET Core Identity
- JWT Token Authentication
- Login / Register from MVC with session token management

### API
- CRUD for Categories and Products
- Standardized JSON responses with status handling
- Logging for every API request/response (logs saved under `/Logs`)
- Swagger documentation

### MVC Client
- Clean UI with Bootstrap
- Authenticated views and navigation
- Full CRUD for Category and Product (with pagination)
- Token passed automatically with every API request
- Delete confirmation popups
- Unauthorized (401) middleware handling

### Architecture
- Repository & Unit of Work Pattern
- AutoMapper for mapping between entities and DTOs
- Pagination support for listings
- Standardized error and success responses
- Modular, testable, and scalable structure

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2022+ / Rider
- .NET 8 SDK
- SQL Server

## 🛠️ How to Run the Project

1. **Clone the Repository**
2. - **Set API as Startup Project**
    
    - In Visual Studio: Right-click `ProductCatalog.API` → Set as Startup Project

 **Update the Connection String:**

	Modify the connection string in two places:
	
	appsettings.json in the ProductCatalog.Api project
	
	DesignTimeDbContextFactory.cs in the ProductCatalog.Infrastructure project


- **Apply Migrations**  
    Open **Package Manager Console** and run:
	Set-Project -Name ProductCatalog.Infrastructure Update-Database
	
- **Run the API**
    
    - Launch `ProductCatalog.API`
        
    - Swagger will be available at `https://localhost:7177/swagger`
        
- **Run the MVC Client**
    
    - Set `ProductCatalog.MVC` as startup project
        
    - Hit `F5` or run from terminal:
	    dotnet run --project ProductCatalog.MVC

## 🔐 Authentication Notes

- On login, JWT token is stored in `Session`.
    
- All HttpClient services in `ProductCatalog.Service` automatically include the token in the request header using `Authorization: Bearer <token>`.
    
- Protected routes will redirect to login or show a custom `401` unauthorized page using a middleware.

## Logs

Each API request and response is logged in a `Logs` folder inside the API project. This helps in debugging and tracing.

## 📚 Additional Notes

- All services use a **standardized response** (`BaseCommandResponse<T>`) for consistency.
    
- All major logic like Unit of Work, Repository, Logger, and Pagination exist in the **Framework Layer**.
    
- The `ProductCatalog.Service` layer contains all **HttpClient service implementations** used in the MVC client to consume the API.

