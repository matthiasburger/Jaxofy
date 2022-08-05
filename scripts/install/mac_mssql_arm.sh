docker pull mcr.microsoft.com/azure-sql-edge:latest

docker run -d --name Dev -p 1433:1433 -e ACCEPT_EULA=1 -e 'MSSQL_SA_PASSWORD=Pa$$w0rd' -e MSSQL_PID=Developer mcr.microsoft.com/azure-sql-edge