cd ./E2ETests/ 
dotnet test ./E2ETests.csproj
allure serve ./bin/Debug/net8.0/allure-results 
