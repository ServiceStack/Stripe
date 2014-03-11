
REM SET BUILD=Debug
SET BUILD=Release

COPY ..\src\Stripe\bin\Release\Stripe.* "..\NuGet\ServiceStack.Stripe\lib\net40"
COPY ..\src\Stripe.Pcl\bin\Release\Stripe.* "..\NuGet\ServiceStack.Stripe.Pcl\lib\portable-net45+win8+monotouch+monoandroid"
