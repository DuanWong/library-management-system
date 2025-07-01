# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# ����������Ŀ�ļ��е�������
COPY LibraryManagementSystem.BLL/ LibraryManagementSystem.BLL/
COPY LibraryManagementSystem.DAL/ LibraryManagementSystem.DAL/
COPY LibraryManagementSystem.Model/ LibraryManagementSystem.Model/
COPY LibraryManagementSystem.Common/ LibraryManagementSystem.Common/
COPY LibraryManagementSystem/ LibraryManagementSystem/

# �л�������Ŀ�ļ���
WORKDIR /src/LibraryManagementSystem

# ��ԭ����
RUN dotnet restore

# ������Ŀ�� /app/out
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "LibraryManagementSystem.dll"]
