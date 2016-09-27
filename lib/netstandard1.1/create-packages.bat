@echo off
SET BUILD=Debug
REM SET BUILD=Release

SET NOBUILD=--no-build

PUSHD ..\..\..\Stripe\src\Stripe\
dotnet pack %NOBUILD% --configuration %BUILD%
POPD
COPY ..\..\..\Stripe\src\Stripe\bin\%BUILD%\Stripe.1.0.0.* .\packages\
