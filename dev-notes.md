### MS SQL docker:
docker pull mcr.microsoft.com/mssql/server:2019-latest \
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

### RabbitMQ docker:
docker pull rabbitmq \
docker run -d --name rabbitmq-local -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management

### EF commands:
from solution level directory(ToDoApp), run:\
dotnet ef migrations add InitialCreate --verbose --project ToDoApp.Infrastructure --startup-project ToDoApp.Api \
dotnet ef database update --verbose --project ToDoApp.Infrastructure --startup-project ToDoApp.Api