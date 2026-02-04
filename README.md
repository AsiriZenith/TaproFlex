# Tapro Flex – .NET 8 Microservices E-Commerce Platform

Tapro Flex is a **real-world e-commerce backend system** designed and implemented using **.NET 8 Microservices architecture**.  
The project focuses on **scalability, maintainability, and event-driven design**, following modern backend engineering best practices.

This repository demonstrates how production-grade microservices can be built using **Domain-Driven Design (DDD), CQRS, Clean Architecture**, and **event-based communication**.

---

## 🧩 Project Overview

Tapro Flex is composed of multiple independent microservices, each responsible for a specific business capability:

- **Catalog Service** – Product management
- **Basket Service** – Shopping cart & checkout
- **Discount Service** – Discount calculations via gRPC
- **Ordering Service** – Order lifecycle and fulfillment
- **YARP API Gateway** – Routing, rate limiting, and gateway concerns
- **Shopping Web App** – Client-facing web application

All services are **containerized using Docker** and orchestrated with **Docker Compose**.

---

## 🏗️ Architecture Highlights

- Microservices Architecture
- API Gateway Pattern using **YARP**
- Event-Driven Architecture with **RabbitMQ**
- **Saga Pattern** for distributed workflows
- **Transactional Outbox Pattern** for data consistency
- **CQRS** (Command Query Responsibility Segregation)
- **Clean Architecture & Vertical Slice Architecture**
- **Domain-Driven Design (DDD)**
- Asynchronous communication between services

---

## 🔧 Technologies Used

### Backend & Frameworks
- .NET 8
- ASP.NET Core Web API
- C#
- Carter (Minimal API routing)
- MediatR
- Mapster
- MassTransit

### Communication
- REST APIs
- gRPC
- RabbitMQ (Event Bus)

### Datastores
- PostgreSQL – Catalog & Basket services
- SQL Server – Ordering service
- SQLite – Discount service
- Redis – Distributed caching (Basket)

### Infrastructure
- Docker
- Docker Compose
- YARP API Gateway

---

## 🔁 Service Communication Flow

1. Client requests enter through **YARP API Gateway**
2. Gateway routes requests to the appropriate microservice
3. Services communicate:
   - Synchronously via REST or gRPC
   - Asynchronously via RabbitMQ events
4. **Transactional Outbox** ensures events are reliably published
5. **Saga Pattern** coordinates long-running business processes
6. Data consistency is achieved via **eventual consistency**

---

## 📦 Microservices Breakdown

### Catalog Service
- Manages product data
- Uses PostgreSQL
- Implements **Vertical Slice Architecture**
- Uses **Marten** as a document database

### Basket Service
- Handles shopping cart operations
- Uses Redis for fast access
- Publishes `BasketCheckoutIntegrationEvent`

### Discount Service
- Provides discount data via **gRPC**
- Uses SQLite
- Follows **N-Layer Architecture**

### Ordering Service
- Core business logic (behavior-centric domain)
- Uses **DDD Aggregates & Domain Events**
- Implements **Clean Architecture**
- Uses SQL Server
- Coordinates order lifecycle using **Saga Pattern**

---

## 🚀 Running the Project

### Prerequisites
- Docker Desktop
- .NET 8 SDK

### Run with Docker Compose
```bash
docker-compose up --build
