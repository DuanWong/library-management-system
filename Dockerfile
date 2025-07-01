# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# 复制所有项目文件夹到容器里
COPY LibraryManagementSystem.BLL/ LibraryManagementSystem.BLL/
COPY LibraryManagementSystem.DAL/ LibraryManagementSystem.DAL/
COPY LibraryManagementSystem.Model/ LibraryManagementSystem.Model/
COPY LibraryManagementSystem.Common/ LibraryManagementSystem.Common/
COPY LibraryManagementSystem/ LibraryManagementSystem/

# 切换到主项目文件夹
WORKDIR /src/LibraryManagementSystem

# 还原依赖
RUN dotnet restore

# 发布项目到 /app/out
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "LibraryManagementSystem.dll"]
