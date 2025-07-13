@echo off
@setlocal

cd /d %~dp0

rd /s /q Migrations

dotnet ef migrations add Initial
