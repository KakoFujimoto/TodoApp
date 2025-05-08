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

dotnet ef migrations add AddBodyToTodoTask
dotnet ef database update

dotnet ef migrations add LimitTitleAndBodyLength
dotnet ef database update

cd .\TodoApp\
libman init
libman restore

dotnet ef migrations add AddIsCompletedToTodoTask
dotnet ef migrations add CompleteTaskFix
dotnet ef database update


dotnet add package Microsoft.AspNetCore.Mvc.Testing

```
