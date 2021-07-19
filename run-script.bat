dotnet clean

dotnet restore

dotnet build

dotnet tool install --global dotnet-ef

dotnet ef database update --startup-project .\Financial.Chat.Web.Api

start dotnet run --project .\Financial.Chat.Web.API

start dotnet run --project .\Financial.Chat.Web.App

start dotnet run --project .\FinancialChat.MessageBroker

echo "Chrome will start in"

timeout 10

start chrome https://localhost:5002/login

start chrome -incognito https://localhost:5002/login

echo "Project running";