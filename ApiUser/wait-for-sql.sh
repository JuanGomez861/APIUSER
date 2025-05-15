#!/bin/bash

# Esperar hasta que SQL Server esté listo
until /opt/mssql-tools/bin/sqlcmd -S $1 -U $2 -P $3 -Q "SELECT 1" &>/dev/null
do
  echo "Esperando a que SQL Server esté listo..."
  sleep 2
done

echo "SQL Server está listo. Continuando con la ejecución..."
exec "$@"
