@echo off
@setlocal

cd /d %~dp0

dotnet ef migrations add %*
