# Guide

### CLI

```bash
dotnet new console -o dotnet-audit

cd dotnet- audit

dotnet build
-- 실행
dotnet run
```

### \*.csproj 수정

```csharp
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <RootNamespace>dotnet_audit</RootNamespace>
    <ImplicitUsings>disable</ImplicitUsings>
    <Nullable>disable</Nullable>
  </PropertyGroup>
```

### Package 설치

```bash

dotnet add package Microsoft.EntityFrameworkCore.SqlServer 7.0.1

dotnet add package Microsoft.EntityFrameworkCore.Tools 7.0.1

dotnet add package Microsoft.EntityFrameworkCore.Design 7.0.1

```


### Migration
> Package Console 에서 실행
```console
Add-Migration "AddItems"

Update-Database
```

## Temporal 옵션으로 AuditTable 생성하기

##### Temporal 모델 생성 옵션 추가하기
```csharp

public class StorageBroker : DbContext
{
    public StorageBroker() => 
        this.Database.Migrate();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DotnetAuditDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .ToTable(name: "Items", itemsTable => itemsTable.IsTemporal());
    }

    public DbSet<Item> Items { get; set; }
}
```
##### 스키마 업데이트
```bash
Add-Migration "AddItemsTemporal"
```

##### 빌드
```bash
dotnenet run --project dotnet-audit.csproj
```
