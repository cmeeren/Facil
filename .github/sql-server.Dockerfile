FROM ubuntu:22.04

# The test database uses SQL Server 2025 with full-text search.
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl ca-certificates gnupg \
    && curl -fsSL https://packages.microsoft.com/keys/microsoft.asc \
       | gpg --dearmor -o /usr/share/keyrings/microsoft.gpg

RUN curl -fsSL https://packages.microsoft.com/config/ubuntu/22.04/mssql-server-2025.list \
       -o /etc/apt/sources.list.d/mssql-server.list \
    && sed -i 's|deb \[|deb [signed-by=/usr/share/keyrings/microsoft.gpg |' /etc/apt/sources.list.d/mssql-server.list \
    && apt-get update \
    && ACCEPT_EULA=Y DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends mssql-server mssql-server-fts \
    && rm -rf /var/lib/apt/lists/*

EXPOSE 1433
CMD ["/opt/mssql/bin/sqlservr"]
