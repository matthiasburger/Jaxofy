FROM mcr.microsoft.com/dotnet/sdk:5.0 
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
EXPOSE 80/tcp
EXPOSE 443/tcp
ENTRYPOINT ["dotnet", "run"]