# Fake Quota Management System（假勤额度管理系统）

## 项目概述

一个基于 ASP.NET Core + Entity Framework Core + Oracle 的假勤额度管理系统，支持陪护假类型管理、额度管理、工信息管理和 RDP 远程桌面管理。

## 技术栈

### 后端
- **框架**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.0
- **数据库**: Oracle 23.5.0 Enterprise
- **日志**: Serilog（控制台 + 文件）
- **API 文档**: Swashbuckle/Swagger

### 前端（待开发）
- **框架**: Blazor WebAssembly
- **UI 库**: Bootstrap 5

### 部署
- **容器**: Docker + Docker Compose
- **CI/CD**: GitHub Actions
- **远程部署**: SSH

---

## 功能特性

### 1. 陪护假类型管理
- ✅ 查看所有陪护假类型
- ✅ 新增陪护假类型
- ✅ 编辑陪护假类型
- ✅ 删除陪护假类型

### 2. 额度管理
- ✅ 查看所有类型陪护假的剩余额度
- ✅ 查看当前额度使用记录
- ✅ 天额度设置
- ✅ 时额度设置
- ✅ 额度预警（低于阈值时提醒）

### 3. 工信息管理
- ✅ 查看所有工信息
- ✅ 新增工信息
- ✅ 更新工信息
- ✅ 删除工信息

### 4. RDP 远程桌面管理
- ✅ 查看 RDP 任务状态
- ✅ 创建 RDP 任务
- ✅ 编辑 RDP 任务
- ✅ 删除 RDP 任务
- ✅ 远程桌面连接监控

### 5. 签核流程
- ✅ 陪护假申请时需要签核
- ✅ 签核完成后自动更新目标表

---

## 数据库设计

### TNA_TBL_EMPLQUOTA（额度表）

| 字段名 | 类型 | 说明 |
|---------|------|---------|
| REGION_ID | VARCHAR2(33) | 区域 ID |
| QUOTA_SEQNO | NUMBER(20) | 配额序号 |
| YEAR | VARCHAR2(4) | 年份 |
| APPLICATION_TYPE | VARCHAR2(3) | 应用类型（北京/深圳/北京护理/深圳护理） |
| DAY_AMOUNT | NUMBER(10,4) | 天额度 |
| HOUR_AMOUNT | NUMBER(10,4) | 时额度 |
| QUOTA_DAY_AMOUNT | NUMBER(10,4) | 赫日额度 |
| QUOTA_HOUR_AMOUNT | NUMBER(10,4) | 赎时额度 |

### TNA_TBL_EMPVL（陪护假表）

| 字段名 | 类型 | 说明 |
|---------|------|---------|
| EMPID | VARCHAR2(6) | 员工 ID |
| ACTIVITYNAME | VARCHAR2(30) | 陪护假类型名称 |
| ACTIVITYDAY | VARCHAR2(30) | 陪护假天数 |
| JOINCERTIFICATE | VARCHAR2(100) | 关联证书 |
| STATUS | VARCHAR2(30) | 状态 |
| CREATEEMPID | VARCHAR2(30) | 创建人员 ID |
| CREATEEMPNAME | VARCHAR2(30) | 创建人员名称 |
| CREATEDATE | VARCHAR2(30) | 创建时间 |
| UPDATEEMPID | VARCHAR2(30) | 更新人员 ID |
| UPDATEEMPNAME | VARCHAR2(30) | 更新人员名称 |
| UPDATEDATE | VARCHAR2(30) | 更新时间 |
| APPLYQUOTADAYS | NUMBER(20) | 申请天数 |
| RDPPTASKID | VARCHAR2(100) | RDP 任务 ID |
| RDPPNODEACCOUNT | VARCHAR2(4000) | RDP 节点账户 |
| RDPPNODENUMBER | VARCHAR2(100) | RDP 节点名称 |
| RDPPID | VARCHAR2(100) | RDP 节点 ID |
| RDPPREVIEWERS | VARCHAR2(4000) | RDP 审阅人 |
| RDPPID | VARCHAR2(100) | 申请类型（陪护假额度管理） |

---

## 项目结构

\`\`\`
fake-quota-system/
├── backend/                   # .NET 8 后端 API
│   ├── Controllers/         # API 控制器
│   ├── Services/           # 业务逻辑服务
│   ├── Models/            # 数据模型
│   ├── Data/               # EF Core 数据访问
│   ├── Helpers/            # 辅助类
│   ├── Program.cs           # 程序入口
│   ├── appsettings.json     # 配置文件
├── frontend/                # Blazor 前端（待开发）
│   ├── Pages/              # 页面组件
│   ├── Shared/            # 共享组件
│   ├── wwwroot/           # 静态资源
│   └── Program.cs
├── docs/                   # API 文档
├── docker/                 # Docker 配置
│   ├── Dockerfile
│   ├── docker-compose.yml
│   └── docker-compose.prod.yml
├── scripts/                 # 部署脚本
└── .github/workflows/      # GitHub Actions CI/CD
\`\`\`

---

## 快速开始

### 后端开发

\`\`\`bash
# 进入后端目录
cd backend

# 还原依赖
dotnet restore

# 构建解决方案
dotnet build --configuration Release

# 运行应用
dotnet run
\`\`\`

### 部署到生产环境

\`\`\`bash
# 使用生产环境配置
docker-compose -f docker-compose.prod.yml up -d

# 查看 Oracle 数据库连接
docker-compose exec backend docker-compose -f docker-compose.prod.yml db sqlplus / as sysdba

# 查看应用日志
docker-compose logs backend -f docker-compose.prod.yml
\`\`\`

### 本地开发环境配置

复制 `.env.example` 到 `.env` 并配置：

\`\`\`bash
# Oracle 连接字符串
ConnectionStrings__OracleConnection=Data Source=(DESCRIPTION=(ADDRESS_LIST=(PROTOCOL=TCP)(HOST=your_oracle_host)(PORT=1521));User Id=SYSTEM;Password=your_password;Connection Pool Timeout=30;

# MySQL 连接字符串（如果需要）
ConnectionStrings__MySqlConnection=Server=localhost;Database=fakequota;Uid=root;Pwd=your_password;
\`\`\`

---

## API 端点

### Swagger/OpenAPI 文档
- **Swagger UI**: `http://localhost:8080/swagger`
- **API 端点**: `/api`
- **健康检查**: `/api/health`

