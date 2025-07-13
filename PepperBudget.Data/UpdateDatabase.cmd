@echo off
@setlocal

cd /d %~dp0

dotnet ef database update
