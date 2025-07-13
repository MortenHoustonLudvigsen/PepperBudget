@echo off
setlocal

cd /d %~dp0

dotnet ef migrations add %1 --project PepperBudget.Data --startup-project PepperBudget.Api