### 主要 API

| 功能 | 端点 | 方法 | 说明 |
|------|------|------|---------|
| 陪护假类型 | GET `/api/empltype` | 获取所有类型 |
|  | GET `/api/empltype/{id}` | 获取单个类型 |
|  | POST `/api/empltype` | 创建类型 |
|  | PUT `/api/empltype/{id}` | 更新类型 |
|  | DELETE `/api/empltype/{id}` | 删除类型 |
| 额度 | GET `/api/quota` | 获取所有额度 |
|  | GET `/api/quota/empltype/{emplTypeId}` | 获取类型额度 |
|  | POST `/api/quota` | 设置额度 |
| 工信息 | GET `/api/jobs` | 获取所有工信息 |
|  | POST `/api/jobs` | 创建工信息 |
|  | PUT `/api/jobs/{id}` | 更新工信息 |
|  | DELETE `/api/jobs/{id}` | 删除工信息 |
| RDP 任务 | GET `/api/rdp` | 获取所有 RDP 任务 |
|  | POST `/api/rdp` | 创建 RDP 任务 |
| RDP 任务 | PUT `/api/rdp/{id}` | 更新 RDP 任务 |
| RDP 任务 | DELETE `/api/rdp/{id}` | 删除 RDP 任务 |

---

## 部署说明

### 开发环境
1. 克隆项目：\`git clone https://github.com/zinianly-aide/fake-quota-system.git\`\`
2. 进入项目目录：\`cd fake-quota-system\`\`
3. 还原依赖：\`dotnet restore\`\`
4. 构建项目：\`dotnet build --configuration Release\`\`
5. 运行应用：\`dotnet run\`\`

### 生产环境
1. 连接到生产服务器
2. 使用 SSH 工具或 Docker 部署
3. 运行 Docker Compose：\`docker-compose -f docker-compose.prod.yml up -d\`\`

### 数据库迁移

如果需要迁移数据库，可以运行：

\`\`\`bash
# 使用 EF Core 迁移
dotnet ef migrations add InitialCreate

# 应用迁移
dotnet ef database update

# 生成 SQL 脚本
dotnet ef migrations script
\`\`\`

---

## 注意事项

### 安全
- ✅ 所有数据库连接使用加密的连接字符串
- ✅ API 端点需要身份验证（生产环境）
- ✅ 敏感数据脱敏处理
- ✅ 定期更新 Oracle 密码

### 性能优化
- ✅ 使用 EF Core 查询缓存
- ✅ 数据库连接池配置
- ✅ API 响应压缩（大数据）
- ✅ 分页查询支持

### 日志管理
- ✅ 结构化日志（Serilog）
- ✅ 日志级别配置（开发/生产）
- ✅ 日志文件轮转（每天一个文件）
- ✅ 日志保留 30 天

### 监控
- ✅ 应用健康检查端点
- ✅ Serilog 指标收集
- ✅ Oracle 数据库监控
- ✅ Docker 容器监控

---

## 开发规范

### 代码规范
- 遵循 .NET 编码规范
- 使用异步编程模式
- 异常处理和日志记录
- 代码注释清晰

### Git 规范
- 功能分支开发（feature/*）
- 修复分支（fix/*）
- 提交信息清晰明确
- 主分支受保护（main）
- 使用 Pull Request 进行代码审查

---

## 贡献指南

欢迎贡献！请遵循以下规范：

1. Fork 项目仓库
2. 创建功能分支（feature/your-feature-name）
3. 提交 Pull Request
4. 等待代码审查通过
5. 合并到主分支

---

## 许可证

MIT License

Copyright (c) 2026 OpenClaw Agent. All rights reserved.

---

## 联系方式

如有问题或建议，请通过以下方式联系：

- GitHub Issues: [https://github.com/zinianly-aide/fake-quota-system/issues](https://github.com/zinianly-aide/fake-quota-system/issues)

---

## 更新日志

### v1.0.0 (2026-02-04)
- 初始版本
- 后端 API（ASP.NET Core 8.0 + EF Core + Oracle）
- Docker 支持（开发和生产环境）
- GitHub Actions CI/CD 工作流
- 完整的文档

---

**感谢使用 Fake Quota Management System！**
