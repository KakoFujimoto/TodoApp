```bash
dotnet tool install --global dotnet-ef
cd .\TodoApp\
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet build
dotnet ef migrations add InitialCreate
dotnet ef database update
