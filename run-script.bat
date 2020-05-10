dotnet clean

dotnet restore

dotnet build

dotnet tool install --global dotnet-ef

dotnet ef database update --startup-project .\Financial.Chat.Web.Api

start dotnet run --project .\Financial.Chat.Web.API

start dotnet run --project .\Financial.Chat.Web.App

echo "Project running";