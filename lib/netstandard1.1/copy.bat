SET BUILD=Debug
REM SET BUILD=Release

COPY ..\..\..\ServiceStack.Text\src\ServiceStack.Text\bin\%BUILD%\netstandard1.1\ServiceStack.Text.* .\
COPY ..\..\..\ServiceStack\src\ServiceStack.Interfaces\bin\%BUILD%\netstandard1.1\ServiceStack.Interfaces.* .\
COPY ..\..\..\ServiceStack\src\ServiceStack.Client\bin\%BUILD%\netstandard1.1\ServiceStack.Client.* .\

