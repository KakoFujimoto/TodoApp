```bash
dotnet tool install --global dotnet-ef
cd .\TodoApp\
dotnet ef migrations add InitialCreate
dotnet add package Microsoft.EntityFrameworkCore.Tools

