# 🌍 Geo-Event RESTful API

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker)
![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)

A production-ready, highly optimized backend service designed for location-based event management. This project serves as a showcase of modern backend engineering practices, demonstrating advanced spatial query processing, strict enterprise security, and containerized deployment.

## 🚀 Key Features & Architectural Highlights

* **Geospatial Queries (NetTopologySuite):** Performs high-performance, database-level spatial logic. It calculates distances and filters events within a precise radius directly on the SQL Server side using geographic points (Latitude/Longitude).
* **JWT Authentication & Claims-Based Authorization:** Secure endpoint protection with JSON Web Tokens. Includes fully decoupled registration, password hashing, and user authentication with runtime identity claims extraction (`[Authorize]`).
* **Enterprise Validation (FluentValidation):** Features robust request validation layers. It intercepts bad data (e.g., impossible geographic coordinates, past date rejections) before it hits controllers or the database.
* **Global Exception Handling Middleware:** Implements a centralized interceptor that safely catches unhandled runtime errors, returning standard, secure JSON Problem Details (`RFC 7231`) while hiding sensitive stack traces from users.
* **Smart Pagination & API Wrapper:** Utilizes optimized generic wrapper classes (`PagedResponse<T>`) to distribute thousands of geolocation records efficiently without overloading client-side applications.
* **Self-Documenting OpenAPI/Swagger:** Tailored Swagger UI integration including XML code comments, interactive sample payloads, and custom `SecurityDefinitions` for testing JWT Bearer tokens natively in the browser.

## 🛠 Tech Stack

* **Framework:** ASP.NET Core Web API (.NET 8)
* **Database:** Microsoft SQL Server 2022
* **ORM:** Entity Framework Core (Code-First)
* **Spatial Data:** NetTopologySuite.SqlServer.Topology
* **Validation:** FluentValidation.AspNetCore
* **Containerization:** Docker & Docker Compose Orchestration

## 🐳 Quick Start & Deployment (Docker Compose)

The entire infrastructure (the Web API and the SQL Server database instance) is fully containerized and can be launched with a single command. 

### Prerequisites
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.

### Installation Steps

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/FarizShukiurov/Geo-Event-API.git](https://github.com/FarizShukiurov/Geo-Event-API.git)
    cd GeoEventApi
    ```

2.  **Spin up the containers:**
    ```bash
    docker-compose up -d --build
    ```

3.  **Explore the API:**
    Once the containers are healthy, open your browser and navigate to the interactive Swagger documentation:
    👉 **`http://localhost:8080/swagger`**

## 📌 Core API Endpoints

### 🔑 Authentication
* `POST /api/auth/register` — Register a new account (returns access token).
* `POST /api/auth/login` — Authenticate credentials and receive a JWT.
* `GET /api/auth/me` — Retrieve current authenticated session profile `[Protected]`.

### 📍 Event Management
* `POST /api/events` — Create a new localized event with spatial coordinates `[Protected]`.
* `GET /api/events/nearby` — Query events filtered by distance radius (meters) relative to a specific Longitude/Latitude point, including full server-side pagination parameters.

---
*Maintained and developed with high-quality coding principles. Available for professional freelance opportunities and contract-based backend architecture roles.*
