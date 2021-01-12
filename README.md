# efurni

```
dotnet new sln -o efurni
dotnet new webapi -o efurni-api
dotnet sln add efurni-api
dotnet new classlib -o efurni-shared
dotnet sln add efurni-shared
dotnet new webapp -o efurni-web
dotnet sln add efurni-web
dotnet add reference ../efurni-shared
```
