# Fake Quota Management System (假勤额度管理系统)

A .NET 8.0 + Blazor WebAssembly + Oracle Database system for managing fake employee leave quotas with multi-environment support.

## 📋 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [System Architecture](#system-architecture)
- [Database Schema](#database-schema)
- [API Endpoints](#api-endpoints)
- [Frontend Structure](#frontend-structure)
- [Deployment](#deployment)
- [Development Setup](#development-setup)
- [Contributing](#contributing)

---

## 🎯 Features

### ✅ Implemented Features

#### 1. Quota Management (额度管理)
- **Multi-region support**: 北京、深圳、北京护理、深圳护理
- **Multi-apply type**: 年度/月度/时度
- **Quota allocation**: 天额度、时额度、年额度
- **Real-time monitoring**: 额度使用率监控
- **Alerting**: 额度预警功能

#### 2. Employee Leave Management (假勤管理)
- **Employee management**: 员工 ID、名称、部门
- **Leave types**: 陪护假（5天/7天/10天/15天）
- **Application workflow**: 新建申请 → 签核 → 批准 → 额度分配
- **Certificate management**: 关联证书管理
- **Status tracking**: 活跃、禁用、已删除状态

#### 3. System Features (系统功能)
- **Multi-environment**: Development、Staging、Production
- **Logging system**: Serilog 结构化日志
- **Health checks**: 健康检查端点
- **Swagger/OpenAPI**: 完整 API 文档
- **CORS support**: 跨域访问支持

---

## 🛠️ Tech Stack

### Backend
- **Framework**: .NET 8.0
- **API**: ASP.NET Core Web API
- **ORM**: Entity Framework Core
- **Database**: Oracle Database
- **Logging**: Serilog + Serilog.Sinks.Console
- **Documentation**: Swagger/OpenAPI (Swashbuckle.AspNetCore.SwaggerGen)
- **Configuration**: Microsoft.Extensions.Configuration

### Frontend
- **Framework**: Blazor WebAssembly
- **UI Library**: Bootstrap 5.3
- **HTTP Client**: Microsoft.Extensions.Http.Json
- **Runtime**: .NET 8.0 WebAssembly

### Infrastructure
- **Containerization**: Docker + Docker Compose
- **Web Server**: Nginx (for Blazor Wasm)
- **Database**: Oracle Database (containerized)
- **Cache**: Redis (containerized)
- **Reverse Proxy**: Nginx

### DevOps
- **Version Control**: Git + GitHub
- **CI/CD**: GitHub Actions
- **Environment Management**: Multiple configurations (dev/staging/prod)

---

## 🏗️ System Architecture

```
┌─────────────┐      ┌──────────────┐      ┌──────────────┐      ┌──────────────┐
│             │      │              │      │              │      │
│   Blazor   │      │   .NET 8 API │      │    Oracle    │
│  Wasm UI   │◄────▶│              │◄────▶│              │◄────▶│
│             │      │              │      │              │      │
│  (Frontend)  │      │   (Backend)    │      │ (Database)   │
└─────────────┘      └──────────────┘      └──────────────┘      └──────────────┘

┌─────────────┐
│             │
│   Nginx     │
│  (Proxy)     │
│   :8080      │
└─────────────┘
```

### Project Structure

```
fake-quota-system/
├── backend/                  # .NET 8.0 Backend API
│   ├── Controllers/         # API controllers
│   ├── Services/           # Business logic services
│   ├── Models/            # Data models
│   ├── Data/               # EF Core data access
│   ├── Helpers/            # Helper classes
│   ├── Program.cs           # Program entry point
│   └── appsettings.json     # Configuration
├── frontend/                # Blazor WebAssembly frontend
│   ├── Pages/              # Razor pages
│   ├── Shared/             # Shared components
│   ├── wwwroot/           # Static files
│   └── Program.cs
├── docker/                 # Docker configurations
│   ├── Dockerfile.backend   # Backend Dockerfile
│   ├── Dockerfile.frontend  # Frontend Dockerfile
│   └── docker-compose.yml    # Multi-service compose
├── docs/                   # API documentation (Swagger)
├── scripts/                 # Deployment scripts
└── .github/workflows/      # GitHub Actions CI/CD
```

---

## 📊 Database Schema

### TNA_TBL_EMPLQUOTA (额度表)
- `Id` (Long) - Primary key
- `RegionId` (String) - 区域 ID (BJ/SZ)
- `QuotaSeqNo` (Integer) - 配额序号
- `Year` (Integer) - 年份
- `ApplicationType` (String) - 应用类型
- `DayAmount` (Decimal) - 天额度
- `HourAmount` (Decimal) - 时额度
- `QuotaDayAmount` (Decimal) - 年额度
- `QuotaHourAmount` (Decimal) - 时额度
- `Remarks` (String) - 备注

### TNA_TBL_EMPVL (假勤表)
- `Id` (Long) - Primary key
- `EmpId` (String) - 工 ID
- `ActivityName` (String) - 陪护假名称
- `ActivityDay` (String) - 陪护假天数
- `Certificate` (String) - 关联证书
- `Status` (String) - 状态 (Active/Deleted)
- `CreateEmpId` (String) - 创建人员 ID
- `CreateEmpName` (String) - 创建人员名称
- `CreateDate` (DateTime) - 创建时间
- `UpdateEmpId` (String) - 更新人员 ID
- `UpdateEmpName` (String) - 更新人员名称
- `UpdateDate` (DateTime) - 更新时间
- `ApplyQuotaDays` (Integer) - 申请天数
- `Rdptaskid` (String) - RDP 任务 ID
- `Rdppnodeaccount` (String) - RDP 节点账户
- `Rdppnodenameber` (String) - RDP 节点名称
- `Rdppid` (String) - RDP 节点 ID
- `Rdppreviewers` (String) - RDP 审阅人
- `Rdppid` (String) - 申请类型 (陪护假/额度管理)

---

## 🔌 API Endpoints

### Health Endpoints
- `GET /api/health` - Health check
  - Returns system status and feature summary

### Quota Management Endpoints
- `GET /api/emplquota/all` - Get all quota types
- `GET /api/emplquota/{id}` - Get quota type by ID
- `POST /api/emplquota/create` - Create new quota type
- `PUT /api/emplquota/{id}` - Update quota type
- `DELETE /api/emplquota/{id}` - Delete quota type

### Employee Leave Endpoints
- `GET /api/empvl/all` - Get all employees
- `GET /api/empvl/active` - Get active employees
- `POST /api/empvl/create` - Create new employee
- `PUT /api/empvl/{id}/update` - Update employee
- `DELETE /api/empvl/{id}` - Delete employee

### Application Service Endpoints
- `GET /api/application/summary` - Get system summary
- `GET /api/application/pending` - Get pending approvals
- `POST /api/application/new` - Create new application
- `POST /api/application/approve` - Approve application
- `POST /api/application/update-usage` - Update quota usage

---

## 🎨 Frontend Structure

### Pages (前端页面)
- `Index.razor` - Dashboard (系统概览)
  - 额度概览
  - 员工管理统计
  - 系统状态

- `EmplQuota.razor` - Quota Management (额度管理)
  - 额度类型列表
  - 编辑/删除额度类型
  - 额度使用预警

- `Empvl.razor` - Employee Leave Management (假勤管理)
  - 员工列表
  - 编辑/删除员工
  - 签核管理

- `QuotaUsage.razor` - Quota Usage (额度使用记录)
  - 额度使用统计
  - 区域使用分布
  - 使用率分析

- `NewApplication.razor` - New Application (新建申请)
  - 新增陪护假申请
  - 申请表单
  - 审核流程说明

### Shared Components
- Navigation menu
- Status indicators
- Alert modals
- Data tables

---

## 🚀 Deployment

### Development Environment
```bash
# Clone repository
git clone https://github.com/zinianly-aide/fake-quota-system.git

# Navigate to project directory
cd fake-quota-system

# Start services with docker-compose
docker-compose up -d

# Access application
# Frontend: http://localhost:8081
# Backend API: http://localhost:8080
# Swagger UI: http://localhost:8080/swagger
```

### Staging Environment
```bash
# Set environment variables
export ASPNETCORE_ENVIRONMENT=Staging
export ConnectionStrings__OracleConnection=${ORACLE_CONNECTION_STAGING}
export Serilog__MinimumLevel=Warning

# Build and deploy
docker-compose -f docker-compose.yml up -d
```

### Production Environment
```bash
# Set environment variables
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__OracleConnection=${ORACLE_CONNECTION_PRODUCTION}
export Serilog__MinimumLevel=Error

# Build and deploy
docker-compose -f docker-compose.yml up -d
```

### Environment Switching
```bash
# Switch to development
export ASPNETCORE_ENVIRONMENT=Development

# Switch to production
export ASPNETCORE_ENVIRONMENT=Production

# Reload application
docker-compose restart backend frontend
```

---

## 🛠️ Development Setup

### Prerequisites
- **.NET 8.0 SDK**: Download and install
- **Docker**: Install Docker Desktop
- **Oracle Database**: Oracle Database instance
- **Git**: Git command line tools

### Local Development (Without Docker)
```bash
# Navigate to backend directory
cd backend

# Restore dependencies
dotnet restore

# Build project
dotnet build --configuration Release

# Run application
dotnet run

# Access Swagger UI
# http://localhost:8080/swagger
```

### Frontend Development
```bash
# Navigate to frontend directory
cd frontend

# Restore dependencies
dotnet restore

# Run Blazor application
dotnet watch run

# Access application
# http://localhost:8081
```

### Database Configuration
```bash
# Configure Oracle connection
export ORACLE_CONNECTION="Data Source=(DESCRIPTION=(ADDRESS_LIST=(localhost:1521))(CONNECT_DATA=(HOST=1521)(PORT=1521))(SERVICE_NAME=ORCL);User Id=system;Password=oracle1234;"

# Test connection
cd backend
dotnet run
```

---

## 📝 API Documentation

### Swagger UI
- **Production**: `http://your-domain.com/swagger`
- **Staging**: `http://staging.your-domain.com/swagger`
- **Development**: `http://localhost:8080/swagger`

### API Examples

#### Get All Quota Types
```bash
curl -X GET "http://localhost:8080/api/emplquota/all" \
  -H "accept: application/json"
```

#### Create New Quota Type
```bash
curl -X POST "http://localhost:8080/api/emplquota/create" \
  -H "Content-Type: application/json" \
  -d '{
    "regionId": "BJ",
    "quotaSeqNo": 1,
    "year": 2025,
    "applicationType": "北京",
    "dayAmount": 365,
    "hourAmount": 365 * 24,
    "quotaDayAmount": 365,
    "quotaHourAmount": 365 * 24 * 60,
    "remarks": "北京年度额度"
  }'
```

#### Get Employee Summary
```bash
curl -X GET "http://localhost:8080/api/application/summary" \
  -H "accept: application/json"
```

---

## 🤝 Contributing

### Code Style
- Follow C# coding standards
- Use meaningful variable and method names
- Add XML documentation to public methods
- Keep methods small and focused

### Pull Request Process
1. Fork the repository
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Make your changes
4. Commit changes (`git commit -m "Add your feature"`)
5. Push to the branch (`git push origin feature/your-feature`)
6. Create a Pull Request

### Code Review Checklist
- Code follows coding standards
- Code is properly formatted
- API endpoints are tested
- Database migrations are included
- Documentation is updated

### Issue Reporting
- Use GitHub Issues for bug reports
- Include steps to reproduce
- Include expected behavior
- Include actual behavior

---

## 📊 Environment Variables

### Application Configuration
- `ASPNETCORE_ENVIRONMENT` - Environment (Development/Staging/Production)
- `ConnectionStrings__OracleConnection` - Oracle database connection string
- `Serilog__MinimumLevel` - Minimum log level (Debug/Info/Warning/Error)
- `ORACLE_PWD` - Oracle password file location
- `ORACLE_SID` - Oracle system identifier

### Example `.env` file
```bash
# Environment
ASPNETCORE_ENVIRONMENT=Development

# Database Connection
ConnectionStrings__OracleConnection=Data Source=(DESCRIPTION=(ADDRESS_LIST=(localhost:1521))(CONNECT_DATA=(HOST=1521)(PORT=1521))(SERVICE_NAME=ORCL);User Id=system;Password=oracle1234;

# Logging
Serilog__MinimumLevel=Information
```

---

## 📚 License

MIT License

## 📧 Contact

For support and questions, please open an issue in the GitHub repository.

## 🙏 Acknowledgments

- **.NET 8.0** - Microsoft
- **Blazor** - Microsoft
- **Bootstrap** - Bootstrap
- **Oracle Database** - Oracle Corporation
- **Entity Framework Core** - Microsoft
- **Serilog** - Serilog
- **Swagger/OpenAPI** - Swashbuckle
- **Docker** - Docker, Inc.

---

**Project created by OpenClaw Agent**
**Version**: 1.0.0
**Last Updated**: 2026-02-04
