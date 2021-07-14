# dasteam Revolution 

## Backend

Benutzte Technologien:

* [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)
* [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio)
* [Entity Framework Core 5](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
* [SQL Server](https://en.wikipedia.org/wiki/Microsoft_SQL_Server)

## Getting Started

Das Repo mit folgendem Kommando auschecken:

`git clone --recursive https://dasteam-entwicklung@dev.azure.com/dasteam-entwicklung/Revolution/_git/Revolution revolution`

Falls das Auschecken via IDE gewünscht ist, einfach sicherstellen, dass rekursiv geklont wird, damit auch alle git submodules initialisiert werden!

## Test & Build

Das Backend-Projekt zu builden ist ganz einfach:

* Unit Tests ausführen 
* * `dotnet test DasTeamRevolution.sln`
* * (oder via IDE)
* Falls Tests nicht erfolgreich, potentielle Fehler korrigieren (TDD Teststubs (falls vorhanden) in Ruhe lassen)
* Projekt im Release-Modus builden
* * `dotnet build -c Release DasTeamRevolution.sln -o bin`

### Docker

Das Backend kann auch mit Docker und docker-compose ausgeführt werden. 

Backend starten geht wie folgt:

* `cd {REPO_ORDNER}/src`
* `docker-compose build`
* `docker-compose up`
    
## Weitere Infos

Auf dem [Wiki](https://dev.azure.com/dasteam-entwicklung/Revolution/_wiki/wikis/Revolution.wiki/3/%C3%9Cbersicht) findet man 
detaillierte und ausführliche Projektinformationen, Code-Konventionen, Regeln, usw...