@echo off
@setlocal

cd /d %~dp0

psql -U postgres -f RecreateDatabase.sql

call ReplaceMigrations.cmd
dotnet ef database update
