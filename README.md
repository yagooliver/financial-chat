# Financial Chat

[![Build status](https://ci.appveyor.com/api/projects/status/rl2ja69994rt3ei6?svg=true)](https://ci.appveyor.com/project/yagooliver/financial-chat)


What is the Financial chat Project?
=====================
Is a simple browser-based chat application using .NET core that use signalR to make the communication with logged users. Also, the user can make calls using the code "/stock=stock_code" to know the stock price.

## Requirements

- If you're running on visual studio you will need the Visual Studio 2019 Version 16.5.3 or High.
- .NET Core SDK 3.1
- The SDK and tools can be downloaded from https://dot.net/core.
- Docker
- Linux or Windows with Hyper-V enable

## Technologies implemented:

- ASP.NET Core 5.0
- ASP.NET WebApi Core with JWT Bearer Authentication
- ASP.NET Identity Core
- ASP.NET CORE SIGNALR
- WORKER
- BLAZOR
- Entity Framework Core 5.0
- .NET Core Native DI
- AutoMapper
- FluentValidator
- MediatR
- Swagger UI with JWT support
- Docker
- RabbitMQ
- MassTransit

## Architecture:

- SOLID
- Domain Driven Design (Layers and Domain Model Pattern)
- Domain Notification
- CQRS
- Unit of Work
- Repository and Generic Repository

## Instructions (using docker)
To run this application you just have to execute the "docker-compose build" command on base directory of the project and then execute "docker-compose up -d". These commands wil run the application and automatically. After running the web api is accessed by http://localhost:8082/swagger and web app by http://localhost:8080

## Other instructions

To run this application you just have to execute the "run-script.bat" on base directory of the project. This script will run the dotnet cli commands to run the application and automatically open chrome navigator(normal mode and incognito mode). After running the web api is accessed by https://localhost:5001/swagger and web app by https://localhost:5002

Also, you can run the solution open cmd on base directory and execute the following commands:

- dotnet clean
- dotnet restore
- dotnet build
- dotnet tool install --global dotnet-ef
- dotnet ef database update --startup-project .\Financial.Chat.Web.Api
- start dotnet run --project .\Financial.Chat.Web.API
- start dotnet run --project .\Financial.Chat.Web.App
- start dotnet run --project .\FinancialChat.MessageBroker

OBS: In this case, you may have to change the links in the class ChatService(Financial.Chat.Web.App) and appsettings.Development(FinancialChat.MessageBroker).
Because the API starts using 5001 port and you have to install the rabbitmq in your machine, you also can use docker for that.

If you are running in visual studio, follow the steps bellow:
- Make sure you have the instance of SQL SERVER started
- Start installed RabbitMQ or run it with docker command (docker run --rm -it --hostname your-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management) 
- Start the Financial.Chat.Web.API
- Start The Financial.Chat.Web.App
- Start The FinancialChat.MessageBroker

## Observations
- If web api have been executing in another port, you must change de url port on ChatService class in namespace Financial.Chat.Web.App.Data
and the api url in appsettings.{enviroment}.json on FinancialChat.MessageBroker
- Is necessary to open in two different browsers or in normal mode and other tab in incognito mode because the token used for authentication is saved on localstorage for make the requests on Web API
- Make sure that you are using the latest browser version (Chrome, Firefox, Microsoft Edge)
