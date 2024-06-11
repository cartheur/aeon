FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy the *.csproj files.
COPY ./core .
COPY ./poly .
COPY ./run .

# Copy the folders needed to run the application.
COPY ./run/config /animals/config/
COPY ./run/dataset /animals/dataset/
COPY ./run/emotive /animals/emotive/
COPY ./run/fragments /animals/fragments/
COPY ./run/grammar /animals/grammar/
COPY ./run/logs /animals/logs/
COPY ./run/mindpixel /animals/mindpixel/
COPY ./run/nucode /animals/nucode/
COPY ./run/personality /animals/personality/
COPY ./run/python /animals/python/
COPY ./run/reductions /animals/reductions/
COPY ./run/snippets /animals/snippets/
COPY ./run/sounds /animals/sounds/
COPY ./run/tonal /animals/tonal/
COPY ./run/update /animals/update/

# Copy and publish the application and its libraries.
COPY . .
RUN dotnet restore ./run/run.csproj
RUN dotnet publish ./run/run.csproj -c debug -o /animals --no-restore

# Final stage/image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /animals
COPY --from=build /animals .
#VOLUME [ "/config" ]
#COPY ./config/Settings.xml -v /config/
ENV DOTNET_AttachStdout=1
ENV DOTNET DOTNET_CLI_TELEMETRY_OPTOUT=1
ENTRYPOINT ["dotnet", "run.dll"]
CMD ["-u"]